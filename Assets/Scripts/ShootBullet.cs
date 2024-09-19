using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    private int speed = 37; // Speed of the bullet --IMPLIMENTED
    private int range = 50; // How far the bullet can travel from its initial location before it despawns --IMPLIMENTED
    private Vector3 spawnPos;
    private Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        Debug.Log(Vector3.Distance(currentPos, spawnPos));
        if (Vector3.Distance(currentPos, spawnPos) > range)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

    }

    void OutOfBounds()
    {
        // write code to kill bullet out of bounds
    }
}
