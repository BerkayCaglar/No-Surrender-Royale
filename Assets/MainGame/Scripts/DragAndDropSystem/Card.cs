using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // This is the original card prefab. We need this to instantiate the original card when we drop the card.
    [SerializeField] private GameObject _originalCardPrefab;

    // This is the preview card prefab. We need this to instantiate the preview card when we drag the card.
    [SerializeField] private GameObject _previewCardPrefab;

    // This is to store the y axis offset of the card. We need this to set the position of the card when we instantiate the card.
    private Vector3 _yAxisOffset;

    private void Start()
    {
        // Set y axis offset to original card prefab's y position.
        _yAxisOffset = new Vector3(0f, _originalCardPrefab.transform.position.y, 0f);
    }
    /// <summary>
    /// Get original card prefab
    /// </summary>
    /// <returns> Original card prefab </returns>
    public GameObject GetOriginalCard()
    {
        return _originalCardPrefab;
    }

    /// <summary>
    /// Get preview card prefab
    /// </summary>
    /// <returns> Preview card prefab </returns>
    public GameObject GetPreviewCard()
    {
        return _previewCardPrefab;
    }

    /// <summary>
    /// Get y axis offset
    /// </summary>
    /// <returns> Y axis offset </returns>
    public Vector3 GetYAxisOffset()
    {
        return _yAxisOffset;
    }
}
