using PurpleCable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Timbers : Singleton<Timbers>
{
    [SerializeField] Timber[] Prefabs = null;

    private NamedResourceDictionary<TimberDef> Defs;

    protected override void Awake()
    {
        base.Awake();

        Defs = new NamedResourceDictionary<TimberDef>("Timbers");
    }

    public static TimberDef GetDef(string timberType)
        => Instance.Defs.Get(timberType);

    public static IEnumerable<TimberDef> GetDefs()
        => Instance.Defs.AsEnumerable();

    public static Timber GetPrefeb(TimberDef timberDef)
        => Instance.Prefabs.FirstOrDefault(x => x.Def == timberDef);
}