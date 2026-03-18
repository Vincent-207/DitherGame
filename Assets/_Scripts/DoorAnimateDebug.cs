using System;
using UnityEngine;

public class DoorAnimateDebug : MonoBehaviour
{
    Animation animation;
    public String animName;
    void Start()
    {
        animation = GetComponent<Animation>();
        animation.Play(animName);
    }
}
