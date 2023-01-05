using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recettes", menuName = "Recette")]
public class Recettes : ScriptableObject
{
    [SerializeField,Tooltip("Diff�rentes recettes possibles")]
    private List<Recette> recettes; public List<Recette> AllRecettes { get { return recettes; } }
}

[System.Serializable]
public struct Recette
{
    /// <summary>
    /// Les ingr�dients n�cessaires � la recette
    /// </summary>
    public List<Item> component;
    /// <summary>
    /// R�sultat du la recette
    /// </summary>
    public Item product;
}