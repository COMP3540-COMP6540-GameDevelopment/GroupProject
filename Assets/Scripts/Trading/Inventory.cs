using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<CollectibleItem> keys;
    public Dictionary<CollectibleItem, int> CollectibleItems = new Dictionary<CollectibleItem, int>();

    private void Awake()
    {
        keys = new List<CollectibleItem>(CollectibleItems.Keys);
    }

    public void AddItem(CollectibleItem item)
    {
        
        if (CollectibleItems.ContainsKey(item))
        {
            CollectibleItems[item] += 1;
        } else
        {
            CollectibleItems[item] = 1;
            keys.Add(item);
        }
    }


}


