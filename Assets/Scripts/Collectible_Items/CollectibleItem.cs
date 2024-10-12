using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum CollectibleItemType { HPRecover, MPRecover };

[CreateAssetMenu(fileName = "New Collectible", menuName = "Collectible Item")]
public class CollectibleItem : ScriptableObject
{
    public string itemName;
    public CollectibleItemType type;
    public int value;


    public void PrintInfo()
    {
        Debug.Log("Item: " + itemName + ", Value: " + value + ", Type: " + type);
    }
}
