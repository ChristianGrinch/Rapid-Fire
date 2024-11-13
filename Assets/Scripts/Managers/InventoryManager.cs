using System;
using System.Collections.Generic;
using UnityEngine;
public enum GunTypes
{
	Pistol,
	AssaultRifle
}
public class InventoryManager : MonoBehaviour
{
	
}
public class InventoryData
{
	static int gunTypesLength = Enum.GetValues(typeof(GunTypes)).Length;

	List<bool> ownedGuns = new(gunTypesLength);
	List<List<int>> gunUpgrades = new(gunTypesLength);
	List<Enum> slotData;
	public void SlotData()
	{

	}
}
