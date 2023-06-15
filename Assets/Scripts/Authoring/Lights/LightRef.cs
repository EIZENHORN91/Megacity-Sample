using Unity.Entities;
using UnityEngine;

namespace Unity.MegaCity.Lights
{
    /// <summary>
    /// Creates SharedLight component, used later by the light pool system
    /// </summary>
    public class LightRef : MonoBehaviour
    {
        public GameObject LightReference;
    }

    [BakingVersion("Julian", 2)]
    public class LightRefBaker : Baker<LightRef>
    {
        public override void Bake(LightRef authoring)
        {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            AddSharedComponentManaged(entity, new SharedLight { Value = authoring.LightReference });
        }
    }
}
