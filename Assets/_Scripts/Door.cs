using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] string levelToLoadName;
    [SerializeField] LevelTimer levelTimer;
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
        SceneManager.LoadScene(levelToLoadName);
        levelTimer.Save();
    }
    
}
