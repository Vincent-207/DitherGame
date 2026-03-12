using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float cooldownDuration, shootPower, reloadTime;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] InputActionReference fire, reload;
    [SerializeField] Transform head;
    [SerializeField] int maxCapacity, currentCapacity;
    [SerializeField] TMP_Text debugText;
    public LayerMask whatIsground;
    bool canFire = true, isReloading = false;
    public CameraShake cameraShake;
    public ParticleSystem muzzlePoof;
    AudioSource audioSource;
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
        Reload();
    }
    void Reload()
    {
        if(!isReloading && maxCapacity != currentCapacity) StartCoroutine(ReloadRoutine());
        
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
            RaycastHit raycastHit;
            if(Physics.Raycast(head.position, head.forward, out raycastHit, 1000f, whatIsground))
            {
                // rb.AddForce(-Camera.main.transform.forward * shootPower, ForceMode.Impulse);
                currentCapacity--;
                // cameraShake.StartShake();
                GameObject bullet = Instantiate(bulletPrefab, head.transform.position + head.transform.forward, Quaternion.LookRotation(head.transform.forward));
                bullet.transform.position = raycastHit.point;
                muzzlePoof.Play();
                audioSource.Play();
                UpdateCounter();

                StartCoroutine(Cooldown(cooldownDuration));
            }

            return;
            
        }
        else if(currentCapacity <= 0) Reload();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentCapacity = maxCapacity;
        audioSource = GetComponent<AudioSource>();
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
        // Debug.DrawRay(Camera.main.transform.position, head.transform.forward * 5f, Color.red);
    }
}
