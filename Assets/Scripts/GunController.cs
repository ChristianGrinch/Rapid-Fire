using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject bullet;
    public GameObject player;
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        bullet.transform.Translate(Vector3.forward, player.transform);
        Debug.Log(transform.rotation);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("it ran");
            Instantiate(bullet, transform.transform);
        }
    }
}
