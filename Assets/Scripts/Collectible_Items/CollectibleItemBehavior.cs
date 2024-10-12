using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItemBehavior : MonoBehaviour
{
    public GameObject collected;
    public CollectibleItem collectibleItemData;
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Collide with player
                GameObject effect = Instantiate(collected, transform.position, Quaternion.identity);

                Inventory playerInventory = collision.gameObject.GetComponent<Inventory>();
                CollectibleItem thisItem = gameObject.GetComponent<CollectibleItem>();
                if (thisItem != null && playerInventory != null)
                {
                    playerInventory.AddItem(gameObject.GetComponent<CollectibleItem>());
                    Debug.Log($"item {thisItem.itemName} is added to player inventory");
                }
                Destroy(gameObject);
            }
        }
    }
}
