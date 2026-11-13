using UnityEngine.Audio;
using System.Collections;
using UnityEngine;

public class CuriousBlock : MonoBehaviour, IHittable
{
    public Contained contained = Contained.Coin;
    public GameObject coinPrefab;
    public GameObject powerUpPrefab;
    public ParticleSystem popEffect;
    public float coinLaunchForce = 6f;
    public float powerUpRiseEffect = 1f;
    public float popAnimationTime = 0.25f;
    public Material usedMaterial;
    public AudioClip popSfx;
    public AudioMixerGroup sfxGroup;

    private AudioSource audioSource;
    private bool used = false;
    
    public enum Contained
    {
        None,
        Coin,
        PowerUp
    }

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxGroup;
    }

    private void SpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.Euler(90f, 0f, 0f));
        coin.GetComponent<Rigidbody>().AddForce(Vector3.up * coinLaunchForce, ForceMode.Impulse);
        Destroy(coin, 0.5f);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.AddScore(1);
        }
    }

    private IEnumerator Pop()
    {
        Vector3 start = transform.position;
        Vector3 top = start + Vector3.up * 0.3f;
        float t = 0f;
        while (t < popAnimationTime)
        {
            t += Time.deltaTime;
            float k = t / popAnimationTime;
            // Mathf.Sin() makes the block rise and then fall because the sine value goes from 0 to 1 and back to 0 as k goes from 0 to 1.
            transform.position = Vector3.Lerp(start, top, Mathf.Sin(k * Mathf.PI));
            // Pauses the coroutine until the next frame, allowing the animation to update smoothly over time.
            yield return null;
        }
        transform.position = start;
    }

    private IEnumerator RisePowerUp()
    {
        GameObject powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * powerUpRiseEffect;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            powerUp.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        } 
    } 
    
    [ContextMenu("Hit From Below")]
    public void HitFromBelow()
    {
        if (used) {return;}
        used = true;

        GetComponent<Renderer>().material = usedMaterial;
        
        if (popEffect != null)
        {
            var fx = Instantiate(popEffect, transform.position + Vector3.up * 1f, Quaternion.identity);
            fx.Play();
            Destroy(fx, 2f);
        }

        if (popSfx != null)
        {
            audioSource.PlayOneShot(popSfx);
        }

        switch (contained)
        {
            case Contained.Coin:
                SpawnCoin();
                break;

            case Contained.PowerUp:
                StartCoroutine(RisePowerUp());
                break;

            case Contained.None:
                break;
        }

        StartCoroutine(Pop());
    }
}
