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
                GameObject player = collision.gameObject;
                GameObject effect = Instantiate(collected, transform.position, Quaternion.identity);
                CollectibleItemType type = collectibleItemData.type;
                
                if (type == CollectibleItemType.HPRecover)
                {
                    player.GetComponent<BattleScript>().RecoverHP(collectibleItemData.value);

                    player.GetComponent<DisplayHUD>().UpdateStatus();
                    Debug.Log($"Player HP recovered by {collectibleItemData.value}");
                }

                Destroy(gameObject);
            }
        }
    }
}
