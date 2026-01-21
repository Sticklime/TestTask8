using UnityEngine;

namespace CodeBase.Infrastructure.Services.ResourcesLoader
{
    public class AssetLoader : IAssetLoader
    {
        public T Load<T>(string path) where T : Object
        {
            T asset = Resources.Load<T>(path);
        
            if (asset == null) 
                Debug.LogError($"Failed to load resource at path: {path} of type {typeof(T)}");
        
            return asset;
        }
    
        public T[] LoadAll<T>(string path) where T : Object
        {
            T[] assets = Resources.LoadAll<T>(path);
        
            if (assets == null || assets.Length == 0)
                Debug.LogError($"Failed to load any resources at path: {path} of type {typeof(T)}");
        
            return assets;
        }
    }
}