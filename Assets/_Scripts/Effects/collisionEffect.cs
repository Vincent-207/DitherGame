using UnityEngine;

public class collisionEffect : MonoBehaviour
{
    [SerializeField] CameraShake cameraShake;
    [SerializeField] float effectThreshold;
    [SerializeField] AudioClip collisionSound;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        float impulseAmount = collision.impulse.magnitude / Time.fixedDeltaTime;
        if(impulseAmount >= effectThreshold)
        {
            cameraShake.StartShake();
            audioSource.PlayOneShot(collisionSound);
        }
    }
}
