using System;
using Unity.Entities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unity.MegaCity.Audio
{
    /// <summary>
    /// Defines the ECSound emitter type and parameters
    /// such as distance, volume, and probability
    /// </summary>

    public class SoundEmitterDefinitionAsset : ScriptableObject
    {
        public ECSoundEmitterDefinition data;

        [NonSerialized]
        private Entity definitionEntity;

        internal Entity GetEntity(EntityManager entityManager)
        {
            // Avoid error message "All entities passed to EntityManager must exist. One of the entities has already been destroyed or was never created." caused by calling ValidateEntity from EntityDataManager.Exists
            if (definitionEntity.Index >= entityManager.EntityCapacity)
                return Entity.Null;

            if (definitionEntity != Entity.Null)
                return entityManager.Exists(definitionEntity) ? definitionEntity : Entity.Null;

            definitionEntity = entityManager.CreateEntity();
            //entityManager.AddComponentData(definitionEntity, data);
            return definitionEntity;
        }

        public Entity Reflect(World world)
        {
            if (!world.IsCreated)
                return Entity.Null;
            EntityManager entityManager = world.EntityManager;
            // These extra checks are needed to guard against MonoBehaviour destruction order that would otherwise cause errors in scenes like _SoundObjects
            var entity = GetEntity(entityManager);
            if (entity != Entity.Null && entityManager.HasComponent<ECSoundEmitterDefinition>(entity))
                //entityManager.SetComponentData(entity, data);
                return entity;
            return Entity.Null;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Sound Emitter Definition Asset")]
        public static void CreateAsset()
        {
            var asset = ScriptableObject.CreateInstance<SoundEmitterDefinitionAsset>();

            asset.data.probability = 100.0f;
            asset.data.volume = 0.5f;
            asset.data.coneAngle = 360.0f;
            asset.data.coneTransition = 0.0f;
            asset.data.minDist = 5.0f;
            asset.data.maxDist = 100.0f;

            AssetDatabase.CreateAsset(asset, "Assets/SoundEmitterDefinition.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
#endif
    }
}
