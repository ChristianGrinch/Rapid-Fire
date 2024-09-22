using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{

    private int currentWave = 0;
    private float waveTime = 60;
    private float nextWaveTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextWaveTime)
        {
            nextWaveTime = Time.time + waveTime;
            currentWave++;
            Debug.Log("Current wave: " + currentWave);
        }
    }
}
