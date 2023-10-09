using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OpenByProximity : MonoBehaviour
{
    public GameObject player;
    Vector3 playerPos;
    public GameObject[] objectsToTurnOn;
    Vector3 uiPos;
    public int detectionRange;
    bool awakened = false;

    private void Start()
    {
        awakened = false;
    }

    private void Update()
    {
        if (awakened == true)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < detectionRange)
            {
                print("Player has entered detection range");
                foreach (GameObject obj in objectsToTurnOn)
                {
                    obj.SetActive(true);
                }
            }

            else
            {
                foreach (GameObject obj in objectsToTurnOn)
                {
                    obj.SetActive(false);
                }
            }
        }        
    }

    public void WakeUp()
    {
        awakened = true;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(this.transform.position, detectionRange);
    //}
}
