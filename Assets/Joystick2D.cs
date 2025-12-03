using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick2D : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform handle;
    public float handleLimit = 80f; // радиус хода ручки в пиксел€х

    private Vector2 _input;

    public Vector2 Direction => _input; // сюда будут обращатьс€ другие скрипты

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform bg = (RectTransform)transform;

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                bg,
                eventData.position,
                eventData.pressEventCamera,
                out pos))
        {
            // нормализованный вектор
            pos = Vector2.ClampMagnitude(pos, handleLimit);
            handle.anchoredPosition = pos;
            _input = pos / handleLimit;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
