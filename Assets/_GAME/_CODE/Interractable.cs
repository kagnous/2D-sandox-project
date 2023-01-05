using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Interractable : MonoBehaviour
{
    private const int INTERRACTABLE_INT_VALUE = 6;

    /// <summary>
    /// Met le layer interractif si jamais l'�l�ment est interractible
    /// </summary>
    private void Awake()
    {
        if(gameObject.layer != INTERRACTABLE_INT_VALUE)
        {
            Debug.LogWarning(gameObject.name + " n'a pas le layer \"Interractable\" mais � un script qui h�rite de \"Interractable\"");
            gameObject.layer = INTERRACTABLE_INT_VALUE;
        }
    }

    /// <summary>
    /// Fonction lanc�e par l'interraction du joueur
    /// </summary>
    public virtual void Interract() { }
}