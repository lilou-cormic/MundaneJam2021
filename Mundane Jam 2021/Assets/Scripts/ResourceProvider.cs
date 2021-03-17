using UnityEngine;

public class ResourceProvider : MonoBehaviour
{
    [SerializeField] ResourceProviderDef Def = null;

    private int _PressesLeft;

    private bool _isCollecting = false;

    private void Start()
    {
        _PressesLeft = Def.ButtonPresses;
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
}