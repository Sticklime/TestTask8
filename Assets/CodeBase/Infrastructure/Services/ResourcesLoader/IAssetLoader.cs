using UnityEngine;

namespace CodeBase.Infrastructure.Services.ResourcesLoader
{
    public interface IAssetLoader
    {
        T Load<T>(string path) where T : Object;
        T[] LoadAll<T>(string path) where T : Object;
    }
}