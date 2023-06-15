using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Unity.MegaCity.Gameplay
{
    /// <summary>
    /// Add required tag components for cosmetic physics
    /// </summary>
    public class PlayerVehicleCosmeticPhysicsAuthoring : MonoBehaviour
    {
    }

    [BakingVersion("Julian", 2)]
    public class PlayerVehicleCosmeticPhysicsBaker : Baker<PlayerVehicleCosmeticPhysicsAuthoring>
    {
        public override void Bake(PlayerVehicleCosmeticPhysicsAuthoring authoring)
        {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            AddComponent<PlayerVehicleCosmeticPhysics>(entity);
        }
    }
}
