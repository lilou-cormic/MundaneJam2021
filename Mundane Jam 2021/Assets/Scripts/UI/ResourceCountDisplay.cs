using TMPro;
using UnityEngine;

public class ResourceCountDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CountText;

    [SerializeField] ResourceType ResourceType;

    private void Awake()
    {
        ResourceInventory.ResourceCountChanged += ResourceInventory_ResourceCountChanged;
    }

    private void OnDestroy()
    {
        ResourceInventory.ResourceCountChanged -= ResourceInventory_ResourceCountChanged;
    }

    private void ResourceInventory_ResourceCountChanged(ResourceType resourceType)
    {
        if (resourceType != ResourceType)
            return;

        CountText.text = ResourceInventory.Current.GetCount(resourceType).ToString("00");
    }
}
