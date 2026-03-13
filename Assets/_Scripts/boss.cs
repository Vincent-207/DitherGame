using System;
using System.Collections;
using UnityEngine;

public class boss : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform player;
    [SerializeField] float lookSpeed;
    bool isStunned;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStunned)
        {
            LookAtPlayer();
            UpdateLaser();
        }
            
    }
    public void Stun()
    {
        Debug.Log("Stun!");
        StartCoroutine(DoStun(3f));
    }

    IEnumerator DoStun(float duration)
    {
        float elapsedTime = 0f;
        isStunned = true;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isStunned = false;
    }
    void UpdateLaser()
    {
        lineRenderer.SetPosition(0, transform.position + transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else lineRenderer.SetPosition(1, transform.forward * 100f);
    }
    void LookAtPlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        Quaternion lookAtRot = Quaternion.LookRotation(toPlayer, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRot, lookSpeed * Time.deltaTime);
    }
}
