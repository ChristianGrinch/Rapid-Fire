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
 	public GameObject[] guns;

	private int currentGun = 1;
	private float fireRate = 0.1f;
	private float nextFireTime = 0f;
    private int[] ammo = new int[] { 20, 40 };
	public int pistolAmmo;
	public int assaultRifleAmmo;


    // Start is called before the first frame update
    void Start()
	{
		playerRb = player.GetComponent <Rigidbody> ();
        pistolAmmo = ammo[0];
        assaultRifleAmmo = ammo[1];
    }

	// Update is called once per frame
	void Update()
	{
		ChangeCurrentGun();

        ShootGun();
    }

	void ChangeCurrentGun()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			currentGun = 1;
		} else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			currentGun = 2;
		}
	}

	void ShootGun()
	{
		ShootBullet shootBullet = bullet.GetComponent<ShootBullet>();

		float yRotation = playerRb.rotation.eulerAngles.y;
		if (yRotation < 0)
		{
			yRotation += 360;
		}

		switch (currentGun)
		{
			case 1:
				{
					guns[0].SetActive(true);
					guns[1].SetActive(false);

					if (Input.GetMouseButtonDown(0) && AmmoCheck(0))
					{
						Debug.Log("pistol ammo is: "+ammo[0]);
						int pistolDamage = 10;
						int pistolRange = 50;
						float pistolSpeed = 1f;
						shootBullet.UpdateStats(pistolDamage, pistolRange, pistolSpeed);

						Vector3 spawnPosition = player.transform.TransformPoint(offset);
						GameObject instantiatedBullet = Instantiate(bullet, spawnPosition, Quaternion.Euler(90, yRotation, 0));
						instantiatedBullet.transform.parent = bulletParent.transform; // Sets parent
					}

					break;
				}

			case 2:
				{
					guns[0].SetActive(false);
					guns[1].SetActive(true);

					if (Input.GetMouseButton(0) && Time.time >= nextFireTime && AmmoCheck(1))
					{
                        Debug.Log("ar ammo is: " + ammo[1]);
                        int assaultRifleDamage = 8;
						int assaultRifleRange = 75;
						float assaultRifleSpeed = 1.5f;
						shootBullet.UpdateStats(assaultRifleDamage, assaultRifleRange, assaultRifleSpeed);

						Vector3 spawnPosition = player.transform.TransformPoint(offset);
						GameObject instantiatedBullet = Instantiate(bullet, spawnPosition, Quaternion.Euler(90, yRotation, 0));
						instantiatedBullet.transform.parent = bulletParent.transform; // Sets parent

						nextFireTime = Time.time + fireRate;  // Reset next fire time
					}

					break;
				}
		}

	}
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ammo"))
		{
            CollectAmmo();
            Destroy(collision.gameObject);
		}
    }
    void CollectAmmo()
	{
		ammo[0] += 10;
        ammo[1] += 15;
	}

	bool AmmoCheck(int currentGun)
	{
		if (ammo[currentGun] > 0)
		{
			ammo[currentGun] = ammo[currentGun] - 1;
            return true;
		}
		else
		{
            return false;
		}

	}
}
