using UnityEngine;

public class ResourceProvider : MonoBehaviour
{
    [SerializeField] ResourceProviderDef Def = null;

    [SerializeField] GameObject Tooltip = null;

    private int _PressesLeft;

    private bool _isCollecting = false;

    private void Start()
    {
        _PressesLeft = Def.ButtonPresses;

        if (Tooltip != null)
            Tooltip.SetActive(false);
    }

    public void Collect()
    {
        if (_isCollecting)
            return;

        _isCollecting = true;

        _PressesLeft--;

        if (_PressesLeft <= 0)
        {
            if (ResourceInventory.Current.HasResources(Def.Recipe))
            {
                ResourceInventory.Current.AddResource(Def.ResourceType);
                ResourceInventory.Current.TakeResources(Def.Recipe);

                _PressesLeft = Def.ButtonPresses;
            }
        }

        _isCollecting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Tooltip != null)
            Tooltip.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Tooltip != null)
            Tooltip.SetActive(false);
    }
}