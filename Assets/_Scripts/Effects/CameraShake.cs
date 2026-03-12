using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Vector3 origin;
    bool isShaking;
    public void StartShake()
    {
        StartShake(0.15f, 0.4f);
    }
    public void StartShake(float duration, float magnitude)
    {
        
        if(!isShaking) StartCoroutine(Shake(duration, magnitude));
    }
    IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Random.insideUnitSphere * magnitude;
            yield return null;
        }

        transform.localPosition = origin;
        isShaking = false;
    }
}
