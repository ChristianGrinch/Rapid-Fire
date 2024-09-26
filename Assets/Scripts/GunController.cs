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
 	public GameObject[] gunObjects;

    public GunType currentGun = GunType.Pistol;
    private float fireRate = 0.1f;
	private float nextFireTime = 0f;
    private int[] ammo = new int[] { 20, 40 };  // ammo[0] = pistol ammo, ammo[1] = assault rifle ammo

    AudioSource audioData;

	public enum GunType
	{
		Pistol,
		AssaultRifle
	}

    // Start is called before the first frame update
    void Start()
	{
		playerRb = player.GetComponent <Rigidbody> ();

        audioData = GetComponent<AudioSource>();
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
			currentGun = GunType.Pistol;
		} else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			currentGun = GunType.AssaultRifle;
		}
	}

	void ShootGun()
	{
        
        ShootBullet shootBullet = bullet.GetComponent<ShootBullet>();

		float yRotation = playerRb.rotation.eulerAngles.y;

		if (yRotation < 0) yRotation += 360;

		switch (currentGun)
		{
			case GunType.Pistol:
				{
					gunObjects[0].SetActive(true);
					gunObjects[1].SetActive(false);

					if (Input.GetMouseButtonDown(0) && TryUseAmmo(0))
					{
                        audioData.Play();

                        SetBulletStats(shootBullet);
                        InstantiateBullet(yRotation);
                    }

					break;
				}

			case GunType.AssaultRifle:
				{
					gunObjects[0].SetActive(false);
					gunObjects[1].SetActive(true);

					if (Input.GetMouseButton(0) && Time.time >= nextFireTime && TryUseAmmo(currentGun))
					{
                        audioData.Play();

						SetBulletStats(shootBullet);
                        InstantiateBullet(yRotation);

                        nextFireTime = Time.time + fireRate;  // Reset next fire time
					}

					break;
				}
		}

	}

	void SetBulletStats(ShootBullet shootBullet)
	{
		float bulletSpeed;
		int bulletDamage;
		int bulletRange;

		switch (currentGun)
		{
			case GunType.Pistol:
				bulletSpeed = 1f;
				bulletDamage = 10;
				bulletRange = 50;
				break;

			case GunType.AssaultRifle:
				bulletSpeed = 1.5f;
				bulletDamage = 8;
				bulletRange = 75;
				break;
			default:
				return;
		}

		shootBullet.UpdateStats(bulletDamage, bulletRange, bulletSpeed);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            CollectAmmo();
            Destroy(other.gameObject);
        }
    }
    void CollectAmmo()
	{
		ammo[0] += 10;
        ammo[1] += 15;
	}

	bool TryUseAmmo(GunType gunType)
	{
		int gunIndex = (int)gunType;

		if (ammo[gunIndex] > 0)
		{
			ammo[gunIndex]--;
            return true;
		}
		else
		{
            return false;
		}

	}

	void InstantiateBullet(float yRotation)
	{
        Vector3 spawnPosition = player.transform.TransformPoint(offset);
        GameObject instantiatedBullet = Instantiate(bullet, spawnPosition, Quaternion.Euler(90, yRotation, 0));
        instantiatedBullet.transform.parent = bulletParent.transform; // Sets parent
    }
}
