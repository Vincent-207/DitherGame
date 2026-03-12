using UnityEngine;

public class ParticleOneShot : MonoBehaviour
{
    
    ParticleSystem ps;
    AudioSource audioSource;
    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAtPosition(Vector3 worldPos)
    {
        transform.position = worldPos;
        ps.Stop();
        ps.Play();
        audioSource.Stop();
        audioSource.Play();
    }
}
