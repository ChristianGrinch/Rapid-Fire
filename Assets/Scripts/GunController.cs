using UnityEngine;
using static SlotData;

public class GunController : MonoBehaviour
{
	public enum GunType
	{
		None,
		Pistol,
		AssaultRifle,
		SubMachineGun
	}

	public GameObject bullet;
	private GameObject player;
	private Vector3 offset = new(0, 0, 1.25f);
	private Rigidbody playerRb;

	private GameObject bulletParent;

	public ItemData currentGunData;
	public GunData gunData;
	public int currentGunInt;
	private float nextFireTime = 0f;

	public AudioClip audioClip;
	AudioSource audioData;

	public GameObject instantiatedPrimary;
	public GameObject instantiatedSecondary;

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
		if (!UIManager.Instance.IsGamePaused())
		{
			if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeCurrentGun(0);
			if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeCurrentGun(1);
			if (Input.GetMouseButton(0)) ShootGun();
		}
	}
	void ChangeCurrentGun(int index)
	{
		GameObject instantiatedWeapon = index == 0 ? instantiatedPrimary : instantiatedSecondary;
		GameObject otherWeapon = index == 0 ? instantiatedSecondary : instantiatedPrimary;
		
		// If no weapon of the corresponding index is selected, return
		if (!InventoryManager.Instance.selectedGuns[index].gameObject) return;

		// If the new weapon to equip is different, destroy the existing one
		if (instantiatedWeapon && instantiatedWeapon != InventoryManager.Instance.selectedGuns[index].gameObject)
			Destroy(instantiatedWeapon);

		Destroy(otherWeapon);
		currentGunData = InventoryManager.Instance.selectedGuns[index];
		gunData = currentGunData.gameObject.GetComponent<GunData>();
		currentGunInt = index;
		
		if (index == 0)
			instantiatedPrimary = Instantiate(currentGunData.gameObject, player.transform);
		else
			instantiatedSecondary = Instantiate(currentGunData.gameObject, player.transform);
	}
	void ShootGun()
	{
		ShootBullet shootBullet = bullet.GetComponent<ShootBullet>();
		float yRotation = playerRb.rotation.eulerAngles.y;

		if (yRotation < 0) yRotation += 360;

		switch (currentGunData.isWeaponAutomatic)
		{
			case true:
				{
					//Debug.Log("Weapon is automatic.");
					//if (instantiatedPrimary == null) return;
					if (Input.GetMouseButton(0) && Time.time >= nextFireTime && TryUseAmmo(currentGunData.itemType))
					{
						audioData.clip = audioClip;
						audioData.Play();

						SetBulletStats(shootBullet, currentGunData.itemType);
						Shoot(yRotation);

						nextFireTime = Time.time + currentGunData.gameObject.GetComponent<GunData>().gunStats.firerate;  // Reset next fire time
					}
					break;
				}
			case false:
				{
					//Debug.Log("Weapon is not automatic.");
					//if (instantiatedSecondary == null) return;
					if (Input.GetMouseButtonDown(0) && TryUseAmmo(currentGunData.itemType))
					{
						audioData.clip = audioClip;
						audioData.Play();

						SetBulletStats(shootBullet, currentGunData.itemType);
						Shoot(yRotation);
					}
					break;
				}
		}

	}

	void SetBulletStats(ShootBullet shootBullet, ItemDataType itemDataType)
	{
		switch (itemDataType)
		{
			case ItemDataType.Primary:
				GunStats primaryData = InventoryManager.Instance.primaryData;
				shootBullet.UpdateStats(primaryData.damage, primaryData.range, primaryData.bulletSpeed);
				return;
			case ItemDataType.Secondary:
				GunStats secondaryData = InventoryManager.Instance.secondaryData;
				shootBullet.UpdateStats(secondaryData.damage, secondaryData.range, secondaryData.bulletSpeed);
				return;
		}
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
		if (InventoryManager.Instance.selectedGuns[0].itemType != ItemDataType.None)
		{
			InventoryManager.Instance.selectedGuns[0].ammo += 16;
		}

		if (InventoryManager.Instance.selectedGuns[1].itemType != ItemDataType.None)
		{
			InventoryManager.Instance.selectedGuns[1].ammo += 10;
		}
	}

	bool TryUseAmmo(ItemDataType itemDataType)
	{
		// Make sure the inventory and shop are closed before shooting
		if (UIManager.Instance.IsInterfaceOpen(InterfaceElements.Inventory) || UIManager.Instance.IsInterfaceOpen(InterfaceElements.Shop)) return false;
		switch (itemDataType)
		{
			case ItemDataType.Primary:
			{
				ItemData itemData = InventoryManager.Instance.selectedGuns[0];
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
			case ItemDataType.Secondary:
			{
				ItemData itemData = InventoryManager.Instance.selectedGuns[1];
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
			default:
				// Return false incase the provided data type is NOT a weapon
				Debug.LogError("Tried to use ammo on a non-weapon data type!");
				return false;
		}
	}

	void Shoot(float yRotation)
	{
		float accuracy = gunData.gunStats.accuracy;
		float spread = gunData.gunStats.spread;

		float maxAngle = Random.Range(-spread, spread);
		float actualOffset = maxAngle * (100f - accuracy) / 100; // 10 * (100f - 80) / 100 Results in a 2 degree variance
		yRotation += actualOffset;
		
		Vector3 spawnPosition = player.transform.TransformPoint(offset);
		GameObject instantiatedBullet = Instantiate(bullet, spawnPosition, Quaternion.Euler(90, yRotation, 0));
		instantiatedBullet.transform.parent = bulletParent.transform; // Sets parent
	}
}
