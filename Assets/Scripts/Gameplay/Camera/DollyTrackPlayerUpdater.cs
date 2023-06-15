using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.MegaCity.Gameplay;
using Unity.Transforms;

namespace Unity.MegaCity.CameraManagement
{
    /// <summary>
    /// Reads the transform data copied from the dolly cart game object and moves the player accordingly
    /// </summary>

    [BurstCompile]
    internal partial struct DollyTrackPlayerUpdaterJob : IJobEntity
    {
        public float3 DollyTrackPosition;
        public quaternion DollyTrackRotation;
        public float DeltaTime;

        public void Execute(
            ref LocalTransform localTransform,
            in PlayerVehicleSettings playerVehicleSettings)
        {
            if (math.distancesq(localTransform.Position, DollyTrackPosition) > playerVehicleSettings.TargetSqLerpThreshold)
            {
                localTransform.Position = DollyTrackPosition;
            }

            localTransform.Position = math.lerp(localTransform.Position, DollyTrackPosition,
                DeltaTime * playerVehicleSettings.TargetFollowDamping);
            localTransform.Rotation = math.slerp(localTransform.Rotation, DollyTrackRotation,
                DeltaTime * playerVehicleSettings.TargetFollowDamping);
        }
    }


    [BurstCompile]
    [UpdateBefore(typeof(PlayerCameraTargetUpdater))]
    public partial struct DollyTrackPlayerUpdater : ISystem
    {

        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = state.WorldUnmanaged.Time.DeltaTime;

            if (HybridCameraManager.Instance == null)
                return;

            if (HybridCameraManager.Instance.m_CameraTargetMode != HybridCameraManager.CameraTargetMode.DollyTrack)
                return;

            var dollyTrackRotation = HybridCameraManager.Instance.GetDollyCameraRotation();
            var dollyTrackPosition = HybridCameraManager.Instance.GetDollyCameraPosition();
            var dollyTrackPlayerUpdaterJob = new DollyTrackPlayerUpdaterJob
            {
                DollyTrackPosition = dollyTrackPosition,
                DollyTrackRotation = dollyTrackRotation,
                DeltaTime = deltaTime
            };

            state.Dependency = dollyTrackPlayerUpdaterJob.ScheduleParallel(state.Dependency);
        }
    }
}
