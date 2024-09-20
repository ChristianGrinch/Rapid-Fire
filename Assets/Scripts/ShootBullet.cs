using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{

    private int speed = 1; // Speed of the bullet --IMPLIMENTED
    private int range = 50; // How far the bullet can travel from its initial location before it despawns --IMPLIMENTED

    public GameObject[] enemy;

    private Vector3 spawnPos;
    private Vector3 currentPos;
    private Rigidbody bulletRb;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        bulletRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        //  Debug.Log(Vector3.Distance(currentPos, spawnPos));
        if (Vector3.Distance(currentPos, spawnPos) > range)
        {
            Destroy(gameObject);
        }
        else
        {
            //transform.Translate(Vector3.up * speed * Time.deltaTime);
            bulletRb.AddRelativeForce(Vector3.up * speed, ForceMode.Impulse);
        }
    }

    void OutOfBounds()
    {
        // write code to kill bullet out of bounds
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            //Debug.Log("collision detected");
        }
        //Debug.Log("function ran, no tag though");
    }
}
