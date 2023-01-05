using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField, Min(1), Tooltip("Maximum d'instance de l'objet pouvant être stacké")]
    private int _maxInstance = 1; public int MaxInstance { get { return _maxInstance; } set { _maxInstance = value; } }

    [SerializeField, Tooltip("Image de l'item")]
    private Sprite _image; public Sprite Image { get { return _image; } }

    [SerializeField, Tooltip("Description de l'item")]
    private string _description; public string Description { get { return _description; } }

    [SerializeField, Tooltip("Type d'item")]
    private ItemType _type; public ItemType Type { get { return _type; } }
}

public enum ItemType
{
    Weapon,
    Consumable,
    Craft
}