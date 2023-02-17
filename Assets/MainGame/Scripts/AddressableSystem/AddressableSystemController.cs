using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
public class AddressableSystemController : MonoBehaviour
{
    [SerializeField] private AssetReference _battleSceneAssetReference;

    // Start is called before the first frame update
    void Start()
    {
        string assetKey = _battleSceneAssetReference.RuntimeKey.ToString();
        Addressables.InitializeAsync().Completed += AddressablesManager_Complated;

        AsyncOperationHandle<long> downloadSizeHandle = Addressables.GetDownloadSizeAsync(assetKey);

        if (downloadSizeHandle.IsDone)
        {
            long downloadSize = downloadSizeHandle.Result;
            Debug.Log($"Download size of {assetKey}: {downloadSize} bytes");
        }
        else
        {
            Debug.Log("Download size not yet available");
        }
    }

    private void AddressablesManager_Complated(AsyncOperationHandle<IResourceLocator> obj)
    {
        _battleSceneAssetReference.InstantiateAsync().Completed += (go) =>
        {
            go.Result.transform.position = Vector3.zero;

            var renderers = go.Result.GetComponentsInChildren<Renderer>(true);

            foreach (var renderer in renderers)
            {
                ReplaceShaders(renderer.sharedMaterials);
            }
        };
    }
    private void ReplaceShaders(Material[] materials)
    {
        foreach (var material in materials)
        {
            if (material.shader.name == "Standard")
            {
                material.shader = Shader.Find("Standard");
            }
        }
    }
}
