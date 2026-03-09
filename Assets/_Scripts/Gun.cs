using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float cooldownDuration, shootPower, reloadTime;
    bool canFire = true, isReloading = false;
    [SerializeField] InputActionReference fire, reload;
    [SerializeField] Transform orientation;
    [SerializeField] int maxCapacity, currentCapacity;
    [SerializeField] TMP_Text debugText;
    Rigidbody rb;
    void OnEnable()
    {
        fire.action.started += Shoot;
        reload.action.started += Reload;
    }
    void OnDisable()
    {
        fire.action.started -= Shoot;
        reload.action.started -= Reload;
        
    }
    
    void Reload(InputAction.CallbackContext context)
    {
        if(!isReloading) StartCoroutine(ReloadRoutine());
    }
    void UpdateCounter()
    {
        debugText.text = String.Format("{0} / {1}", currentCapacity, maxCapacity);
    }
    IEnumerator ReloadRoutine()
    {
        isReloading = true;
        debugText.text = "Reloading...";
        float duration = reloadTime;
        while(duration > 0)
        {
            duration -=Time.deltaTime;
            yield return null;
        }
        isReloading = false;
        currentCapacity = maxCapacity;
        UpdateCounter();
    }
    void Shoot(InputAction.CallbackContext context)
    {
        if(canFire && (currentCapacity > 0 && !isReloading) )
        {
            rb.AddForce(-Camera.main.transform.forward * shootPower, ForceMode.Impulse);
            currentCapacity--;
            UpdateCounter();
            StartCoroutine(Cooldown(cooldownDuration));
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentCapacity = maxCapacity;
        UpdateCounter();
    }

    IEnumerator Cooldown(float duration)
    {
        
        canFire = false;
        while(duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        canFire = true;
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, orientation.transform.forward * 5f, Color.red);
    }
}
