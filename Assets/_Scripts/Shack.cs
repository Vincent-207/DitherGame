using System;
using System.Collections;
using UnityEngine;

public class Shack : MonoBehaviour
{
    Transform player;
    [SerializeField] Animator animator;
    [SerializeField] float thresholdDistance = 5f;
    [SerializeField] AudioClip openSound, closeSound;
    [SerializeField] float openSoundDelay, closeSoundDelay;
    AudioSource audioSource;
    String isPlayerCloseParam = "isPlayerClose";
    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>().transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool previous = animator.GetBool(isPlayerCloseParam);
        bool current = isPlayerClose();
        animator.SetBool(isPlayerCloseParam, current);
        if(previous != current) StateChanged(current);
    }

    void StateChanged(bool isPlayerNowClose)
    {
        if(isPlayerNowClose)
        {
            audioSource.clip = openSound;
            audioSource.PlayDelayed(openSoundDelay);
        }
        else
        {
            audioSource.clip = closeSound;
            audioSource.PlayDelayed(closeSoundDelay);
        }
    }

    bool isPlayerClose()
    {
        float sqrDistanceToPlayer = (player.position - transform.position).sqrMagnitude;
        // Debug.Log("Distance to player : " + sqrDistanceToPlayer);
        return sqrDistanceToPlayer < (thresholdDistance * thresholdDistance);

    }

    
}
