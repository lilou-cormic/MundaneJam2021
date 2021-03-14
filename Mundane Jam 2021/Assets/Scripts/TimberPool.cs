using PurpleCable;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TimberPool : Pool<Timber, TimberDef>
{
    private float _countDown = 5f;

    public static TimberPool Current { get; private set; }

    private Dictionary<TimberDef, List<Timber>> lists;

    protected override void Awake()
    {
        Current = this;

        base.Awake();
    }

    private void Update()
    {
        if (_countDown <= 0)
        {
            lists.SelectMany(x => x.Value).Where(x => x.gameObject.activeSelf).ToArray().GetRandom()?.StartDecaying();

            _countDown = 5f;
            return;
        }

        _countDown -= Time.deltaTime;
    }

    protected override Timber CreateItem(TimberDef category)
    {
        Timber timber = Instantiate(Timbers.GetPrefeb(category), transform);
        timber.gameObject.SetActive(false);

        return timber;
    }

    protected override Dictionary<TimberDef, List<Timber>> GetInitialLists()
    {
        lists = new Dictionary<TimberDef, List<Timber>>();

        foreach (TimberDef timberDef in Timbers.GetDefs())
        {
            lists.Add(timberDef, new List<Timber>());
        }

        foreach (Timber timber in gameObject.GetComponentsInChildren<Timber>())
        {
            lists[timber.Def].Add(timber);
        }

        return lists;
    }

    public Timber GetTimber(TimberDef timberDef)
    {
        ResourceInventory.Current.TakeResources(timberDef);

        Timber timber = GetItem(timberDef);
        timber.transform.position = GameManager.Player.transform.position + Vector3.up;
        timber.transform.position = new Vector3(timber.transform.position.x, timber.transform.position.y);
        timber.transform.rotation = Quaternion.identity;

        return timber;
    }
}
