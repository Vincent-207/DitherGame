using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{

    float elapsedTime = 0f, bestTime;
    [SerializeField] TMP_Text bestTimeBox, currentTimeBox;
    String SceneName, recordModifier = "RecordTime", NewRecordModifier = "NewRecord", elpasedTimeModifer = "ElapsedTime";
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        String bestTimeParam =  SceneName + recordModifier;
        bestTime = PlayerPrefs.GetFloat(bestTimeParam, -1f);
        bestTimeBox.text = String.Format("Best time: {0}", bestTime.ToString("F2"));
        if(bestTime == -1) bestTimeBox.text = "No record";
        elapsedTime = 0f;
        PlayerPrefs.SetString("LastGameplayScene", SceneName);
        PlayerPrefs.Save();
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        currentTimeBox.text = String.Format("Elapsed Time : {00:000}", elapsedTime.ToString("F2"));
    }

    public void Save()
    {
        // Debug.Log("Starting save");
        if(elapsedTime < bestTime || bestTime == -1)
        {
            // Debug.Log("New record!");
            // Debug.Log("Setting bool w name: " + SceneName + NewRecordModifier);
            PlayerPrefs.SetInt(SceneName + NewRecordModifier, 1);
            PlayerPrefs.SetFloat(SceneName + recordModifier, elapsedTime);
            PlayerPrefs.Save();
        }
        // Debug.Log("Continuing");
        PlayerPrefs.SetFloat(SceneName + elpasedTimeModifer, elapsedTime);
        PlayerPrefs.Save();

    }
}
