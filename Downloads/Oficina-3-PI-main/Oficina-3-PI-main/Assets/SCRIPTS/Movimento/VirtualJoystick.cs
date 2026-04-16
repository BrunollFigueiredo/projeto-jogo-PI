using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image baseImage;
    private Image handleImage;
    private Vector2 inputVector;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    void Start()
    {
        baseImage = GetComponent<Image>();
        handleImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / (baseImage.rectTransform.sizeDelta.x / 2));
            pos.y = (pos.y / (baseImage.rectTransform.sizeDelta.y / 2));

            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handleImage.rectTransform.anchoredPosition = new Vector2(
                inputVector.x * (baseImage.rectTransform.sizeDelta.x / 3),
                inputVector.y * (baseImage.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handleImage.rectTransform.anchoredPosition = Vector2.zero;
    }
}