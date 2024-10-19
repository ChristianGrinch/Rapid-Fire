using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartMenuUI : MonoBehaviour
{
    public static RestartMenuUI Instance { get; private set; }
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
    public GameObject restartMenu;
    [Header("Other")]
    public Button restart;
    private void Start()
    {
        restart.onClick.AddListener(() => GameManager.Instance.RestartGame());
    }
    public void ShowRestartMenu() 
    {
        restartMenu.SetActive(true);
    }
}
