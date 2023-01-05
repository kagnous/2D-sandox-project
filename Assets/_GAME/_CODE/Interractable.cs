using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Interractable : MonoBehaviour
{
    private const int INTERRACTABLE_INT_VALUE = 6;

    /// <summary>
    /// Met le layer interractif si jamais l'élément est interractible
    /// </summary>
    private void Awake()
    {
        if(gameObject.layer != INTERRACTABLE_INT_VALUE)
        {
            Debug.LogWarning(gameObject.name + " n'a pas le layer \"Interractable\" mais à un script qui hérite de \"Interractable\"");
            gameObject.layer = INTERRACTABLE_INT_VALUE;
        }
    }

    /// <summary>
    /// Fonction lancée par l'interraction du joueur
    /// </summary>
    public virtual void Interract() { }
}