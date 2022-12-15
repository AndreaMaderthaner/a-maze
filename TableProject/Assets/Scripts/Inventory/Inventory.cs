using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Dictionary<pickUpObjCode, GameObject> itemsInScene;

    void Awake()
    {
        instance = this;

    }
    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        items.Add(item);
        InventoryUI.instance.addItemToUI(item);
    }
    public void Init()
    {
        itemsInScene = new Dictionary<pickUpObjCode, GameObject>();

        var items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Debug.Log(item.GetComponent<ItemPickup>().item.objCode);
            itemsInScene.Add(item.GetComponent<ItemPickup>().item.objCode, item);
        }
    }
    public void OnMsg(pickUpObjCode objCode, bool use)
    {
        if (!items.Contains(itemsInScene[objCode].GetComponent<ItemPickup>().item))
        {
            var item = itemsInScene[objCode];
            Add(item.GetComponent<ItemPickup>().item);
            item.SetActive(false);
        }
    }
    public void Reset()
    {
        itemsInScene = new Dictionary<pickUpObjCode, GameObject>();
        items = new List<Item>();
        InventoryUI.instance.clear();
    }

}