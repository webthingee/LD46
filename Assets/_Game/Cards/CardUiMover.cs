using UnityEngine;
using UnityEngine.EventSystems;

public class CardUiMover : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 origPos;
    public CardLayout cardLayout;
    public bool isFrozen;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log($"Pointer Down + {gameObject.name}");
        //origPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log($"OnBeginDrag + {gameObject.name}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isFrozen) transform.position = eventData.pointerCurrentRaycast.worldPosition;
        //Debug.Log($"OnDrag + {gameObject.name}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log($"OnEndDrag + {gameObject.name}");
        if (cardLayout)
        {
            Card card = GetComponentInParent<Card>();
            cardLayout.MakeChild(card);
        }
        
        transform.position = transform.parent.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        
        //Debug.Log($"Trigger + {other.gameObject.name} and {gameObject.name}");
        CardLayout cl = other.GetComponent<CardLayout>();
        if (cl != null) cardLayout = cl;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) return;
        
        if (cardLayout) cardLayout = null;
    }
}
