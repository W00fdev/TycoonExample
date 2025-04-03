using _Project.Scripts.Components;
using _Project.Scripts.Infrastructure.Pools;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Factories
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class EnemyFactory
    {
        private readonly ObjectPoolService _poolService;
        private readonly Enemy.EnemySceneReferences _sceneReferences;

        public EnemyFactory(ObjectPoolService poolService, Enemy.EnemySceneReferences sceneReferences)
        {
            _poolService = poolService;
            _sceneReferences = sceneReferences;
        }
        
        // TODO: defense factory with types of enemy or configs set
        public Enemy Next(Vector3 position)
        {
            var enemyObject = _poolService.Next(ObjectPoolService.DefenceType.Enemy1);
            enemyObject.transform.position = position;
            
            var enemy = enemyObject.GetComponent<Enemy>();
            if (enemy.IsInitialized == false)
                enemy.Initialize(_sceneReferences);

            return enemy;
        }
    }
}