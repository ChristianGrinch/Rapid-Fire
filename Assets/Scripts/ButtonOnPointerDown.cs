using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// The base of all of this is not my code. Thanks ChatGPT!

public class ButtonOnPointerDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	private Vector3 originalPos;
	private GridLayoutGroup gridLayoutGroup;
	private int slotNum;
	private int originalNum;
	private bool isDragging;

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
		// Store the original position
		originalPos = transform.position;
		InventoryUI.Instance.originalSlotNum = int.Parse(gameObject.name.Split(char.Parse(" "))[1]);
	}

	public void OnDrag(PointerEventData eventData)
	{
		isDragging = true;
		// Move the object with the mouse
		transform.position = Input.mousePosition;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if(InventoryUI.Instance.hoveredSlotNum != -1 && isDragging)
		{
			Debug.LogWarning("If inside onpoinerup ran.");
			isDragging = false;
			Transform originalSlot = transform;
			Transform hoveredSlot = GameObject.Find("Slot " + InventoryUI.Instance.hoveredSlotNum).transform;
			Debug.Log(hoveredSlot);
			if (hoveredSlot == null)
			{
				Debug.LogError("Hovered slot is NULL!");
			}

			int originalIndex = originalSlot.GetSiblingIndex();
			int hoveredIndex = hoveredSlot.GetSiblingIndex();

			hoveredSlot.SetSiblingIndex(originalIndex);
			originalSlot.SetSiblingIndex(hoveredIndex);

			// Must be disabled and reenabled in order to "reset" the dragged slot position
			gridLayoutGroup.enabled = false;
			gridLayoutGroup.enabled = true;
		}
		else if(isDragging)
		{
			if(originalPos != Vector3.zero)
			{
				transform.position = originalPos;
			}
			else
			{
				Debug.LogError("Original pos is NULL!");
			}
			
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("slot entered");
		if(isDragging == false)
		{
			Debug.Log("Set hovered slot num");
			InventoryUI.Instance.hoveredSlotNum = int.Parse(gameObject.name.Split(char.Parse(" "))[1]);
			InventoryUI.Instance.hoveredSlotPos = transform.position;
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		if (isDragging == false)
		{
			InventoryUI.Instance.hoveredSlotNum = -1;
			InventoryUI.Instance.hoveredSlotPos = new();
		}
	}
}
