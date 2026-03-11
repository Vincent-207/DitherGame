using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndBoundary : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
