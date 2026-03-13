using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{

    String LastSceneParam = "LastGameplayScene";
    public void Continue()
    {
        String lastSceneName = PlayerPrefs.GetString(LastSceneParam);
        Debug.Log("Last scene name: " + lastSceneName);
        String LevelsFilePath = "Assets/Scenes/Levels/";
        String SceneFileType = ".unity";
        int lastSceneIndex = SceneUtility.GetBuildIndexByScenePath(LevelsFilePath + lastSceneName + SceneFileType);
        Debug.Log("Last scene index: " + (lastSceneIndex));
        SceneManager.LoadScene(lastSceneIndex + 1);
    }
}
