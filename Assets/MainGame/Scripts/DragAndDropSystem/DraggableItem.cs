using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private Image _image;
    private bool _isDragging;
    private Transform _parentAfterDrag;
    public Transform ParentAfterDrag
    {
        get => _parentAfterDrag;
    }
    private void Start()
    {
        // Get image component
        _image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        // Set dragging to true
        _isDragging = true;

        // Set _parentAfterDrag to current parent
        _parentAfterDrag = transform.parent;

        // Set parent to root
        transform.SetParent(transform.root);

        // Set as last sibling
        transform.SetAsLastSibling();

        // Set raycast target to false
        _image.raycastTarget = false;

        // Set alpha to 0.5 to show that we are dragging. Image is half transparent now.
        _image.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        // Set position of item to mouse position
        transform.position = Input.mousePosition;

        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast hit
        RaycastHit hit;

        // Raycast to check if we hit player side
        if (Physics.Raycast(ray, out hit))
        {
            // If we hit player site, ------
            if (hit.collider.CompareTag("PlayerSideGround"))
            {

            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        // Set dragging to false
        _isDragging = false;

        // If after drag, gameobject is not in player side, set parent to _parentAfterDrag. This is to prevent item from being lost. Item will be in its original parent.
        transform.SetParent(_parentAfterDrag);

        // Set raycast target to true
        _image.raycastTarget = true;

        // Set alpha to 1 to show that we are not dragging. Image is not transparent now.
        _image.color = new Color(1, 1, 1, 1);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
    }
}
