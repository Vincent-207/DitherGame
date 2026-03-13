using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public float Duration;
    public UnityEvent OnInteract; 
    public void DoInteract()
    {
        Debug.Log("Doing interact!");
        OnInteract.Invoke();
    }
}
