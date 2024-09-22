using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject bullet;
    public GameObject player;
    private Vector3 offset = new(0, 0, 1.25f);
    private Rigidbody playerRb;

    public GameObject bulletParent;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = player.GetComponent <Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
        float yRotation = playerRb.rotation.eulerAngles.y;
        if (yRotation < 0)
        {
            yRotation += 360;
        }

        //Debug.Log(yRotation);
        //Debug.Log(transform.rotation);

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("it ran");
            Vector3 spawnPosition = player.transform.TransformPoint(offset);
            GameObject instantiatedBullet = Instantiate(bullet, spawnPosition, Quaternion.Euler(90, yRotation, 0));
            instantiatedBullet.transform.parent = bulletParent.transform; // Sets parent
        }

    }
}
