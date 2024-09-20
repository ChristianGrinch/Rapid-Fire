using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("current enemy health: " + health + " HEALTHSYSTEM");
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
