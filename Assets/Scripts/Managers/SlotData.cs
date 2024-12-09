using MessagePack;
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
		Pistol,
		SubMachineGun,
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
	public void NullifyData()
	{
		itemData = new ItemData();
	}
}
[System.Serializable]
[MessagePackObject]
public class ItemData
{
	[Key(0)] public SlotData.ItemDataType itemType;
	[Key(1)] public SlotData.PrimaryType primaryType;
	[Key(2)] public SlotData.SecondaryType secondaryType;
	[Key(3)] public SlotData.PowerupType powerupType;
	[Key(4)] public SlotData.ArmorType armorType;
	[Key(5)] public int ammo;
}
