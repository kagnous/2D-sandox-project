using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    [SerializeField]
    private string charaName; public string Name { get { return charaName; } }

    [Header("Faces")]
    [SerializeField ] private Sprite normalFace; public Sprite NormalFace { get { return normalFace; } }
    [SerializeField] private Sprite angryFace; public Sprite AngryFace { get { return angryFace; } }
    [SerializeField] private Sprite sadFace; public Sprite SadFace { get { return sadFace; } }
    [SerializeField] private Sprite happyFace; public Sprite HappyFace { get { return happyFace; } }
}