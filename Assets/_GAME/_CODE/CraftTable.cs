using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTable : Interractable
{
    // Contenir les infos de la table de craft (recettes, taille, type de craft quoi...)
    [SerializeField]
    private Recettes _dataTable;

    private CraftManager _craftManager;

    private void Start()
    {
        _craftManager = FindObjectOfType<CraftManager>();
    }

    public override void Interract()
    {
        _craftManager.ActiveCraftMode(_dataTable);
    }
}