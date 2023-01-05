using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Gère le relai entre l'interface (boutons) et le CraftManager
/// </summary>
public class CraftInterface : MonoBehaviour
{
    public GameObject _quitButton;

    private CraftManager _craftManager;

    [SerializeField, Tooltip("Liste des bouttons des ingrédients de craft")]
    private List<Button> _components;
    [SerializeField]
    private Button _produit;

    [SerializeField, Tooltip("Liste de boutons de l'inventaire")]
    private List<Button> _inventoryButtons;

    #region Enable

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_quitButton);

        _craftManager = FindObjectOfType<CraftManager>();

        SetInventoryInterface();
    }
    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        ClearCraftZoneInterface();
        ClearInventoryInterface();
    }

    #endregion

    public void AddComponent(Button bouton)
    {
        if(bouton.TryGetComponent(out ItemComponent i))
        {
            if (_craftManager.ActualComponents.Count < _components.Count)
            {
                _craftManager.AddCraftComponenent(i.item);
                _craftManager.Inventory.RemoveItem(i.item);
                RefreshInterface();
            }
        }
        else
        {
            Debug.LogWarning("Pas ItemComponent associé");
        }
    }

    public void RemoveComponent(Button bouton)
    {
        if (bouton.TryGetComponent(out ItemComponent i))
        {
            if(i.item != null)
            {
                _craftManager.RemoveCraftComponent(i.item);
                _craftManager.Inventory.AddItem(i.item);
                RefreshInterface();
            }
        }
    }

    public void Craft(Button button)
    {
        if (button.TryGetComponent(out ItemComponent i))
        {
            if (i.item != null)
            {
                // On remet tout les items dans l'inventaire, c'est le CraftManager qui est chargé de les suppprimer définitivement
                for (int j = 0; j < _components.Count; j++)
                {
                    ItemComponent tmp = _components[j].GetComponent<ItemComponent>();
                    if(tmp.item != null)
                    {
                        _craftManager.Inventory.AddItem(tmp.item);
                    }
                }

                _craftManager.Craft();
                RefreshInterface();
            }
        }
    }

    #region RefreshInterface

    public void RefreshInterface()
    {
        SetInventoryInterface();
        SetCraftZoneInterface();
    }

    /// <summary>
    /// Affiche les images des items de l'inventaire dans les bouttons de la zone d'inventaire
    /// </summary>
    private void SetInventoryInterface()
    {
        ClearInventoryInterface();
        if (_craftManager != null)
        {
            for (int i = 0; i < _inventoryButtons.Count; i++)
            {
                //Test si onglet nécessaire
                if (i == _craftManager.Inventory.Inventory.Count)
                {
                        //Debug.Log("Plus besoin de slots d'inventaire : effacement des autres slots");
                    for (; i < _inventoryButtons.Count; i++)
                    {
                        _inventoryButtons[i].gameObject.SetActive(false);
                    }
                    return;
                }

                //Partie image
                _inventoryButtons[i].image.sprite = _craftManager.Inventory.Inventory[i].item.Image;
                if(_inventoryButtons[i].TryGetComponent(out ItemComponent item)) { item.item = _craftManager.Inventory.Inventory[i].item; }

                //Partie compteur
                if(_craftManager.Inventory.Inventory[i].instance > 1)
                {
                    _inventoryButtons[i].GetComponentInChildren<Text>().text = "x"+ _craftManager.Inventory.Inventory[i].instance;
                }

            }
        }
    }

    private void ClearInventoryInterface()
    {
        for (int i = 0; i < _inventoryButtons.Count; i++)
        {
            _inventoryButtons[i].gameObject.SetActive(true);
            _inventoryButtons[i].image.sprite = null;
            if(_inventoryButtons[i].TryGetComponent(out ItemComponent item)) { item.item = null;}
            _inventoryButtons[i].GetComponentInChildren<Text>().text = "";
        }
    }

    /// <summary>
    /// Affiche les images des items dans les bouttons de la zone de craft
    /// </summary>
    private void SetCraftZoneInterface()
    {
        ClearCraftZoneInterface();
        if (_craftManager != null)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if(i < _craftManager.ActualComponents.Count)
                {
                    if (_craftManager.ActualComponents[i] != null)
                    {
                        _components[i].image.sprite = _craftManager.ActualComponents[i].Image;
                        if (_components[i].TryGetComponent(out ItemComponent itemA)) { itemA.item = _craftManager.ActualComponents[i]; }
                    }
                }
            }

            if(_craftManager.ActualProduct!= null)
            {
                _produit.image.sprite = _craftManager.ActualProduct.Image;
                if (_produit.TryGetComponent(out ItemComponent itemB)) { itemB.item = _craftManager.ActualProduct; }
            }
        }
    }

    private void ClearCraftZoneInterface()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].image.sprite = null;
            if (_components[i].TryGetComponent(out ItemComponent itemA)) { itemA.item = null; }
        }

        _produit.image.sprite = null;
        if(_produit.TryGetComponent(out ItemComponent item)) { item.item = null; }
    }

    #endregion

    public void Quit()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            if (_components[i].TryGetComponent(out ItemComponent j))
            {
                if(j.item != null)
                {
                    _craftManager.Inventory.AddItem(j.item);
                }
            }
        }
        _craftManager.DesactiveCraftMode();
    }
}