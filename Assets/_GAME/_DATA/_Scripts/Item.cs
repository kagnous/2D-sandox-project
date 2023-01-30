using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField, Min(0), Tooltip("Identifiant numérique unique de l'item")]
    private int _ID; public int ID { get { return _ID; } }

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