using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustEffect;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip landSfx;
    [SerializeField] private float minImpactSpeed = 3f;

    public void PlayJump()
    {
        AudioManager.Instance.PlaySfx(jumpSfx);
        dustEffect.Play();
    }

    // Checks if the impact is strong enough and if the collision occurs on the lower half of the player, so the effects only play when landing on a surface.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > minImpactSpeed && 
            collision.GetContact(0).point.y < transform.position.y)
        {
            dustEffect.Play();
            AudioManager.Instance.PlaySfx(landSfx);
        }
    }
}
