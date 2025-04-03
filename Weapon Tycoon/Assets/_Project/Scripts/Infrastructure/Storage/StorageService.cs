using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace _Project.Scripts.Infrastructure.Storage
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class StorageService
    {
        public async UniTask<T> LoadFromAdressable<T>(AssetReference reference)
            where T : Object =>
            await Addressables.LoadAssetAsync<T>(reference).ToUniTask();

        public async UniTask<T> LoadFromAdressable<T>(string name)
            where T : Object =>
            await Addressables.LoadAssetAsync<T>(name).ToUniTask();
        
        public async UniTask<T> LoadFromAdressable<T>(IResourceLocation location)
            where T : Object =>
            await Addressables.LoadAssetAsync<T>(location).ToUniTask();
        
        public async UniTask<IList<IResourceLocation>> LoadResourceLocations<T>(List<string> keys)
            where T : Object =>
            await Addressables.LoadResourceLocationsAsync(keys, Addressables.MergeMode.Union, typeof(T));
    }
}