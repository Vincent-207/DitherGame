using UnityEngine;

public class BossWeakspot : MonoBehaviour
{
    [SerializeField] boss boss;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<Bullet>())
        {
            boss.Stun();
        }
    }
}
