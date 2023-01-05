using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) //Layer 7 = collectibles
        {
            switch (collision.gameObject.tag)
            {
                case "Item":
                    if (collision.gameObject.TryGetComponent(out ItemComponent item))
                    {
                        if(inventory.AddItem(item.item))
                        {
                            Destroy(collision.gameObject);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}