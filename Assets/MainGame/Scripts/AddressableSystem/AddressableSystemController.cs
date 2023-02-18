using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets.ResourceLocators;
using System.Threading.Tasks;

public class AddressableSystemController : MonoBehaviour
{
    // AssetReference is a wrapper for a string key that is used to load assets from Addressables.
    [SerializeField] private AssetReference _battleSceneAssetReference;

    private void Start()
    {
        // Clear the cache
        Caching.ClearCache();

        // Set the game state to Updating
        GameManager.Instance._gameState = GameManager.GameState.Updating;

        // Initialize download size and download the asset
        StartCoroutine(DownloadContentInformationGetterAndSetter());
    }

    /// <summary>
    /// Get the download size of the asset
    /// </summary>
    /// <returns></returns>
    public IEnumerator DownloadContentInformationGetterAndSetter()
    {
        // Get the asset key from the asset reference
        string assetKey = _battleSceneAssetReference.RuntimeKey.ToString();

        // Get the download size of the asset with the asset key. This is an asynchronous operation.
        AsyncOperationHandle<long> downloadSizeHandle = Addressables.GetDownloadSizeAsync(assetKey);

        // Wait until the download size is available
        yield return downloadSizeHandle;

        // Check the download size before the download is completed
        if (downloadSizeHandle.IsDone)
        {
            // Get the download size
            long downloadSize = downloadSizeHandle.Result;

            // Set the download size text
            UIManager.Instance.SetDownloadAmountText(((double)downloadSize / (1024 * 1024)).ToString("F1") + " MB");

            // Check if the download size is 0
            if (downloadSize <= 0)
            {
                // If the download size is 0 immediately start the download of the asset
                UIManager.Instance.PlayButton();

                // Start the download of the asset
                StartInitializeAddressables();
            }
        }
        else
        {
            // If the download size is not available, print an error message
            Debug.LogError("Download size not yet available");
        }
    }

    /// <summary>
    /// This method is called from UIManager.cs to start the download of the asset. It is called after the download size is available.
    /// </summary>
    public void StartInitializeAddressables()
    {
        // Initialize Addressables
        Addressables.InitializeAsync().Completed += (obj) =>
        {
            // Start the download of the asset
            DownloadAssetWithLabel();
        };
    }

    /// <summary>
    /// This method is called from StartInitializeAddressables() to download the asset.
    /// </summary>
    /// <returns></returns>
    public async void DownloadAssetWithLabel()
    {
        // Download the dependencies of the asset with the asset reference. This is an asynchronous operation.
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(_battleSceneAssetReference);

        // Wait until the download is completed
        while (!downloadHandle.IsDone)
        {
            Debug.Log("Download progress: " + downloadHandle.PercentComplete);
            // Set the download progress slider value and the download progress text value.
            UIManager.Instance.SetDownloadingSliderValueAndTextValue(downloadHandle.PercentComplete);

            // Wait for 1 frame
            await Task.Delay(1);
        }

        // Check the download status
        if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            // Instantiate the asset with the asset reference. This is an asynchronous operation.
            var playerArmatureGameObject = Addressables.InstantiateAsync(_battleSceneAssetReference);

            // Wait until the instantiation is completed
            await playerArmatureGameObject.Task;

            // Set the position of the instantiated asset
            playerArmatureGameObject.Result.transform.position = Vector3.zero;

            // Get the renderers of the instantiated asset
            var renderers = playerArmatureGameObject.Result.GetComponentsInChildren<Renderer>(true);

            // Replace the shaders of the renderers
            foreach (var renderer in renderers)
            {
                // Replace the shaders of the renderers
                ReplaceShaders(renderer.sharedMaterials);
            }
        }
        else
        {
            // If the download is not successful, print an error message
            Debug.LogError("Download is not successful. : " + downloadHandle.OperationException.Message);
        }
    }

    /// <summary>
    ///  Replace the shaders of the materials. Because the shaders of the materials are pink in the build. This is a temporary solution.
    /// </summary>
    /// <param name="materials"> The materials of the renderers </param>
    private void ReplaceShaders(Material[] materials)
    {
        // Loop through the materials
        foreach (var material in materials)
        {
            // Check the shader name of the material
            if (material.shader.name == "Standard")
            {
                // Replace the shader of the material
                material.shader = Shader.Find("Standard");
            }
        }
    }
}
