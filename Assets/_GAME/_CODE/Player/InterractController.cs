using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterractController : MonoBehaviour
{
    private GameInput _inputInstance;
    private PlayerController _playerController;

    [SerializeField, Tooltip("Elements réagissants à l'interraction")]
    private LayerMask _interractLayer;

    [SerializeField, Tooltip("Distance du cercle d'interraction")]
    private float _interractRange = 1;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _inputInstance = new GameInput(); //_playerController.InputInstance;
    }

    private void OnEnable()
    {
        // Assignation des fonctions aux Inputs
        _inputInstance.Player.Enable();
        _inputInstance.Player.Interract.performed += Interract;
    }
    private void OnDisable()
    {
        // Désassignation des fonctions aux Inputs
        _inputInstance.Player.Interract.performed -= Interract;
    }

    /// <summary>
    /// Lance l'event d'interraction
    /// </summary>
    private void Interract(InputAction.CallbackContext context)
    {
            //Debug.Log("Interraction");
        Collider2D[] interractables = Physics2D.OverlapCircleAll(transform.position, _interractRange, _interractLayer);
        if(interractables.Length > 0)
        {
            if(interractables[0].TryGetComponent(out Interractable interractable))
            {
                interractable.Interract();
            }
        }
    }
}