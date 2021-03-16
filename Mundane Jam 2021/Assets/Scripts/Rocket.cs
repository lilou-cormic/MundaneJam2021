using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider = null;

    private bool _isExploding = false;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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
}
