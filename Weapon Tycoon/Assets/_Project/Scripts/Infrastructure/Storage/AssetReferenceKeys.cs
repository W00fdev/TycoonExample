using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.Storage
{
    public static class AssetReferenceKeys
    {
        public static readonly List<string> AssetReferenceSpawnerKeys = new()
        {
            AssetReferences.Pistol1Prefab,
            AssetReferences.Shotgun1Prefab,
            AssetReferences.Rifle1Prefab,
        };
        
        public static readonly List<string> AssetReferenceEconomyKeys = new()
        {
            AssetReferences.BoxPrefab,
            AssetReferences.LongBoxPrefab,
            AssetReferences.MoneyTextPrefab,
        };
        
        public static readonly List<string> AssetReferenceDefenseKeys = new()
        {
            AssetReferences.DefaultProjectilePrefab,
            AssetReferences.ProjectilePrefab,
            AssetReferences.ExplosionPrefab,
            AssetReferences.Enemy1Prefab,
        };
    }
}