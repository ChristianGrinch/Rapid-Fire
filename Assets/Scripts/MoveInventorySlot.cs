using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveInventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	private GridLayoutGroup gridLayoutGroup;

	private bool isDragging;

	[Header("Dragged Slot")]
	private GameObject draggedSlot;
	private int draggedSlotNumber = -1;
	private int draggedSlotIndex = -1; // index relative to siblings in the gridlayoutgroup
	void Start()
    {
		gridLayoutGroup = InventoryManager.Instance.gridLayoutGroup;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log(gameObject.name.Split(char.Parse(" "))[0]);
		Debug.Log(gameObject.name.Split(char.Parse(" "))[1]);
		InventoryManager.Instance.hoveredSlotNumber = int.Parse(gameObject.name.Split(char.Parse(" "))[1]);
		InventoryManager.Instance.hoveredSlot = GameObject.Find("Slot " + InventoryManager.Instance.hoveredSlotNumber);
		InventoryManager.Instance.hoveredSlotIndex = InventoryManager.Instance.hoveredSlot.transform.GetSiblingIndex();
		Debug.Log("Slot " + InventoryManager.Instance.hoveredSlotNumber + " entered.");
	}
	public void OnPointerDown(PointerEventData eventData)
	{ 
		draggedSlotNumber = int.Parse(gameObject.name.Split(char.Parse(" "))[1]);
		draggedSlot = GameObject.Find("Slot " + draggedSlotNumber);
		draggedSlotIndex = draggedSlot.transform.GetSiblingIndex();
	}
	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log("Dragging slot " + draggedSlotNumber + ".");
		isDragging = true;
		transform.position = Input.mousePosition;
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		if(InventoryManager.Instance.hoveredSlotNumber != -1 && isDragging) //if you let go while on a slot:
		{
			isDragging = false;
			draggedSlot.transform.SetSiblingIndex(InventoryManager.Instance.hoveredSlotIndex);
			InventoryManager.Instance.hoveredSlot.transform.SetSiblingIndex(draggedSlotIndex);
			// Must be disabled and re-enabled in order to "reset" the dragged slot position
			gridLayoutGroup.enabled = false;
			gridLayoutGroup.enabled = true;
		} 
		else if (isDragging)
		{
			// resets dragged slot position
			gridLayoutGroup.enabled = false;
			gridLayoutGroup.enabled = true;
		}
		draggedSlotNumber = -1;
		draggedSlot = null;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!isDragging)
		{
			InventoryManager.Instance.hoveredSlotNumber = -1;
			InventoryManager.Instance.hoveredSlot = null;
		}
	}
}
