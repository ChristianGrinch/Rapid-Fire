using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPanelUI : MonoBehaviour
{
    public static VideoPanelUI Instance { get; private set; }
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
    [Header("Panel")]
    public GameObject videoPanel;
    void Start()
    {
        
    }
}
