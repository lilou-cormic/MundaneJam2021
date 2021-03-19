using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider = null;

    private Rigidbody2D rb = null;

    private SpriteRenderer SpriteRenderer = null;

    [SerializeField] GameObject Fire = null;

    private bool _isExploding = false;

    private bool _isTakingOff = false;

    private float t = 0f;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isExploding)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (var contact in collision.contacts)
            {
                ExplosionManager.SpawnExplosion(contact.point);
            }

            StartCoroutine(DoExplode());
        }
    }

    private IEnumerator DoExplode()
    {
        _isExploding = true;

        int count = 0;
        int counter = 0;

        yield return new WaitForSeconds(0.5f);

        while (count < 15)
        {
            counter++;

            if (counter > 1000)
                break;

            var pt = transform.position + (Random.insideUnitSphere * 6);

            if (capsuleCollider.OverlapPoint(pt))
            {
                count++;
                ExplosionManager.SpawnExplosion(pt);
                yield return new WaitForSeconds(Random.Range(0.2f, 0.4f - (0.02f * count)));
            }
        }

        GameManager.GameOver();
    }

    public void TakeOff()
    {
        StartCoroutine(DoTakeOff());
    }

    private IEnumerator DoTakeOff()
    {
        Fire.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        _isTakingOff = true;
    }

    private void FixedUpdate()
    {

        if (_isTakingOff)
            rb.AddForce(transform.up * 40f);

        if (_isExploding)
        {
            t += Time.deltaTime;

            SpriteRenderer.color = Color.Lerp(Color.white, Color.gray, t / 3);
        }
        else
        {
            if (Mathf.Abs(rb.rotation) > 15)
                StartCoroutine(DoExplode());
        }
    }
}
