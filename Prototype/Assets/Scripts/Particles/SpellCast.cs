using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCast : MonoBehaviour
{
    [Tooltip("Key to press for this spell")]
    public KeyCode key;
    [Tooltip("Name of trigger in Animation Controller")]
    public string animationTrigger;

    [Tooltip("castFX goes off on castPart as soon as the key is pressed")]
    public VisualFX castFX;
    public BodyPart castPart;

    public VisualFX spellFX;
    public BodyPart spellPart;

    public Animator animator;
    public CharacterParticles particles;

    //private bool active = false;

    // Update is called once per frame
    private void Update()
    {
        {
            if (Input.GetKeyDown(key))
            {
                // Fire the animation, mark this object as an active spell
                animator.SetTrigger(animationTrigger);
                //active = true;

                // Do the cast effect immediately
                if (castFX != null) 
                {
                    GameObject go = castFX.Spawn(particles.GetBodyPart(castPart));

                    Destroy(go, 3);
                }
            }
        }
    }
}
