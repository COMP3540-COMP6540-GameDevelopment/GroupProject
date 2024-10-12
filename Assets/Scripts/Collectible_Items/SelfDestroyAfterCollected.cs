using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyAfterCollected : MonoBehaviour
{
    public GameObject collected;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Collide with player
                GameObject effect = Instantiate(collected, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
