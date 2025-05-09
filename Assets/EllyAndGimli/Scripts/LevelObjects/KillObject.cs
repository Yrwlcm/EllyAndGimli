using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Elly") || other.CompareTag("Gimli"))
        {
            var player = other.GetComponent<PlayerControllerBase>();
            if (player != null) player.Die();
        }
    }
}
