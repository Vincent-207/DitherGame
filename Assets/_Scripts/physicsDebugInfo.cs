using System;
using TMPro;
using UnityEngine;

public class physicsDebugInfo : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    [SerializeField] Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        String output = "";
        String currentSpeed = string.Format("Current speed: {0}", rb.linearVelocity.magnitude);
        output += currentSpeed;
        textBox.text = output;
    }
}
