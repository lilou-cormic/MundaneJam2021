using UnityEngine;

[CreateAssetMenu(fileName = "Timber", menuName = "Timber")]
public class TimberDef : ScriptableObject
{
    public int WeightLimit;

    public int Integrity;

    public Recipe Recipe;
}