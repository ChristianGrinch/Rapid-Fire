using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    private int speed = 37; // IF I ADD MORE GUNS WITH STATS, LOWER THIS SPEED CUZ BALANCING OR SMTH IDK

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OutOfBounds()
    {
        // write code to kill bullet out of bounds
    }
}
