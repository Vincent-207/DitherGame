using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    String LastSceneParam = "LastGameplayScene";
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        
    }

    public void Restart()
    {
        String lastSceneName = PlayerPrefs.GetString(LastSceneParam);
        SceneManager.LoadScene(lastSceneName);
    }

    
}
