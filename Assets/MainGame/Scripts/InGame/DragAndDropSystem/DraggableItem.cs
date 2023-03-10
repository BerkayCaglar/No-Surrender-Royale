using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Photon.Bolt;

// This is to make sure that the gameobject has Card and Image component
[RequireComponent(typeof(Card)), RequireComponent(typeof(Image))]
public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    // This is the image component of the gameobject. We need this to set alpha of the image when we drag the card
    private Image _image;

    // This is the text component of the gameobject. This is temporary.
    private TMP_Text _text;

    // This is the card component of the gameobject. We need this to get the preview card and original card prefab
    private Card _card;

    // This is to check if the card is on the player side
    private bool _cardIsOnPlayerSide;

    public string _sideName;

    // This is to store the instantiated preview card and original card.
    private GameObject _instantiatedPreviewCard, _instantiatedOriginalCard;

    // This is the parent of the gameobject after we drag the card. If we drag the card to a place where it is not allowed, we will set the parent of the gameobject to this parent.
    private Transform _parentAfterDrag;

    // Getter for _parentAfterDrag property. 
    public Transform ParentAfterDrag
    {
        get => _parentAfterDrag;
    }
    private void Start()
    {
        // Get image, card and text component
        _image = GetComponent<Image>();
        _card = GetComponent<Card>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// This is called when we begin drag
    /// </summary>
    /// <param name="eventData"> Event data </param>
    public void OnBeginDrag(PointerEventData eventData)
    {

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

    /// <summary>
    /// This is called when we drag the item
    /// </summary>
    /// <param name="eventData"> Event data </param>
    public void OnDrag(PointerEventData eventData)
    {
        // Set position of item to mouse position
        transform.position = Input.mousePosition;

        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast hit
        RaycastHit hit;

        // Raycast to check if we hit player side
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("PlayerTowers", "EnemyTowers", "PlayerSide", "EnemySide")))
        {
            // If we hit player side, 
            if (hit.collider.CompareTag(_sideName))
            {
                // Set _cardIsOnPlayerSide to true
                _cardIsOnPlayerSide = true;

                // Set image transparency to 0 to show that we are on player side. Image is transparent now.
                _image.color = new Color(1, 1, 1, 0f);

                // Set text transparency to 0.
                _text.color = new Color(1, 1, 1, 0f);

                // If _instantiatedPreviewCard is null, instantiate preview card
                if (_instantiatedPreviewCard == null)
                {
                    if (_sideName == "PlayerSide")
                    {
                        // Instantiate preview card and set it to _instantiatedPreviewCard
                        _instantiatedPreviewCard = Instantiate(_card.GetPreviewCard(), Input.mousePosition, Quaternion.identity);
                    }
                    else if (_sideName == "EnemySide")
                    {
                        // Instantiate preview card and set it to _instantiatedPreviewCard
                        _instantiatedPreviewCard = Instantiate(_card.GetPreviewCard(), Input.mousePosition, Quaternion.Euler(0, -180f, 0));
                    }

                }

                // Set _instantiatedPreviewCard transform to hit point + offset. This means that the preview card will be on the ground with card offset.
                _instantiatedPreviewCard.transform.position = hit.point;

                return;
            }
        }

        // Set _cardIsOnPlayerSide to false
        _cardIsOnPlayerSide = false;

        // Set alpha to 0.5 to show that we are dragging. Image is half transparent now.
        _image.color = new Color(1, 1, 1, 0.5f);

        // Set text color to black.
        _text.color = Color.black;

        // If _instantiatedPreviewCard is not null, destroy it
        if (_instantiatedPreviewCard != null)
        {
            Destroy(_instantiatedPreviewCard);
        }
    }

    /// <summary>
    /// This is called when we end drag
    /// </summary>
    /// <param name="eventData"> Event data </param>
    public void OnEndDrag(PointerEventData eventData)
    {

        // If after drag, gameobject is not in player side, set parent to _parentAfterDrag. This is to prevent item from being lost. Item will be in its original parent.
        transform.SetParent(_parentAfterDrag);

        // Set raycast target to true
        _image.raycastTarget = true;

        // Set alpha to 1 to show that we are not dragging. Image is not transparent now.
        _image.color = new Color(1, 1, 1, 1);

        // Set text color to black.
        _text.color = Color.black;
    }

    /// <summary>
    /// This is called when we drop the item
    /// </summary>
    /// <param name="eventData"> Event data </param>
    public void OnDrop(PointerEventData eventData)
    {
        // If we drop the card to player side, instantiate original card and destroy preview card
        if (_cardIsOnPlayerSide)
        {
            if (_card.GetCardCost() <= UIManager.Instance.ElixirCount * 10)
            {
                UIManager.Instance.DecreaseElixir(_card.GetCardCost());

                Quaternion rotation = _sideName == "PlayerSide" ? Quaternion.identity : Quaternion.Euler(0, -180f, 0);

                if (_card.GetOriginalCard().CompareTag("Knights") || _card.GetOriginalCard().CompareTag("SkeletonWarriors"))
                {
                    var evnt = SpawnObjectEvent.Create();
                    evnt.PrefabID = _card.GetOriginalCard().transform.GetChild(0).GetComponent<BoltEntity>().PrefabId;
                    evnt.PrefabPosition = _instantiatedPreviewCard.transform.position + new Vector3(-0.8f, 0f, 0f);
                    evnt.PrefabRotation = rotation;
                    evnt.Send();

                    evnt = SpawnObjectEvent.Create();
                    evnt.PrefabID = _card.GetOriginalCard().transform.GetChild(1).GetComponent<BoltEntity>().PrefabId;
                    evnt.PrefabPosition = _instantiatedPreviewCard.transform.position + new Vector3(+0.8f, 0f, 0f);
                    evnt.PrefabRotation = rotation;
                    evnt.Send();
                }
                else
                {
                    Debug.Log("Spawned");
                    var evnt = SpawnObjectEvent.Create();
                    evnt.PrefabID = _card.GetOriginalCard().GetComponent<BoltEntity>().PrefabId;
                    evnt.PrefabPosition = _instantiatedPreviewCard.transform.position;
                    evnt.PrefabRotation = rotation;
                    evnt.Send();

                }

            }

            // Destroy preview card
            Destroy(_instantiatedPreviewCard);
            // The part of sending the used card to the end of the deck.

        }
    }
}
