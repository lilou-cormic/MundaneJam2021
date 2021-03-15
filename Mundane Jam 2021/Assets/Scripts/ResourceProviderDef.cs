using UnityEngine;

[CreateAssetMenu(fileName = "ResourceProvider", menuName = "ResourceProvider")]
public class ResourceProviderDef : ScriptableObject
{
    public ResourceType ResourceType;

    public int ButtonPresses;

    public Recipe Recipe;
}