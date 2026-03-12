
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
        bool isThereCollider = Physics.Raycast(head.position, head.forward, 1000f);
        bool isThereShootAble = Physics.Raycast(head.position, head.forward, 1000f, whatIsShootable);
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
        if(isThereShootAble) image.sprite = shootable;



    }
}
