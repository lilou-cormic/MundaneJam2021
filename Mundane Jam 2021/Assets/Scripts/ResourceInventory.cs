using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public static ResourceInventory Current { get; private set; }

    private Dictionary<ResourceType, int> resources;

    public static event Action<ResourceType> ResourceCountChanged;

    private void Awake()
    {
        Current = this;

        resources = new Dictionary<ResourceType, int>();

        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            resources.Add(resource, 0);
        }
    }

    public bool HasResources(Recipe recipe)
    {
        foreach (var item in recipe.ResourceCount)
        {
            if (resources[item.Key] < item.Value)
                return false;
        }

        return true;
    }

    public void TakeResources(Recipe recipe)
    {
        foreach (var item in recipe.ResourceCount)
        {
            resources[item.Key] -= item.Value;

            ResourceCountChanged?.Invoke(item.Key);
        }
    }

    public void AddResource(ResourceType resource)
    {
        resources[resource]++;

        ResourceCountChanged?.Invoke(resource);
    }

    public int GetCount(ResourceType resourceType)
    {
        return resources[resourceType];
    }
}