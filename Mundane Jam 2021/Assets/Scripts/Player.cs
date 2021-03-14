using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private Animator Animator = null;

    [SerializeField] SpriteRenderer SpriteRenderer = null;

    private bool _isDead = false;

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

        if (Input.GetButtonDown("Fire2"))
        {
            if (Input.GetAxisRaw("Vertical") > 0.01f)
                transform.position += Vector3.up * 2.2f;
            else if (Input.GetAxisRaw("Vertical") < -0.01f)
                transform.position += Vector3.down * 2.2f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }
}