using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI healthText;

    private HealthSystem healthSystem;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        healthSystem = player.GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
        healthText.text = $"Health: {healthSystem.health}";
    }
}
