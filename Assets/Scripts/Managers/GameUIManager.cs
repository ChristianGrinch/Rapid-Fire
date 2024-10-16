using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("Menus")]
    public GameObject game;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject restartMenu;

    public TMP_Text difficultyText;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OpenMainMenu()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void ShowRestartMenu()
    {
        restartMenu.SetActive(true);
    }
}
