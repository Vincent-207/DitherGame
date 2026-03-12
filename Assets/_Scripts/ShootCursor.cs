
using UnityEngine;
using UnityEngine.UI;

public class ShootCursor : MonoBehaviour
{
    [SerializeField] LayerMask whatIsShootable;
    [SerializeField] Sprite shootable, notShootable, reloading;
    [SerializeField] Transform head;
    Image image;
    public bool isReloading;
    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        RaycastHit hit;
        bool isThereCollider = Physics.Raycast(head.position, head.forward,out hit, 1000f);
        // Debug.Log("hit: " + hit.collider.name);
        bool isShootable = isThereCollider && (whatIsShootable & (1 << hit.collider.gameObject.layer)) != 0;
        image.sprite = null;
        image.color = new Color(1f, 1f, 1f, 0f);
        if(isReloading)
        {
            image.color = new Color(1f, 1f, 1f);
            image.sprite = reloading;
            return;
        }

        if(isThereCollider) 
        {
            image.color = new Color(1f, 1f, 1f);
            image.sprite = notShootable;

        }
        if(isShootable) image.sprite = shootable;



    }
}
