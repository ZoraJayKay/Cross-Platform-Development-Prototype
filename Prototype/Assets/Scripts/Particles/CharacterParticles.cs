using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Enum for choosing a body part on which to apply a visual effect (lazy initialisation outside the class)
public enum BodyPart
{
    Root,
    Head,
    Chest,
    LeftHand,
    RightHand,
    LeftFoot,
    RightFoot
}

public class CharacterParticles : MonoBehaviour
{
    // Variables for foot-based particle animations
        // 1: ^^^^^^^^^^ With a user-defined Particle system ^^^^^^^^^^
        public VisualFX metalFootstepsFX;
        public VisualFX footstepsFX;
        public VisualFX foyerFootStepsFX;

        [Header("Body parts")]
        public Transform leftFoot;
        public Transform rightFoot;
        public Transform leftHand;
        public Transform rightHand;
        public Transform head;
        public Transform chest;

        public PhysicMaterial metalMaterial;
        public PhysicMaterial foyerMaterial;

    // 2: &&&&& With a GameObject &&&&& 
    //public GameObject particles;

    // Add a private Dictionary called 'parts' for quick access to transforms
    Dictionary<BodyPart, Transform> parts;

    // A function for returning the transform of an arbitrary body part
    public Transform GetBodyPart(BodyPart part)
    {
        // lazy initialisation wherein we... 
        if (parts == null)
        {
            // Instantiate and fill the Dictionary the first time we need it...
            parts = new Dictionary<BodyPart, Transform>();
            parts[BodyPart.Root] = transform;
            parts[BodyPart.Head] = head;
            parts[BodyPart.RightHand] = rightHand;
            parts[BodyPart.RightHand] = leftHand;
            parts[BodyPart.RightHand] = rightFoot;
            parts[BodyPart.RightHand] = leftFoot;
        }

        // Perform a lookup using the dictionary
        if (parts.ContainsKey(part))
        {
            // Return the found part
            return parts[part];
        }

        // Otherwise return our transform
        return transform;
    }


    public void Step(int footIndex)
    {
        // 1: Visual effects particle instantiation
        // Find the correct foot to spawn particles at. If the footIndex is 1, then it's leftFoot, else it's rightFoot
        Transform foot = footIndex == 1 ? leftFoot : rightFoot;

        // 2: Determine the material on which the character is standing
        PhysicMaterial ground = null;
            
            // A raycast for monitoring what's underfoot
            RaycastHit hit;

            // Record the position of our transform to our temporary raycast, and if there is anything there, then...
            if (Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit))
            {
                // ... record the PhysicMaterial we collided with... 
                // collider.material WOULD NOT WORK because that is a private copy of the metal asset, and a comparison would not work. sharedMaterial, with a public member variable 'metal' is required, since it points to the original physics material in the assets folder.
                ground = hit.collider.sharedMaterial;
                if (ground)
                {
                    Debug.Log("Shared material found: " + ground.name);
                }
            }

        VisualFX tempFX;

            if (ground == metalMaterial) 
            {
            tempFX = metalFootstepsFX;
            }

            else if (ground == foyerMaterial)
            {
            tempFX = foyerFootStepsFX;
            }
            else { tempFX = footstepsFX;}

            // If the PhysicMaterial we collided with is metalFootstepsFX, then return that particle effect, otherwise return our default footSteps effect
            //VisualFX fx = ground == metalMaterial ? metalFootstepsFX : footstepsFX;


        // 3: Instantiate a copy of the particle with no rotation at the foot's position. It is NOT a child of the foot.
            // %%%%%%%%%%%% With a user-defined Particle system and a PhysicMaterial conditional visual effect %%%%%%%%%%%%
            GameObject go = tempFX.Spawn(foot);
                    
            // &&&&&&&&&&&& With a GameObject &&&&&&&&&&&&
            //GameObject go = Instantiate(particles, foot.position, Quaternion.identity);

            // ^^^^^^^^^^ With a user-defined Particle system and a defined visual effect ^^^^^^^^^^
            //GameObject go = footstepsFX.Spawn(foot);

            // Remove the particle system after a few seconds, after all particles have died away
            Destroy(go, 3);
    }

    public void Spell()
    {

    }

    public void Spell2()
    {

    }
}
