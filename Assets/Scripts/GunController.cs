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
	public GameObject pistol;
	public GameObject assualtRifle;

	private int currentGun = 1;
	private float fireRate = 0.1f;
	private float nextFireTime = 0f;

	// Start is called before the first frame update
	void Start()
	{
		playerRb = player.GetComponent <Rigidbody> ();
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
					pistol.SetActive(true);
					assualtRifle.SetActive(false);

					if (Input.GetMouseButtonDown(0))
					{
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
					pistol.SetActive(false);
					assualtRifle.SetActive(true);

					if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
					{
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
}
