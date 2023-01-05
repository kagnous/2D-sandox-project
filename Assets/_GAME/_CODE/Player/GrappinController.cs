using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GrappinController : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField]
    private float _grappinRange = 10;
    private LineRenderer _lineRenderer;
    private DistanceJoint2D _distanceJoin;
    private GameInput _inputsInstance = null;

    // Surface d'accroche autorisée
    public LayerMask layerMask;

    private bool grab = false;

    private void Awake()
    {
        _inputsInstance = new GameInput();
    }

    private void OnEnable()
    {
        // Assignation des fonctions aux Inputs
        _inputsInstance.Player.Enable();
        _inputsInstance.Player.Grappin.performed += Grappin;
    }

    void Start()
    {
        _distanceJoin= GetComponent<DistanceJoint2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _playerController = GetComponent<PlayerController>();
        _distanceJoin.enabled = false;
    }

    private void Grappin(InputAction.CallbackContext context)
    {
        grab = !grab;
        if(grab)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, _playerController.DirectionMovment, _grappinRange, layerMask);
            
            if(raycast.collider != null)
            {
                Vector2 grabPoint = raycast.point;
                _distanceJoin.connectedAnchor = grabPoint;
                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(0, grabPoint);
                _lineRenderer.SetPosition(1, transform.position);
                _distanceJoin.enabled = true;
                _playerController.Grap = true;
            }
            else
            {
                grab = false;
            }
        }
        else
        {
            _playerController.Grap = false;
            _distanceJoin.enabled = false;
            _lineRenderer.enabled = false;
        }

    }

    private void Update()
    {
        if(grab)
        {
            _lineRenderer.SetPosition(1, transform.position);
            if(_playerController.IsGrounded)
            {
                Grappin(new InputAction.CallbackContext());
            }
        }
    }

}
