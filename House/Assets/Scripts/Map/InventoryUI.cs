using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Sprite dirtSprite;
    public Sprite grassSprite;
    public Sprite waterSprite;
    public Sprite slimeSprite;
    public Sprite skullSprite;
    public Sprite crystalSprite;

    public List<Transform> Slot = new List<Transform>();
    public GameObject SlotItem;
    List<GameObject> items = new List<GameObject>();

    public int selectedIndex = -1;

    public void UpdateInventory(Inventory myInven)
    {
        foreach(var slotItems in items)
        {
            Destroy(slotItems);
        }
        items.Clear();

        int idx = 0;
        foreach(var item in myInven.items)
        {
            var go = Instantiate(SlotItem, Slot[idx].transform);
            go.transform.localPosition = Vector3.zero;
            SlotItemPrefabs sItem = go.GetComponent<SlotItemPrefabs>();
            items.Add(go);

            switch (item.Key)
            {
                case ItemType.floor2:
                    sItem.ItemSetting(dirtSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.floor1:
                    sItem.ItemSetting(grassSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Slime:
                    sItem.ItemSetting(waterSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Skull:
                    sItem.ItemSetting(skullSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Crystal:
                    sItem.ItemSetting(crystalSprite, "x" + item.Value.ToString(), item.Key);
                    break;           
            }

            idx++;
        }
    }

    private void Update()
    {
        for (int i = 0; i < Mathf.Min(9,Slot.Count); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSelectedIndex(i);
            }
        }
    }

    public void SetSelectedIndex(int idx)
    {
        ResetSelection();
        if( selectedIndex == idx)
        {
            selectedIndex = -1;
        }
        else
        {
            if (idx >= items.Count)
            {
                selectedIndex = -1;
            }
            else
            {
                SetSelection(idx);
                selectedIndex = idx;
            }
        }
    }

    public void ResetSelection()
    {
        foreach(var slot in Slot)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }

    void SetSelection(int _idx)
    {
        Slot[_idx].GetComponent<Image>().color = Color.yellow;
    }
    
    public ItemType GetInventorySlot()
    {
        return items[selectedIndex].GetComponent<SlotItemPrefabs>().itemType;
    }
}
