using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Système dédié au déplacements du Player
/// </summary>
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    public float speed = 175;
    float jumpForce = 300;

    private Rigidbody2D rb;

    private Vector3 velocity = Vector3.zero;

    private GameInput _inputsInstance = null; public GameInput InputInstance { get { return _inputsInstance; } }

    #region GroundCheck
    /// <summary>
    /// Booléen de contact avec le sol (true si au sol, false si en l'air)
    /// </summary>
    private bool isGrounded; public bool IsGrounded => isGrounded;

    [SerializeField, Tooltip("Point de référence pour la vérification du contact au sol")]
    private Transform groundCheck;

    [SerializeField, Tooltip("Rayon de la sphère de vérification du contact au sol")]
    private float groundCheckRadius;

    [SerializeField, Tooltip("Layer détectés pour la vérification du contact au sol")]
    private LayerMask collisionLayer;
    #endregion

    private bool grap = false; public bool Grap { get { return grap; } set { grap = value; } }

    private bool isClimb = false; public bool IsClimb { get { return isClimb; } set { isClimb = value; } }

    /// <summary>
    /// Vecteur de la direction du player 
    /// </summary>
    private Vector2 _directionMovment; public Vector2 DirectionMovment { get { return _directionMovment; } set { _directionMovment = value; } }

    private void Awake()
    {
        _inputsInstance = new GameInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Assignation des fonctions aux Inputs
        _inputsInstance.Player.Enable();
        _inputsInstance.Player.Move.performed += Move;
        _inputsInstance.Player.Jump.performed += Jump;
    }
    private void OnDisable()
    {
        // Désassignation des fonctions aux Inputs
        _inputsInstance.Player.Move.performed -= Move;
        _inputsInstance.Player.Jump.performed -= Jump;
    }

    private void FixedUpdate()
    {
        #region Move

        if (!grap)
        {
            if (!isClimb)
            {
                // Calcul de la velocité en fonction du mouvement player sur x, la speed et le temps
                Vector3 targetVelocity = new Vector2(_directionMovment.x * speed * Time.deltaTime, rb.velocity.y);
                // Mouvement en fonction de la velocité calculée ci-desssus
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
            }
            else
            {
                // Alternative avec la montée en y
                Vector3 targetVelocity = new Vector2(_directionMovment.x * speed * Time.deltaTime, _directionMovment.y * speed * Time.deltaTime);
                // Mouvement en fonction de la velocité calculée ci-desssus
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
            }
        }
        // Contôle spécial du grappin
        else if (_directionMovment != Vector2.zero)
        {
            // Calcul de la velocité en fonction du mouvement player sur x, la speed et le temps
            Vector3 targetVelocity = new Vector2(_directionMovment.x * speed * Time.deltaTime, rb.velocity.y);
            // Mouvement en fonction de la velocité calculée ci-desssus
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
        }
        #endregion

        //Flip le sprite selon la direction où elle va
        Flip(-rb.velocity.x);

        TestIsGrounded();
    }

    /// <summary>
    /// Settings de _directionMovment selon les Input du player
    /// </summary>
    public void Move(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        _directionMovment = context.ReadValue<Vector2>();
        //Debug.Log("Move\n"+ context.ReadValue<Vector2>());
    }

    /// <summary>
    /// Application du saut à l'Input du player
    /// </summary>
    private void Jump(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        // On vérifie si le player peut sauter
        TestIsGrounded();
        // Si oui on applique une force vers le haut en fonction de la JumpForce du player
        if (isGrounded) rb.AddForce(new Vector2(0f, jumpForce));

        //Debug.Log("Jump");
    }

    /// <summary>
    /// Scale le player sur x entre positif et négatif pour le tourner selon son mouvement
    /// </summary>
    /// <param name="_velocity">Mouvement sur l'axe x</param>
    private void Flip(float _velocity)
    {
        // Si son mouvement est positif...
        if (_velocity > 0.1f)
        {
            // Son scale sur x vaut -1
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        // Sinon s'il est négatif...
        else if (_velocity < -0.1f)
        {
            // Son scale sur x vaut 1
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// Vérifie si le player à un contact avec le sol
    /// </summary>
    private void TestIsGrounded()
    {
        // On teste si le player est actuellement en contact avec le sol (du moins une collision de collisionLayer) dans le rayon de groundCheck
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);
    }

    public void StopMoveInput()
    {
        _inputsInstance.Player.Disable();
        // Désassignation des fonctions aux Inputs
        //_inputsInstance.Player.Move.performed -= Move;
        //_inputsInstance.Player.Jump.performed -= Jump;

        // On arrête le joueur (si la méthode est lancée en plein déplacement par exemple)
        _directionMovment = Vector2.zero;
    }

    public void ResumeMoveInput()
    {
        _inputsInstance.Player.Enable();
        // Réassignation des fonctions aux Inputs
        //_inputsInstance.Player.Move.performed += Move;
        //_inputsInstance.Player.Jump.performed += Jump;
    }

    // Pour les gizmos (zone de groundCheck)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}