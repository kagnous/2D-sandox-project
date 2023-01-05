using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : Interractable
{
    [SerializeField]
    private Character _character; public Character Character { get { return _character; } }
    [SerializeField]
    private TextAsset inkJson;
    // Etat du dialogue, potentiellement modifié par des utilisation précédentes
    private string inkJsonState = ""; public string InkJsonState { get { return inkJsonState; } set { inkJsonState = value; } }

    private Animator animator;
    private SpriteRenderer sprite;

    private bool _isTalking = false; public bool IsTalking { get { return _isTalking; } set { _isTalking = value; } }

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        //player = FindObjectOfType<PlayerController>()?.transform;
        //sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Interract()
    {
        //if(!_isTalking)
        DialogueManager.instance.StartDialogue(inkJson, inkJsonState, this);

        //_isTalking = true;
    }

    private Transform player;
    /*private void Update()
    {
        animator.SetBool("IsTalking", _isTalking);
        if(player.position.x > transform.position.x)
        {
            sprite.flipX = true;
        }
        else
            sprite.flipX = false;
    }*/
}