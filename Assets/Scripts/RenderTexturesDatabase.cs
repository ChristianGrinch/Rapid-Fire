using System;
using System.Collections.Generic;
using UnityEngine;
using static SlotData;

[CreateAssetMenu(fileName = "RenderTexturesDatabase", menuName = "Database/Render Textures")]
public class RenderTexturesDatabase : ScriptableObject
{
	[Header("Cache")] 
	public List<RenderTexture> primaryWeaponsRT;
	public List<RenderTexture> secondaryWeaponsRT;
	public List<RenderTexture> powerupsRT;
	public List<RenderTexture> otherRT;
	
	[Header("Indexing")] // idk what else to call it since they just show which render texture is for which object
	public List<PrimaryType> primaryTypes;
	public List<SecondaryType> secondaryTypes;
	public List<PowerupType> powerupTypes;
	public List<string> otherTypes;

	public List<RenderTexture> FindRenderTextures()
	{
		List<RenderTexture> renderTextures = new List<RenderTexture>();
		throw new NotImplementedException();
	}
}