using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField, Tooltip("Nombre max d'�l�ments dans l'inventaire")]
    private int _maxInventorySize = 10;

    [SerializeField, Tooltip("Tout les objets d�tenus par le joueur")]
    private List<ItemInventory> _inventory; public List<ItemInventory> Inventory { get { return _inventory; } set { _inventory = value; } }

    /// <summary>
    /// Ajoute un item � l'inventaire
    /// </summary>
    /// <returns>Si l'item a pu s'ajouter o� non</returns>
    public bool AddItem(Item item)
    {
        // Pour tout les �l�ments de l'inventaire
        for (int i = 0; i < _inventory.Count; i++)
        {
            // Si l'item est similaire � celui ajout�
            if (item == _inventory[i].item)
            {
                //Si l'item n'est pas � son stack maximal
                if (_inventory[i].instance < _inventory[i].item.MaxInstance)
                {
                    // On l'incr�mente de 1
                    _inventory[i].instance++;
                    return true;
                }
                // Sinon on continue de chercher
            }
        }
        // Si aucun �l�ment � �t� trouv�, on v�rifie qui reste de la place
        if (_inventory.Count < _maxInventorySize)
        {
            // On ajoute un nouvel �l�ment si y reste de la place
            _inventory.Add(new ItemInventory(item));
            return true;
        }
        else
        {
            // Sinon bah on ajoute pas l'item et on retourne false
            return false;
        }
    }

    /// <summary>
    /// Retire un item de l'inventaire
    /// </summary>
    public bool RemoveItem(Item item)
    {
        // Pour tout les �l�ments de l'inventaire
        for (int i = 0; i < _inventory.Count; i++)
        {
            // Si l'item est similaire � celui enlev�
            if (item == _inventory[i].item)
            {
                // Si il y a plus d'une instance
                if (_inventory[i].instance > 1)
                {
                    // On en enl�ve une
                    _inventory[i].instance--;
                    return true;
                }
                else
                {
                    // Sinon on enl�ve l'objet
                    _inventory.RemoveAt(i);
                    return true;
                }
            }
        }
        return false;
    }

    [System.Serializable]
    public class ItemInventory
    {
        public Item item;
        public int instance;

        public ItemInventory(Item item)
        {
            this.item = item;
            instance = 1;
        }
    }
}