using PurpleCable;
using UnityEngine;

public class Timber : MonoBehaviour, IPoolable
{
    [SerializeField] TimberDef _Def;
    public TimberDef Def => _Def;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    [SerializeField] Sprite[] Images;

    private int _Integrity;
    public int Integrity
    {
        get => _Integrity;

        set
        {
            _Integrity = value;

            if (Integrity == Def.Integrity)
                SpriteRenderer.sprite = Images[0];
            else if (Integrity > 1)
                SpriteRenderer.sprite = Images[1];
            else
                SpriteRenderer.sprite = Images[2];
        }
    }

    public bool IsDecaying { get; private set; }

    private float _timer = 0f;

    private bool _isDestroyed = false;

    private void OnEnable()
    {
        IsDecaying = false;
        _timer = 0f;
        _isDestroyed = false;

        Integrity = Def.Integrity;
    }

    private void Update()
    {
        if (_isDestroyed)
            return;

        if (IsDecaying)
        {
            if (_timer >= 1)
            {
                Integrity--;

                if (Integrity <= 0)
                {
                    _isDestroyed = true;
                    gameObject.SetActive(false);
                    return;
                }

                _timer = 0;
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
    }

    public void StartDecaying()
    {
        IsDecaying = true;
    }

    bool IPoolable.IsInUse => gameObject.activeSelf;

    void IPoolable.SetAsInUse() => gameObject.SetActive(true);

    void IPoolable.SetAsAvailable() => gameObject.SetActive(false);
}
