using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Timber", menuName = "Timber")]
public class TimberDef : ScriptableObject
{
    public int WeightLimit;

    public int Integrity;

    public ResourceType[] Recipe;

    public Dictionary<ResourceType, int> ResourceCount => Recipe.GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
}