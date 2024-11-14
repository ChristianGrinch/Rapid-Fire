using UnityEngine;

public class SlotData : MonoBehaviour
{
	public ItemDataType itemType;

	public GunType gunType;
	public PowerupType powerupType;
	public ArmorType armorType;
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
		AssaultRifle,
		None
	}
	public enum PowerupType
	{
		Ammo,
		Health,
		Speed,
		None
	}
	public enum ArmorType
	{
		Helmet,
		Chestplate,
		Leggings,
		Boots,
		None
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
		gunType = GunType.None;
		powerupType = PowerupType.None;
		armorType = ArmorType.None;
	}
}
