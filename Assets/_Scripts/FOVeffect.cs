using System;
using TMPro;
using UnityEngine;

public class FOVeffect : MonoBehaviour
{
    [SerializeField] float baseFOV, maxFOV;
    [SerializeField] Rigidbody playerRB;
    [SerializeField] TMP_Text debugText;

    void Update()
    {
        float currentSpeed = playerRB.linearVelocity.magnitude;
        Camera.main.fieldOfView = speedToFOV(currentSpeed);
        debugText.text = String.Format("current FOV: {0}", speedToFOV(currentSpeed));
    }

    float speedToFOV(float speed)
    {
        float output = baseFOV + Mathf.Sqrt(speed);
        output = Mathf.Clamp(output, baseFOV, maxFOV);
        return output;
    }
}
