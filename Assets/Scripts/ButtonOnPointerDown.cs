using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// None of this is my code. Thanks ChatGPT!

public class ButtonOnPointerDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	private Vector3 originalPos;
	private GridLayoutGroup gridLayoutGroup;

	void Start()
	{
		// Cache references
		gridLayoutGroup = InventoryManager.Instance.inventory.GetComponent<GridLayoutGroup>();
		if (gridLayoutGroup == null)
		{
			Debug.LogError("Grid layout group is NULL!");
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Ran OnPointerDown");

		// Store the original position
		originalPos = transform.position;

		// Disable the grid layout group
		if (gridLayoutGroup != null)
		{
			gridLayoutGroup.enabled = false;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		// Move the object with the mouse
		transform.position = Input.mousePosition;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		// Reset the position and re-enable the grid layout group
		transform.position = originalPos;

		if (gridLayoutGroup != null)
		{
			gridLayoutGroup.enabled = true;
		}
	}
}
