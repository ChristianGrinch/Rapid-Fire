using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    public static ASyncLoader Instance { get; private set; }

    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    public bool loadedScene = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadLevelBtn(string levelToLoad, int loadType, string saveName)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelToLoad, loadType, saveName));
    }

    IEnumerator LoadLevelASync(string levelToLoad, int loadType, string saveName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
        OnSceneLoaded(loadType, saveName);
    }
    void OnSceneLoaded(int loadType, string saveName)
    {
        switch (loadType) 
        {
            case 1: // Default
                Debug.Log("Load type: " + loadType + " (Default)");
                GameManager.Instance.GetReferences();
                SaveManager.Instance.GetReferences();
                PlayerController.Instance.GetReferences();
                GameUI.Instance.GetReferences();

                GameManager.Instance.StartGame();
                SaveManager.Instance.LoadPlayer(SaveManager.Instance.defaultSave);
                loadedScene = true;
                GameManager.Instance.SetDifficulty();
                GameUI.Instance.SetDifficultyText();
                break;

            case 2: // Existing save
                Debug.Log("Load type: " + loadType + " (Existing)");
                GameManager.Instance.GetReferences();
                SaveManager.Instance.GetReferences();
                PlayerController.Instance.GetReferences();
                GameUI.Instance.GetReferences();

                GameManager.Instance.StartGame();
                SaveManager.Instance.LoadPlayer(saveName);
                loadedScene = true;
                GameManager.Instance.SetDifficulty();
                GameUI.Instance.SetDifficultyText();
                break;

            case 3: // New save
                Debug.Log("Load type: " + loadType + " (New)");
                GameManager.Instance.GetReferences();
                SaveManager.Instance.GetReferences();
                PlayerController.Instance.GetReferences();
                GameUI.Instance.GetReferences();

                GameManager.Instance.StartGame();
                SaveManager.Instance.LoadPlayer(saveName);
                loadedScene = true;
                GameManager.Instance.SetDifficulty();
                GameUI.Instance.SetDifficultyText();
                break;
            default:
                Debug.LogError("No valid load type specified!");
                break;
        }

    }
}
