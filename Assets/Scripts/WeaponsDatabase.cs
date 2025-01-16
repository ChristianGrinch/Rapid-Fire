using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "WeaponsDatabase", menuName = "Database/Weapons")]
public class WeaponsDatabase : ScriptableObject
{
	[Header("Primary")]
	public List<GameObject> assaultRifles;
	[Header("Secondary")]
	public List<GameObject> pistols;
	public List<GameObject> subMachineGuns;

	// Empty lists that are cached when FindAllGameObjectsByLevel is called
	public List<GameObject> level1Weapons;
	public List<GameObject> level2Weapons;
	public List<GameObject> level3Weapons;
	public List<GameObject> level4Weapons;
	public List<GameObject> level5Weapons;
	private void OnEnable()
	{
		ClearCaches();
	}
	private void ClearCaches()
	{
		level1Weapons.Clear();
		level2Weapons.Clear();
		level3Weapons.Clear();
		level4Weapons.Clear();
		level5Weapons.Clear();
		Debug.Log("Cleared all caches");
	}
	public List<GameObject> FindGameObjects(string path)
	{
		//Weapons/Secondary/Pistol
		GameObject[] gameObjects = Resources.LoadAll<GameObject>(path);

		return new List<GameObject>(gameObjects);
	}
	public GameObject FindGameObjectByLevel(int level, string path)
	{
		List<GameObject> gameObjects = FindGameObjects(path);

		foreach(GameObject item in gameObjects)
		{
			string[] splitName = item.name.Split(" ");
			string lastPart = splitName[splitName.Length - 1];

			if(lastPart == level.ToString())
			{
				return item;
			}
		}
		Debug.LogError("Couldn't find an item by level!");
		Debug.LogError($"Path: {path}, Level: {level}");
		return null;
	}
	public List<GameObject> FindAllGameObjects()
	{
		GameObject[] gameObjects = Resources.LoadAll<GameObject>("Weapons");

		if (gameObjects == null || gameObjects.Length == 0)
		{
			Debug.LogError("No GameObjects found in Weapons folder.");
		}
		else
		{
			Debug.Log($"Found {gameObjects.Length} GameObjects in Weapons folder.");
		}

		return new List<GameObject>(gameObjects);
	}
	public List<GameObject> FindAllGameObjectsByLevel(int level)
	{
		var cachedObjects = CacheCheck(level);
		if (cachedObjects != null) // If the cache has a value (not null), just return the cached values
		{
			Debug.Log(cachedObjects.Count);
			return cachedObjects;
		} 
		
		List<GameObject> allGameObjects = FindAllGameObjects();
		List<GameObject> selectedGameObjects = new();

		foreach(var gameObject in allGameObjects)
		{
			string[] splitName = gameObject.name.Split(" ");
			string lastPart = splitName[splitName.Length - 1];

			if (lastPart == level.ToString())
			{
				selectedGameObjects.Add(gameObject);
			}
		}
		
		UpdateCache(level, selectedGameObjects); // Always update the cache 
		
		return selectedGameObjects;
	}
	private List<GameObject> CacheCheck(int level)
	{
		Debug.Log($"CacheCheck called for level {level}");
		return level switch // i made this myself but the shorthand stuff came from chat
		{
			1 => level1Weapons.Count > 0 ? level1Weapons : null,
			2 => level2Weapons.Count > 0 ? level2Weapons : null,
			3 => level3Weapons.Count > 0 ? level3Weapons : null,
			4 => level4Weapons.Count > 0 ? level4Weapons : null,
			5 => level5Weapons.Count > 0 ? level5Weapons : null,
			_ => throw new ArgumentException($"Invalid level: {level}", nameof(level))
		};
	}

	private void UpdateCache(int level, List<GameObject> selectedGameObjects)
	{
		Debug.Log($"UpdateCache called for level {level}. SelectedGameObjects count: {selectedGameObjects.Count}");
		switch (level) // Caching
		{
			case 1:
				Debug.Log("Ran caching 1");
				if (level1Weapons.Count == 0) level1Weapons = selectedGameObjects;
				break;
			case 2:
				Debug.Log("Ran caching 2");
				if (level2Weapons.Count == 0) level2Weapons = selectedGameObjects;
				break;
			case 3:
				Debug.Log("Ran caching 3");
				if (level3Weapons.Count == 0) level3Weapons = selectedGameObjects;
				break;
			case 4:
				Debug.Log("Ran caching 4");
				if (level4Weapons.Count == 0) level4Weapons = selectedGameObjects;
				break;
			case 5:
				Debug.Log("Ran caching 5");
				if (level5Weapons.Count == 0) level5Weapons = selectedGameObjects;
				break;
			default:
				throw new ArgumentException($"Invalid level: {level}", nameof(level));
		}
		Debug.Log($"Cache updated for level {level}");
	}

	public string ReturnPath(ItemData itemData)
	{
		string path = "Weapons/";
		string name = itemData.gunType.ToString();
		name = Regex.Replace(name, "(?<!^)([A-Z])", " $1");
		path += itemData.itemType.ToString() + "/" +  name;
		return path;
	}
}
