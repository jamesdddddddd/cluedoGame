using UnityEngine;
using UnityEngine.EventSystems; //This allows us to use Unity's event system to detect our mouse inputs

public class DragUIObject : MonoBehaviour, IDragHandler, IPointerDownHandler //These classes hold the methods required to handle UI interactions that we need
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalAnchoredPosition;
    private Vector3 originalPanelLocalPosition;
    public float movementSensitivity = 1.0f; // Adjustable sensitivity if needed

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); //Get the RectTransform component of the attached GameObject
        canvas = GetComponentInParent<Canvas>(); //Get the Canvas component of the attached GameObject
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out originalLocalPointerPosition
        );

        originalAnchoredPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition))
        {
            Vector2 offset = (localPointerPosition - originalLocalPointerPosition) * movementSensitivity;
            rectTransform.anchoredPosition = originalAnchoredPosition + offset;
        }
    }
}
