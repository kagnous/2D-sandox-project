using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Data de l'item")]
    private Item _item; public Item item { get { return _item; } set { _item = value; } }
}