using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimberFactory : MonoBehaviour
{
    private EventSystem EventSystem = null;

    public static TimberFactory Current { get; private set; }

    public TimberDef SelectedTimberType { get; set; }

    [SerializeField] TimberTypeSelect[] TimberTypeSelects = null;

    private void Awake()
    {
        Current = this;

        EventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            int next = (TimberTypeSelects.ToList().FindIndex(x => x.TimberType == SelectedTimberType) + 1) % TimberTypeSelects.Length;

            EventSystem.SetSelectedGameObject(TimberTypeSelects[next].gameObject);
        }
    }
}
