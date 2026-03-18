using System;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] float BounceAmount = 3f;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided w/Player");
            Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 normal = collision.GetContact(0).normal;
            Debug.DrawRay(transform.position, normal);
            Vector3 playerVelocity = playerRB.linearVelocity;
            Vector3 reflected = Vector3.Reflect(playerVelocity.normalized, normal.normalized) * playerVelocity.magnitude;
            playerRB.linearVelocity = transform.forward * BounceAmount * playerVelocity.magnitude;
            // playerRB.AddForce( reflected * BounceAmount, ForceMode.VelocityChange);
        }
    }

    void OnTriggerEnter(Collider other)
    {
            
    }

}
