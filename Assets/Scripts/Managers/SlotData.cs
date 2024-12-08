using System.Collections.Generic;
using UnityEngine;

public class SlotData : MonoBehaviour
{
	public ItemData itemData;

	public List<int> powerupCounts = new() { 0, 0, 0 };
	public enum ItemDataType
	{
		None,
		Primary,
		Secondary,
		Powerup,
		Armor,
	}
	public enum PrimaryType
	{
		None,
		AssaultRifle
	}
	public enum SecondaryType
	{
		None,
		Pistol
	}
	public enum PowerupType
	{
		None,
		Ammo,
		Health,
		Speed,
	}
	public enum ArmorType
	{
		None,
		Helmet,
		Chestplate,
		Leggings,
		Boots,
	}
	//public void SetSlotData(int num)
	//{
	//	switch (num)
	//	{
	//		case 0:
	//			NullifyData();
	//			itemType = ItemDataType.Gun;
	//			gunType = GunType.Pistol;
	//			break;
	//		case 1:
	//			NullifyData();
	//			itemType = ItemDataType.Gun;
	//			gunType = GunType.AssaultRifle;
	//			break;
	//		case 2:
	//			NullifyData();
	//			itemType = ItemDataType.Powerup;
	//			powerupType = PowerupType.Ammo;
	//			break;
	//		case 3:
	//			NullifyData();
	//			itemType = ItemDataType.Armor;
	//			armorType = ArmorType.Boots;
	//			break;
	//	}
	//}
	public void NullifyData()
	{
		itemData = new ItemData();
	}
}
[System.Serializable]
public class ItemData
{
	public SlotData.ItemDataType itemType;
	public SlotData.PrimaryType primaryType;
	public SlotData.SecondaryType secondaryType;
	public SlotData.PowerupType powerupType;
	public SlotData.ArmorType armorType;
	public int ammo;
}
