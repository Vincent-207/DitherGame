using UnityEngine;

public class collisionEffect : MonoBehaviour
{
    public CameraShake cameraShake;
    public float effectThreshold;
    void OnCollisionEnter(Collision collision)
    {
        float impulseAmount = collision.impulse.magnitude / Time.fixedDeltaTime;
        if(impulseAmount >= effectThreshold)
        {
            cameraShake.StartShake();
        }
    }
}
