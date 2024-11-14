using UnityEngine;

public class SlotData : MonoBehaviour
{
	public ItemDataType itemType;

	public GunType? gunType;
	public PowerupType? powerupType;
	public ArmorType? armorType;
	public enum ItemDataType
	{
		Gun,
		Powerup,
		Armor,
		None
	}
	public enum GunType
	{
		Pistol,
		AssaultRifle
	}
	public enum PowerupType
	{
		Ammo,
		Health,
		Speed
	}
	public enum ArmorType
	{
		Helmet,
		Chestplate,
		Leggings,
		Boots
	}
	public void SetSlotData(int num)
	{
		switch (num)
		{
			case 0:
				NullifyData();
				itemType = ItemDataType.Gun;
				gunType = GunType.Pistol;
				break;
			case 1:
				NullifyData();
				itemType = ItemDataType.Gun;
				gunType = GunType.AssaultRifle;
				break;
			case 2:
				NullifyData();
				itemType = ItemDataType.Powerup;
				powerupType = PowerupType.Ammo;
				break;
			case 3:
				NullifyData();
				itemType = ItemDataType.Armor;
				armorType = ArmorType.Boots;
				break;
		}
	}
	public void NullifyData()
	{
		itemType = ItemDataType.None;
		gunType = null;
		powerupType = null;
		armorType = null;
	}
}
