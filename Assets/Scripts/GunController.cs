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

	public GunType currentGun;
	public int currentGunInt;
	private float fireRate = 0.1f;
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
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			// If no secondary is selected then return
			if (InventoryManager.Instance.selectedGuns[0].gameObject == null) return;
			// If a secondary has been equipped but the new one to be equipped is different
			if (instantiatedPrimary != null && instantiatedPrimary != InventoryManager.Instance.selectedGuns[0].gameObject) Destroy(instantiatedPrimary);
			// Destory other gun type when swapping
			Destroy(instantiatedSecondary);
			currentGun = InventoryManager.Instance.selectedGuns[0].gunType;
			currentGunInt = 0;
			instantiatedPrimary = Instantiate(InventoryManager.Instance.selectedGuns[0].gameObject, player.transform);
		} 
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			// If no secondary is selected then return
			if (InventoryManager.Instance.selectedGuns[1].gameObject == null) return;
			// If a secondary has been equipped but the new one to be equipped is different
			if (instantiatedSecondary != null && instantiatedSecondary != InventoryManager.Instance.selectedGuns[1].gameObject) Destroy(instantiatedSecondary);
			// Destory other gun type when swapping
			Destroy(instantiatedPrimary);
			currentGun = InventoryManager.Instance.selectedGuns[1].gunType;
			currentGunInt = 1;
			instantiatedSecondary = Instantiate(InventoryManager.Instance.selectedGuns[1].gameObject, player.transform);
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
				if (instantiatedPrimary == null) return;
				if (Input.GetMouseButton(0) && Time.time >= nextFireTime && TryUseAmmo(ItemDataType.Primary))
				{
					audioData.clip = audioClip;
					audioData.Play();

					SetBulletStats(shootBullet, ItemDataType.Primary);
					InstantiateBullet(yRotation);

					nextFireTime = Time.time + fireRate;  // Reset next fire time
				}

				break;
			}
			case GunType.Pistol:
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
		float bulletSpeed = player.GetComponentInChildren<GunData>().gunStats.bulletSpeed;
		int bulletDamage = player.GetComponentInChildren<GunData>().gunStats.damage;
		int bulletRange = player.GetComponentInChildren<GunData>().gunStats.range;
		//bulletSpeed = InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats.bulletSpeed;
		//bulletDamage = InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats.damage;
		//bulletRange = InventoryManager.Instance.selectedGuns[0].gameObject.GetComponent<GunData>().gunStats.range;
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
		if (InventoryManager.Instance.selectedGuns[0].itemType != ItemDataType.None)
		{
			WeaponsUI.Instance.primary.GetComponent<SlotData>().itemData.ammo += 10;
			InventoryManager.Instance.selectedGuns[0] = WeaponsUI.Instance.primary.GetComponent<SlotData>().itemData;
		}

		if (InventoryManager.Instance.selectedGuns[1].itemType != ItemDataType.None)
		{
			WeaponsUI.Instance.secondary.GetComponent<SlotData>().itemData.ammo += 15;
			InventoryManager.Instance.selectedGuns[1] = WeaponsUI.Instance.secondary.GetComponent<SlotData>().itemData;
		}
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
				InventoryManager.Instance.selectedGuns[0].ammo = itemData.ammo;
				return true;
			}
			else
			{
				InventoryManager.Instance.selectedGuns[0].ammo = itemData.ammo;
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
				InventoryManager.Instance.selectedGuns[1].ammo = itemData.ammo;
				return true;
			}
			else
			{
				InventoryManager.Instance.selectedGuns[1].ammo = itemData.ammo;
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
