using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI; // For NavMesh variables and functions
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

// This script reads input and animates the character
public class PlayerController : MonoBehaviour
{
    // An input map for cross-platform user inputs
    private InputActions _input;

    // A coroutine for moving the player according to mouse clicks or touchscreen presses
    private Coroutine _coroutine;

    // Layers (for use in detecting what the user has clicked on-screen)
    private int groundLayer;
    private int pickupLayer;
    private int laboratoryLayer;
    
    // For autonomous 3D navigation with clicks
    private NavMeshAgent playerAgent;
    private bool agentStopped;

    // Indicator whether the user needs to turn left or right
    private float leftRightNum;
    // A factor to slow down the speed at which the user rotates when moving autonomously with Unity's NavMesh system
    private float rotationSpeed = 0.4f;

    // An object to animate the character
    Animator animator;

    // Variables for smoothing the raw integer that comes out of Unity's new movement system
    private float animationSmoothTime = 0.3f;
    private Vector2 animationVector;
    private Vector2 animationVelocity;

    private Vector3 mousePos;
      


    private void Awake()
    {
        // Create a new instance of the input action control set inside this class
        _input = new InputActions();
        
        // Cache the integer number that represents our ground layer, for CompareTo() purposes
        groundLayer = LayerMask.NameToLayer("Ground");
        pickupLayer = LayerMask.NameToLayer("Pickup");
        laboratoryLayer = LayerMask.NameToLayer("Laboratory");
    }

    private void OnEnable()
    {
        // Enable the input action maps for the input methods that are both polled and those that are subscribed to events
        Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe the actions from the events
        _input.Player.MousePosition.performed -= StoreMousePosition;
        _input.Player.Interaction.performed -= DoubleClick;

        // Turn off the actions
        _input.Player.Walk.Disable();
        _input.Player.MousePosition.Disable();
        _input.Player.Interaction.Disable();
    }
       
    public void Enable()
    {
        // For movement with keyboard
        _input.Player.Walk.Enable();
        // For getting the coordinates on the screen where the mouse is
        _input.Player.MousePosition.Enable();
        // For double clicking the world and objects
        _input.Player.Interaction.Enable();

        _input.Player.MousePosition.performed += StoreMousePosition;
        _input.Player.Interaction.performed += DoubleClick;
    }

    void Start()
    {
        // Set our Animator equal to the one attached to the object that calls this script
        animator = GetComponent<Animator>();

        // Cache a reference to the player model's navmesh agent
        playerAgent = GetComponent<NavMeshAgent>();
        playerAgent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        agentStopped = playerAgent.isStopped;

        Vector2 _move = new Vector2 (leftRightNum, 1);

        // If the player is currently being controlled by the mouse and keyboard...
        if (agentStopped)
        {
            // Animate the player character in response to user inputs
            AnimateMovement(MovementInput());
        }

        // Else if the player is currently being controlled by mouseclick or touchscreen
        else
        {
            AnimateMovement(_move);
        }
    }

    private void DoubleClick(InputAction.CallbackContext context)
    {
        // Make a raycast to the in-world mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        
        // if we send out a ray and it a) hits a collider and b) the collider is on either the layer that we've assigned as clickable, or a pickup...
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) &&
        hit.collider )
        {
            // If there's already a coroutine running (because the player has clicked again before the last movement has finished)...
            if (_coroutine != null)
            {
                // Stop the previous movement order
                StopCoroutine(_coroutine);
            }

            // Once we know we've hit a collider (and not empty space), then if we hit somewhere we actually want the user to be able to go...
            if (ThisRayHitLayer(hit, groundLayer) == true || ThisRayHitLayer(hit, pickupLayer) == true || ThisRayHitLayer(hit, laboratoryLayer))
            {
                playerAgent.isStopped = false;
                // Run the coroutine to move the player model towards the point on the screen that was clicked
                _coroutine = StartCoroutine(PlayerMoveTowards(hit));

                mousePos = hit.point;
            }               
        }
    }

    private void AnimateMovement(Vector2 input)
    {
        // ANIMATION
        /* Get a reference to the Animator sibling component and feed it values for how much to go forwards, backwards and turn, which correspond to the following exposed parameters in the AnimationController:
         * "Forward" determines how fast the character should move ( 0 = idle, 0.5 = walk, 1 = run )
         * "Turn" determines how much to turn ( -1 = left,m 0 = straight ahead, +1 = right )
         * "Sense" determines whether the character is moving forward (+1) or backwards (-1)
        */

        // The new Unity Input System does not graduate the controller inputs between 0 and 1 using floating point numbers, it simply makes them an int of -1, 0 or 1, so I have to add in a smoothing effect for the animation transitions to work properly.
        animationVector = Vector2.SmoothDamp(animationVector, input, ref animationVelocity, animationSmoothTime);

        
        animator.SetFloat("Forward", Mathf.Abs(animationVector.y));
        animator.SetFloat("Sense", Mathf.Sign(animationVector.y));
        animator.SetFloat("Turn", animationVector.x);
    }

    public Vector2 MovementInput()
    {
        return _input.Player.Walk.ReadValue<Vector2>();
    }

    public Vector2 MousePosition2D()
    {
        return _input.Player.MousePosition.ReadValue<Vector2>();
    }

    private bool ThisRayHitLayer(RaycastHit r, int layerLevel)
    {
        if (r.collider.gameObject.layer.CompareTo(layerLevel) == 0)
        {
            return true;
        }
        else { return false; }
    }

    private IEnumerator PlayerMoveTowards(RaycastHit ray)
    {
        // By default, assume that the user clicked on the ground
        bool isPickup = false;
        bool isInventory = false;

        // Check whether or not the thing the user clicked was a pickup or not
        if (ray.collider.gameObject.GetComponent<PickUp>() != null)
        {
            isPickup = true;
            isInventory = false;
        }

        else if (ThisRayHitLayer(ray, laboratoryLayer))
        {
            print("inventory found");
            isInventory = true;
            isPickup = false;
        }

        else { isInventory = false; isPickup = false; }

        // Get the 3D location that the user clicked on
        Vector3 target = ray.point;

        // While the distance between the player and its target is greater than some trivial amount...
        while (Vector3.Distance(transform.position, target) > 1)
        {
            // Calculate the direction we're going to rotate towards
            Vector3 direction = target - transform.position;

            UpdateRotationTowards(target, direction);
            playerAgent.destination = target;
            
            // Slow down the rate of rotation so the movement on click looks more like the movement from keyboard
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed * Time.deltaTime);

            yield return null;
        }

        // Once the path has been reached, clear the NavMeshAgent's path so it doesn't keep trying to navigate back here if the player uses manual controls
        playerAgent.ResetPath();
        // Let the Update function know that user-based movement has ended
        playerAgent.isStopped = true;

        // If the user clicked a pickup, collect it once we've stopped moving
        if (isPickup)
        {
            PickUp pickup = ray.collider.gameObject.GetComponent<PickUp>();
            pickup.AddToInventory();
        }

        else if (isInventory == true)
        {
            print("Inventory detected on double mouse click");


            //GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
            //GameObject inv = GameObject.FindGameObjectWithTag("LaboratoryInventoryUI");
            //GameManager gm = gameManager.GetComponent<GameManager>();
            //gm.TurnOn(inv);

            //inv.SetActive(true);
        }

        yield return null;
    }

    // A function to calculate whether the target is on the left or the right of the player
    private void UpdateRotationTowards(Vector3 target, Vector3 direction)
    {
        Vector3 cross;
        
        // I need to calculate this for the Update function, because the animator needs this information to properly convert click data into local geographical information
        cross = Vector3.Cross(transform.forward, direction).normalized;
        leftRightNum = Vector3.Dot(cross, Vector3.up);

        if (leftRightNum > 0f)
        {
            leftRightNum = 1f;
        }
        else if (leftRightNum < 0f)
        {
            leftRightNum = -1f;
        }
        else
        {
            leftRightNum = 0f;
        }
    }

    // Draw a debug circle on the screen where we mouse-click
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(mousePos, 0.25f);
    }
        

    private void StoreMousePosition(InputAction.CallbackContext context)
    {
        // Record where the mouse is on the screen
        mousePos = MousePosition2D();
    }
}