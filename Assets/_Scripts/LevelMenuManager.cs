using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuManager : MonoBehaviour
{
    string LastSceneParam = "LastGameplayScene", recordModifier = "RecordTime", NewRecordModifier = "NewRecord", elapsedTimeModifer = "ElapsedTime";
    [SerializeField] TMP_Text newRecordBox, recordBox, elapsedTimeBox;
    String lastSceneName;
    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnLevelUnloaded;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    void Start()
    {
        lastSceneName = PlayerPrefs.GetString(LastSceneParam);
        bool newRecord = PlayerPrefs.GetInt(lastSceneName + NewRecordModifier, 0) == 1;
        float recordTime = PlayerPrefs.GetFloat(lastSceneName + recordModifier);
        float elapsedTime = PlayerPrefs.GetFloat(lastSceneName + elapsedTimeModifer);

        if(newRecord) newRecordBox.enabled = true;
        // if(newRecord) Debug.Log("loaded new record!");
        recordBox.text = "Record time: " + recordTime.ToString("F2");
        elapsedTimeBox.text = "Latest time: " + elapsedTime.ToString("F2");


    }  

    void OnLevelUnloaded(Scene scene)
    { 
        PlayerPrefs.SetInt(lastSceneName + NewRecordModifier, 0);
        PlayerPrefs.Save();
        
    }
}
