using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Gère le systeme de craft d'un point de vue Data UNIQUEMENT
/// </summary>
public class CraftManager : MonoBehaviour
{
    private PlayerInventory _inventory; public PlayerInventory Inventory { get { return _inventory; } set { _inventory = value; } }

    [SerializeField, Tooltip("Interface de craft")]
    private GameObject _craftPanel; public GameObject CraftPanel { get { return _craftPanel; } }

    private bool _isCrafting = false;

    private Recettes _actualRecettes;
    private List<Item> _actualComponents = new List<Item>(); public List<Item> ActualComponents { get { return _actualComponents; } }
    private Item _actualProduct; public Item ActualProduct { get { return _actualProduct; } }

    private void Start()
    {
        _inventory = FindObjectOfType<PlayerInventory>();
        _craftPanel.SetActive(false);
    }
    
    #region CraftMode On/Off

    public void ActiveCraftMode(Recettes dataTable)
    {
        if (!_isCrafting)
        {
            // Analyse la dataTable pour savoir quel craftPanel utiliser
            _actualRecettes = dataTable;
            _craftPanel.SetActive(true);
            _isCrafting = true;

            FindObjectOfType<PlayerController>().StopMoveInput();
        }
    }

    public void DesactiveCraftMode()
    {
        if (_isCrafting)
        {
            _actualComponents.Clear();
            _actualRecettes = null;
            _craftPanel.SetActive(false);
            _isCrafting = false;

            FindObjectOfType<PlayerController>().ResumeMoveInput();
        }
    }

    #endregion

    public void Craft()
    {
            //Debug.Log($"Craft {_actualProduct.name} ({_actualProduct.Description})");
        _inventory.AddItem(_actualProduct);
        for (int i = 0; i < _actualComponents.Count; i++)
        {
            _inventory.RemoveItem(_actualComponents[i]);
        }
        _actualComponents.Clear();
        _actualProduct = null;
    }

    // On part du principe que pour l'instant tout le craft c'est deux éléments pour en faire un. Ni plus ni moins
    public void AddCraftComponenent(Item item)
    {
        _actualComponents.Add(item);
        RefreshCraft();
    }

    public void RemoveCraftComponent(Item item)
    {
        _actualComponents.Remove(item);
        RefreshCraft();
    }

    public void RefreshCraft()
    {
        _actualProduct = TestRecettte(_actualComponents);
    }

    /// <summary> Teste différents composants et retourne un élément si une recette est possible </summary> /// 
    /// <param name="components">Les composants qui veulent êtres combinés</param>
    private Item TestRecettte(List<Item> components)
    {
        if(_actualRecettes!= null)
        {
            // Pour toutes les recettes connues...
            for (int i = 0; i < _actualRecettes.AllRecettes.Count; i++)
            {
                //Si il y a autant de composants fournis que demandés...
                if (_actualRecettes.AllRecettes[i].component.Count == components.Count)
                {
                    // On crée un booléen pour tester la validité de chaque composant fournis
                    bool isCraftable = true;
                    // Et chaque composant de la recette est testé...
                    for (int j = 0; j < _actualRecettes.AllRecettes[i].component.Count; j++)
                    {
                        // On teste si les composants fournis NE sont PAS dans la recette en checkant
                        // si les composants fournis contiennent celui de la recette (c'est un peu du yodaStyle...)
                        if (!components.Contains(_actualRecettes.AllRecettes[i].component[j]))
                        {
                            // Si c'est le cas, alors par de craft
                            isCraftable = false;
                            //O n arrête de tester cette recette (vu qu'on a mis un élément qui n'y figure pas)
                            break;
                        }
                    }
                    // Si tout les éléments étaient dans la recette
                    if (isCraftable)
                    {
                        return _actualRecettes.AllRecettes[i].product;
                    }
                }
            }
                //Debug.Log("Pas de recette correspondante");
        }
        else { Debug.LogWarning("Pas de recette disponible"); }
        return null;
    }
}