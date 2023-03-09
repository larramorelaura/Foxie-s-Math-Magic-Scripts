using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject hundredsBox;
    public GameObject tensBox;
    public GameObject onesBox;

    private GameObject copy;
    private List<GameObject> clones = new List<GameObject>();

    private RectTransform copyRectTransform;
    [SerializeField] private Canvas canvas;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // create a copy of the original image
        copy = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent); // create a new parent object to hold the copy
        copy.transform.SetSiblingIndex(transform.GetSiblingIndex()); // set the copy's index in the hierarchy to match the original image
        copy.GetComponent<Image>().raycastTarget = false; // disable raycast for the copy so that it doesn't block raycasts for the original image
        copyRectTransform = copy.GetComponent<RectTransform>();
        copyRectTransform.anchoredPosition = Vector2.zero;
    }


    public void OnDrag(PointerEventData eventData)
{
    
    Vector3 worldPos = Vector3.zero;
    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(copyRectTransform, eventData.position, eventData.pressEventCamera, out worldPos))
    {
        // convert the world position back to a local anchored position
        Vector2 anchoredPos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(copyRectTransform.parent as RectTransform, worldPos, eventData.pressEventCamera, out anchoredPos))
        {
            // set the new anchored position
            copyRectTransform.anchoredPosition = anchoredPos;
        }
    }
}

    public void OnEndDrag(PointerEventData eventData)
    {
        // determine which panel the copy was dropped on
        GameObject dropPanel = eventData.pointerCurrentRaycast.gameObject;

        // create a copy of the original image and set its parent to the panel that it was dropped on
        GameObject newCopy = Instantiate(gameObject, copyRectTransform.anchoredPosition, Quaternion.identity);
        clones.Add(newCopy);
        newCopy.transform.SetParent(dropPanel.transform);
        //scale the copy for production build
        // newCopy.transform.localScale = new Vector3(1f, 1f, 1f);

        // set the anchored position of the new copy to the center of its parent
        // newCopy.GetComponent<CanvasGroup>().blocksRaycasts = false;
        // newCopy.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        GridLayoutGroup gridLayout = dropPanel.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            // calculate the position of the new copy based on the grid layout cell size and padding
            float cellSize = gridLayout.cellSize.x;
            float padding = gridLayout.padding.left;
            int col = Mathf.RoundToInt(copyRectTransform.anchoredPosition.x / (cellSize + padding));
            int row = Mathf.RoundToInt(copyRectTransform.anchoredPosition.y / (cellSize + padding));
            newCopy.GetComponent<RectTransform>().anchoredPosition = new Vector2(col * (cellSize + padding), row * (cellSize + padding));
        }
        else
        {
            newCopy.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        // destroy the copy that was being dragged
        Destroy(copy);
    }

    public void ClosePopup()
    {
        foreach (GameObject clone in clones)
        {
            Debug.Log("destroying");
            Destroy(clone);
        }
    }
}
