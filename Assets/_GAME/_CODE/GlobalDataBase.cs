using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Database", menuName = "Database")]
public class GlobalDataBase : ScriptableObject
{
    [SerializeField]
    private List<Item> _allItems; public List<Item> AllItems { get { return _allItems; } }

    // La seule et unique instance de DataBase
    public static GlobalDataBase instance;
    private void Awake()
    {
        // Permet de vérifier qu'il n'y a qu'une seule instance de la classe dans la scène
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Database");
                //DestroyImmediate(this);
            return;
        }
        instance = this;
    }
}