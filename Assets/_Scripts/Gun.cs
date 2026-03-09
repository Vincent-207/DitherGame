using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float cooldownDuration, shootPower;
    bool canFire = true;
    [SerializeField] InputActionReference fire;
    [SerializeField] Transform orientation;
    [SerializeField] int maxCapacity, currentCapacity;
    Rigidbody rb;
    void OnEnable()
    {
        fire.action.started += Shoot;
    }
    void OnDisable()
    {
        fire.action.started -= Shoot;
        
    }
    

    void Shoot(InputAction.CallbackContext context)
    {
        if(canFire)
        {
            rb.AddForce(-Camera.main.transform.forward * shootPower, ForceMode.Impulse);
            StartCoroutine(Cooldown(cooldownDuration));
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentCapacity = maxCapacity;
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
