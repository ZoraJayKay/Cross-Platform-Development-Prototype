using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    // An object to animate the character
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Set our Animator equal to the one attached to the object that calls this script
        animator = GetComponent<Animator>();
    }

    private void AnimateMovement()
    {
        // ANIMATION
        /* Get a reference to the Animator sibling component and feed it values for how much to go forwards, backwards and turn, which correspond to the following exposed parameters in the AnimationController:
         * "Forward" determines how fast the character should move ( 0 = idle, 0.5 = walk, 1 = run )
         * "Turn" determines how much to turn ( -1 = left,m 0 = straight ahead, +1 = right )
         * "Sense" determines whether the character is moving forward (+1) or backwards (-1)
        */

        // The new Unity Input System does not graduate the controller inputs between 0 and 1 using floating point numbers, it simply makes them an int of -1, 0 or 1, so I have to add in a smoothing effect for the animation transitions to work properly.
        //animationVector = Vector2.SmoothDamp(animationVector, 0, ref animationVelocity, animationSmoothTime);


        animator.SetFloat("Forward", Mathf.Abs(0));
        animator.SetFloat("Sense", Mathf.Sign(0));
        animator.SetFloat("Turn", 0);
    }

    // Update is called once per frame
    void Update()
    {
        AnimateMovement();
    }
}
