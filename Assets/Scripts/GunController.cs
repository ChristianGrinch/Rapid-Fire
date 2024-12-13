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
			if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeCurrentGun(0);
			if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeCurrentGun(1);
			ShootGun();
		}
	}
	void ChangeCurrentGun(int index)
	{
		GameObject instantiatedWeapon = index == 0 ? instantiatedPrimary : instantiatedSecondary;
		GameObject otherWeapon = index == 0 ? instantiatedSecondary : instantiatedPrimary;
		
		// If no weapon of the corresponding index is selected, return
		if (InventoryManager.Instance.selectedGuns[index].gameObject == null) return;

		// If the new weapon to equip is different, destory the existing one
		if (instantiatedWeapon != null && instantiatedWeapon != InventoryManager.Instance.selectedGuns[index].gameObject)
			Destroy(instantiatedWeapon);

		Destroy(otherWeapon);
		currentGunData = InventoryManager.Instance.selectedGuns[index];
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
		switch (itemDataType)
		{
			case ItemDataType.Primary:
				GunStats primaryData = InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats;
				shootBullet.UpdateStats(primaryData.damage, primaryData.range, primaryData.bulletSpeed);
				return;
			case ItemDataType.Secondary:
				GunStats secondaryData = InventoryManager.Instance.selectedGuns[1].gameObject.GetComponent<GunData>().gunStats;
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
