using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Animator Animator = null;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    [SerializeField] GroundDetector GroundDetector = null;

    [SerializeField] SpriteRenderer CraftedItem = null;

    [SerializeField] AudioClip JumpSound = null;

    [SerializeField] AudioClip PlaceSound = null;

    [SerializeField] GameObject Axe = null;

    [SerializeField] GameObject Pickaxe = null;

    [SerializeField] AudioClip CollectSound = null;

    private bool _isDead = false;

    private ResourceProvider _resourceProvider = null;

    private bool _canPlace = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Animator = GetComponent<Animator>();

        Axe.SetActive(false);
        Pickaxe.SetActive(false);
    }

    private void Update()
    {
        if (_isDead || GameManager.IsGamePaused)
            return;

        var horizontal = Input.GetAxisRaw("Horizontal");

        MoveController.Move(transform, horizontal, 8);

        Animator.SetFloat("speed", Mathf.Abs(horizontal));
        Animator.SetBool("jump", !GroundDetector.IsGrounded);

        CraftedItem.sprite = (_canPlace && ResourceInventory.Current.HasResources(TimberFactory.Current.SelectedTimberType.Recipe) ? TimberFactory.Current.SelectedTimberType.Image : null);

        if (GroundDetector.IsGrounded)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (_resourceProvider != null)
                {
                    CollectSound.PlayRandomPitch();
                    _resourceProvider.Collect();

                    switch (_resourceProvider.Def.ResourceType)
                    {
                        case ResourceType.Wood:
                            StartCoroutine(DoShowTool(Axe));
                            break;

                        case ResourceType.Stone:
                            StartCoroutine(DoShowTool(Pickaxe));
                            break;
                    }

                }
                else if (_canPlace)
                {
                    if (TimberPool.Current.GetTimber(TimberFactory.Current.SelectedTimberType) != null)
                        PlaceSound.Play();
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                JumpSound.Play();

                Animator.SetBool("jump", true);

                if (Input.GetAxisRaw("Vertical") < -0.01f)
                    transform.position += Vector3.down * 2.2f;
                else
                    transform.position += Vector3.up * 2.5f;
            }
        }
    }

    private IEnumerator DoShowTool(GameObject tool)
    {
        tool.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        tool.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ResourceProvider>() != null)
            _resourceProvider = collision.GetComponent<ResourceProvider>();

        if (collision.CompareTag("WorkArea"))
            _canPlace = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ResourceProvider>() != null)
            _resourceProvider = null;

        if (collision.CompareTag("WorkArea"))
            _canPlace = false;
    }
}