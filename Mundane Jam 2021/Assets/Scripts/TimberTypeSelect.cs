using UnityEngine;
using UnityEngine.EventSystems;

public class TimberTypeSelect : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] internal TimberDef TimberType = null;

    public void OnSelect(BaseEventData eventData)
    {
        TimberFactory.Current.SelectedTimberType = TimberType;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(gameObject);
    }
}