using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyMenuUI : MonoBehaviour
{
    public static DifficultyMenuUI Instance { get; private set; }
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
    [Header("Menu")]
    public GameObject difficultyMenu;
    [Header("Buttons")]
    public Button goBack;
    public Button easy;
    public Button normal;
    public Button master;
    [Header("Other")]
    public int difficulty;
    public bool didSelectDifficulty;

    void Start()
    {
        goBack.onClick.AddListener(() => UIManager.Instance.SwitchToStart());
        easy.onClick.AddListener(() =>
        {
            difficulty = 1;
            didSelectDifficulty = true;
            SetGameManagerValues();

            Debug.Log("Difficulty set to: " + difficulty);
        });
        normal.onClick.AddListener(() => {
            difficulty = 2;
            didSelectDifficulty = true;
            SetGameManagerValues();

            Debug.Log("Difficulty set to: " + difficulty);
        });
        master.onClick.AddListener(() => {
            difficulty = 3;
            didSelectDifficulty = true;
            SetGameManagerValues();

            Debug.Log("Difficulty set to: " + difficulty);
        });
    }
    void SetGameManagerValues()
    {
        GameManager.Instance.didSelectDifficulty = didSelectDifficulty;
        GameManager.Instance.difficulty = difficulty;
    }
}
