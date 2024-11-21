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
		InventoryUI.Instance.originalSlotPos = transform.position;
		InventoryUI.Instance.originalSlotNum = int.Parse(gameObject.name.Split(char.Parse(" "))[1]);
		Debug.Log("transfomr position "+transform.position);
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
			//isDragging = false;
			//Vector3 intialTransformPos = InventoryUI.Instance.originalSlotPos;
			//Vector3 initalHoveredSlotPos = InventoryUI.Instance.hoveredSlotPos;
			//GameObject slot = GameObject.Find("Slot " + slotNum);
			//if(slot != null)
			//{
			//	slot.transform.position = intialTransformPos;
			//}
			//else
			//{
			//	Debug.LogError("Slot is NULL!");
			//}
			//transform.position = initalHoveredSlotPos;
			transform.SetSiblingIndex(InventoryUI.Instance.hoveredSlotNum);
			GameObject.Find("Slot " + InventoryUI.Instance.hoveredSlotNum).transform.SetSiblingIndex(InventoryUI.Instance.originalSlotNum);
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		if(isDragging == false)
		{
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
