using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{

    float elapsedTime = 0f, bestTime;
    [SerializeField] TMP_Text bestTimeBox, currentTimeBox;
    String SceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnLevelUnloaded;
    }
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        String bestTimeParam =  SceneName + "RecordTime";
        bestTime = PlayerPrefs.GetFloat(bestTimeParam, -1f);
        bestTimeBox.text = String.Format("Best time: {0}", bestTime.ToString("F2"));
        if(bestTime == -1) bestTimeBox.text = "No record";
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        currentTimeBox.text = String.Format("Elapsed Time : {00:000}", elapsedTime.ToString("F2"));
    }

    void OnLevelUnloaded(Scene scene)
    {
        if(elapsedTime < bestTime || bestTime == -1)
        {
            PlayerPrefs.SetInt(SceneName + "NewRecord", 1);
            PlayerPrefs.SetFloat(SceneName + "RecordTime", elapsedTime);
            PlayerPrefs.Save();

        }
    }
}
