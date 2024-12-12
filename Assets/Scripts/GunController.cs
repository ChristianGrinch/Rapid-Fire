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
		if (!UIManager.Instance.isGamePaused)
		{
			ChangeCurrentGun();

			ShootGun();
		}
	}

	void ChangeCurrentGun()
	{
		if (currentGunData.gunType == GunType.None) currentGunData = InventoryManager.Instance.selectedGuns[0];
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			// If no primary is selected then return
			if (InventoryManager.Instance.selectedGuns[0].gameObject == null) return;
			// If a primary has been equipped but the new one to be equipped is different, destroy the existing primary
			if (instantiatedPrimary != null && instantiatedPrimary != InventoryManager.Instance.selectedGuns[0].gameObject) Destroy(instantiatedPrimary);
			// Destory other gun type when swapping
			Destroy(instantiatedSecondary);
			currentGunData = InventoryManager.Instance.selectedGuns[0];
			currentGunInt = 0;
			instantiatedPrimary = Instantiate(InventoryManager.Instance.selectedGuns[0].gameObject, player.transform);
		} 
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (InventoryManager.Instance.selectedGuns[1].gameObject == null) return;
			if (instantiatedSecondary != null && instantiatedSecondary != InventoryManager.Instance.selectedGuns[1].gameObject) Destroy(instantiatedSecondary);
			Destroy(instantiatedPrimary);
			currentGunData = InventoryManager.Instance.selectedGuns[1];
			currentGunInt = 1;
			instantiatedSecondary = Instantiate(InventoryManager.Instance.selectedGuns[1].gameObject, player.transform);
		}
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
					if (instantiatedPrimary == null) return;
					if (Input.GetMouseButton(0) && Time.time >= nextFireTime && TryUseAmmo(ItemDataType.Primary))
					{
						audioData.clip = audioClip;
						audioData.Play();

						SetBulletStats(shootBullet, ItemDataType.Primary);
						InstantiateBullet(yRotation);

						nextFireTime = Time.time + InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats.firerate;  // Reset next fire time
					}
					break;
				}
			case false:
				{
					if (instantiatedSecondary == null) return;
					if (Input.GetMouseButtonDown(0) && TryUseAmmo(ItemDataType.Secondary))
					{
						audioData.clip = audioClip;
						audioData.Play();

						SetBulletStats(shootBullet, ItemDataType.Secondary);
						InstantiateBullet(yRotation);
					}
					break;
				}
		}

	}

	void SetBulletStats(ShootBullet shootBullet, ItemDataType itemDataType)
	{
		float bulletSpeed;
		int bulletDamage;
		int bulletRange;
		switch (itemDataType)
		{
			case ItemDataType.Primary:
				bulletSpeed = InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats.bulletSpeed;
				bulletDamage = InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats.damage;
				bulletRange = InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats.range;
				shootBullet.UpdateStats(bulletDamage, bulletRange, bulletSpeed);
				return;
			case ItemDataType.Secondary:
				bulletSpeed = InventoryManager.Instance.selectedGuns[1].gameObject.GetComponent<GunData>().gunStats.bulletSpeed;
				bulletDamage = InventoryManager.Instance.selectedGuns[1].gameObject.GetComponent<GunData>().gunStats.damage;
				bulletRange = InventoryManager.Instance.selectedGuns[1].gameObject.GetComponent<GunData>().gunStats.range;
				shootBullet.UpdateStats(bulletDamage, bulletRange, bulletSpeed);
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
		if (InventoryUI.Instance.inventoryMenu.activeSelf || ShopUI.Instance.shopMenu.activeSelf) return false;
		if (itemDataType == ItemDataType.Primary)
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
		else if (itemDataType == ItemDataType.Secondary)
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
