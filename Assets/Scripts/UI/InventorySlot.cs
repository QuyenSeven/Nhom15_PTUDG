using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private WeaponInfor weaponInfor;

    public WeaponInfor GetWeaponInfor()
    {
        return weaponInfor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Lấy chỉ số của slot được nhấp
        int index = transform.GetSiblingIndex();

        // Kích hoạt slot
        //Debug.Log($"Clicked on UI slot {index}");
        ActiveInventory.Instance.ToggleActiveHighlight(index);
    }
}
