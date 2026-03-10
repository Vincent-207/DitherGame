using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
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
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Random.insideUnitSphere * magnitude;
            yield return null;
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }
}
