using PurpleCable;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Animator Animator = null;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    private bool _isDead = false;

    private ResourceProvider _resourceProvider = null;

    private bool _canPlace = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isDead || GameManager.IsGamePaused)
            return;

        var horizontal = Input.GetAxisRaw("Horizontal");

        MoveController.Move(transform, horizontal, 10);

        Animator.SetFloat("speed", Mathf.Abs(horizontal));

        //HACK
        if (Input.GetButtonDown("Fire1"))
        {
            if (_resourceProvider != null)
                _resourceProvider.Collect();
            else if (_canPlace)
                TimberPool.Current.GetTimber(Timbers.GetDefs().Where(x => ResourceInventory.Current.HasResources(x.Recipe)).ToArray().GetRandom());
        }



        //HACK

        if (Input.GetButtonDown("Jump"))
        {
            if (Input.GetAxisRaw("Vertical") > 0.01f)
                transform.position += Vector3.up * 2.2f;
            else if (Input.GetAxisRaw("Vertical") < -0.01f)
                transform.position += Vector3.down * 2.2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _resourceProvider = collision.GetComponent<ResourceProvider>();

        _canPlace = collision.CompareTag("Platform");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ResourceProvider>() != null)
            _resourceProvider = null;

        if (collision.CompareTag("Platform"))
            _canPlace = false;
    }
}