// #define DRAW_AUDIO_GIZMOS // <- get gizmos for changing cone properties, min/max distances in sound emitters (probably only relevant for small scenes).

using Unity.Entities;

namespace Unity.MegaCity.Audio
{
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    [UpdateInGroup(typeof(PostBakingSystemGroup))]
    [BakingVersion("Abdul", 1)]
    internal partial class SoundEmitterBakingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref SoundEmitterBakingData soundEmitterBakingData) =>
            {
                var data = soundEmitterBakingData.Authoring.Value;
                SoundEmitter emitter = new SoundEmitter();

                emitter.position = data.transform.position;
                emitter.coneDirection = -data.transform.right;

                if (data.definition != null)
                {
                    emitter.definitionIndex = data.definition.data.definitionIndex;
                    data.definition.Reflect(World);
                }

                EntityManager.AddComponentData(entity, emitter);

            }).WithStructuralChanges().Run();
        }
    }
}
