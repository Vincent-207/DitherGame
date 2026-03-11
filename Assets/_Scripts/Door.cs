using UnityEngine;

public class Door : MonoBehaviour
{
    public string levelToLoadName;
    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        Debug.Log("door has been entered correctly!");
    }
    
}
