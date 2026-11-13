using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // This method checks if the player is moving upwards and hitting the block from below.
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body == null || !other.CompareTag("Player") || body.linearVelocity.y <= 0f)
        {
            return;
        }

        IHittable block = GetComponentInParent<IHittable>();

        if (block != null)
        {
            block.HitFromBelow();
        }
    }
}
