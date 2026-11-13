using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BrickBlock : MonoBehaviour, IHittable
{
    public ParticleSystem breakEffect;
    public AudioClip breakSfx;
    private bool destroyed = false;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = false;
    }


    // The sound volume no longer depends on the camera position because the AudioSource is now on the AudioManager GameObject, with Spatial Blend set to 0 (2D).
    // The sound continues playing even after the BrickBlock is destroyed because the AudioSource belongs to the AudioManager, which is not destroyed with the BrickBlock.
    [ContextMenu("Hit From Below")]
    public void HitFromBelow()
    {
        if (destroyed) 
        {
            return;
        }

        destroyed = true;

        if (breakEffect != null)
        {
            var fx = Instantiate(breakEffect, transform.position, Quaternion.identity);

            // A separate variable is needed because fx.main returns a struct (a value), not a reference, so its properties cannot be modified directly.
            // The Destroy stop action automatically destroys the Particle System GameObject when its last particle disappears.
            var main = fx.main;
            main.stopAction = ParticleSystemStopAction.Destroy;

            fx.Play();
        }

        if (breakSfx != null)
        {
            AudioManager.Instance.PlaySfx(breakSfx);
        }

        Destroy(gameObject);
    }
}
