using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed, explosionForce, explosionRadius, upForce, explosionTime, explosionEffectEndScale, pfxOffset;
    public Transform explosionEffect;
    public LayerMask whatIsGround;
    public ParticleOneShot effect;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {
        if((whatIsGround & (1 << collision.collider.gameObject.layer)) != 0)
        {
            Explode(collision);
        }
    }

    void Explode(Collision collision)
    {
        StartCoroutine(ExplosionEffect(explosionTime));
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders)
        {
            Rigidbody colliderRB = collider.GetComponent<Rigidbody>();
            if(colliderRB)
            {
                colliderRB.AddExplosionForce(explosionForce, transform.position, explosionRadius, upForce);
                Vector3 pfxPos = transform.position + collision.GetContact(0).normal.normalized * pfxOffset;
                effect.transform.parent = transform.parent;
                effect.PlayAtPosition(pfxPos);
            }
        }


    }

    IEnumerator ExplosionEffect(float duration)
    {
        Destroy(rb);
        float elapsedTime = 0;
        float startScale = explosionEffect.transform.localScale.x;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newScale = startScale + (explosionEffectEndScale * (elapsedTime / duration));
            Vector3 newScaleVector = new Vector3(newScale, newScale, newScale);
            explosionEffect.localScale = newScaleVector;
            yield return null;
        }
        Destroy(gameObject);
    }

}
