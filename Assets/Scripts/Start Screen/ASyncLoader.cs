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
    public void LoadLevelBtn(string levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
        OnSceneLoaded();
    }
    void OnSceneLoaded()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log(player != null ? "Player found!" : "Player not found!");
        GameManager.Instance.GetReferences();
        SaveManager.Instance.GetReferences();
        PlayerController.Instance.GetReferences();
        GameUI.Instance.GetReferences();
        SaveManager.Instance.LoadPlayer(SaveManager.Instance.defaultSave);
        GameManager.Instance.StartGame();
    }
}
