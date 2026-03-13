using System;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class walkingSFX : MonoBehaviour
{
    [SerializeField]
    PlayerMovement playerMovement;
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.isWalking && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
    }

    
}
