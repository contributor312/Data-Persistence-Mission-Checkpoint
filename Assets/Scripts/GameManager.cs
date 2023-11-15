using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [Serializable]
    private class BestScore
    {
        public string Name;
        public int Score;
    }

    public static GameManager Instance;
    public TMP_Text BestScoreText;
    private string PlayerName;
    private string BestPlayerName;
    private int CurrentBestScore = 0;

    private void SaveScore()
    {
        var scoreToSave = new BestScore
        {
            Name = PlayerName,
            Score = CurrentBestScore
        };

        var json = JsonUtility.ToJson(scoreToSave);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void LoadScore()
    {
        if(File.Exists(Application.persistentDataPath + "/savefile.json"))
        {
            var json = File.ReadAllText(Application.persistentDataPath + "/savefile.json");
            var scoreToLoad = JsonUtility.FromJson<BestScore>(json);

            BestPlayerName = PlayerName = scoreToLoad.Name;
            CurrentBestScore = scoreToLoad.Score;
        }

        BestScoreText.text = $"Best Score: {BestPlayerName} : {CurrentBestScore}";
    }

    private void Awake()
    {
        LoadScore();

        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName(TMP_InputField inputField)
    {
        if(inputField.text.Length > 0)
            GameManager.Instance.PlayerName = inputField.text;
    }

    public void SetCurrentBestScore(int score)
    {
        if (score > CurrentBestScore)
        {
            CurrentBestScore = score;
            BestPlayerName = PlayerName;
            SaveScore();
        }    
    }

    public int GetCurrentBestScore()
    {
        return CurrentBestScore;
    }

    public string GetBestPlayerName()
    {
        return BestPlayerName;
    }

    public void LoadMain()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #elif UNITY_STANDALONE
             Application.Quit();
        #endif
    }
}
