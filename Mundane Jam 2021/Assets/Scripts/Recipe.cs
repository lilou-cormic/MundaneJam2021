using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    [SerializeField] ResourceType[] Resources;

    public Dictionary<ResourceType, int> ResourceCount => Resources.GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
}