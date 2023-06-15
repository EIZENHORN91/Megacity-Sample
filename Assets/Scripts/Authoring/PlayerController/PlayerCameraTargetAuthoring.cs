using Unity.Entities;
using Unity.MegaCity.CameraManagement;
using UnityEngine;

namespace Unity.MegaCity.Gameplay
{
    /// <summary>
    /// Create tag component for the player camera target
    /// </summary>
    public class PlayerCameraTargetAuthoring : MonoBehaviour
    {
    }

    [BakingVersion("Julian", 2)]
    public class PlayerCameraTargetBaker : Baker<PlayerCameraTargetAuthoring>
    {
        public override void Bake(PlayerCameraTargetAuthoring authoring)
        {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            AddComponent<PlayerCameraTarget>(entity);
        }
    }
}
