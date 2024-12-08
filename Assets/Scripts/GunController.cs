using UnityEngine;
using static SlotData;

public class GunController : MonoBehaviour
{
	public enum GunType
	{
		Pistol,
		AssaultRifle,
		None
	}

	public GameObject bullet;
	private GameObject player;
	private Vector3 offset = new(0, 0, 1.25f);
	private Rigidbody playerRb;

	private GameObject bulletParent;
	public GameObject[] gunObjects;

	public GunType currentGun;
	public int currentGunInt;
	private float fireRate = 0.1f;
	private float nextFireTime = 0f;

	public AudioClip audioClip;
	AudioSource audioData;

	// Start is called before the first frame update
	void Start()
	{
		bulletParent = GameManager.Instance.bullets;
		player = GameManager.Instance.player;
		playerRb = player.GetComponent <Rigidbody> ();

		audioData = GetComponent<AudioSource>();
		audioData.clip = audioClip;
	}

	// Update is called once per frame
	void Update()
	{
		if (!UIManager.Instance.isGamePaused)
		{
			ChangeCurrentGun();

			ShootGun();
		}
	}

	void ChangeCurrentGun()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			currentGun = GunType.Pistol;
			currentGunInt = 0;
		} else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			currentGun = GunType.AssaultRifle;
			currentGunInt = 1;
		}
	}

	void ShootGun()
	{
		
		ShootBullet shootBullet = bullet.GetComponent<ShootBullet>();

		float yRotation = playerRb.rotation.eulerAngles.y;

		if (yRotation < 0) yRotation += 360;

		switch (currentGun)
		{
			case GunType.AssaultRifle:
			{
				gunObjects[0].SetActive(false);
				gunObjects[1].SetActive(true);

				if (Input.GetMouseButton(0) && Time.time >= nextFireTime && TryUseAmmo(ItemDataType.Primary))
				{
					audioData.clip = audioClip;
					audioData.Play();

					SetBulletStats(shootBullet);
					InstantiateBullet(yRotation);

					nextFireTime = Time.time + fireRate;  // Reset next fire time
				}

				break;
			}
			case GunType.Pistol:
			{
				gunObjects[0].SetActive(true);
				gunObjects[1].SetActive(false);

				if (Input.GetMouseButtonDown(0) && TryUseAmmo(ItemDataType.Secondary))
				{
					audioData.clip = audioClip;
					audioData.Play();

					SetBulletStats(shootBullet);
					InstantiateBullet(yRotation);
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
		WeaponsUI.Instance.primary.GetComponent<SlotData>().itemData.ammo += 10;
		WeaponsUI.Instance.secondary.GetComponent<SlotData>().itemData.ammo += 15;
	}

	bool TryUseAmmo(ItemDataType itemDataType)
	{
		// Make sure the inventory is closed before shooting
		if (InventoryUI.Instance.inventoryMenu.activeSelf) return false;
		if (itemDataType == ItemDataType.Primary)
		{
			ItemData itemData = WeaponsUI.Instance.primary.GetComponent<SlotData>().itemData;
			int ammo = itemData.ammo;
			if(ammo > 0)
			{
				itemData.ammo--;
				return true;
			}
			else
			{
				return false;
			}
		}
		else if (itemDataType == ItemDataType.Secondary)
		{
			ItemData itemData = WeaponsUI.Instance.secondary.GetComponent<SlotData>().itemData;
			int ammo = itemData.ammo;
			if (ammo > 0)
			{
				itemData.ammo--;
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			// Return false incase the provided data type is NOT a weapon
			Debug.LogError("Tried to use ammo on a non-weapon data type!");
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
