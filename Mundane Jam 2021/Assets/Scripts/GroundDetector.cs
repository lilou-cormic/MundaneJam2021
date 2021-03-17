using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool IsGrounded => _ground || _platform;

    private bool _ground = false;
    private bool _platform = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            _ground = true;

        if (collision.CompareTag("Platform"))
            _platform = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            _ground = false;

        if (collision.CompareTag("Platform"))
            _platform = false;
    }
}