using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySights : MonoBehaviour
{
    // Toggle vision on or off
    public bool VisionToggle;
    public GameObject RayCastOrigin;

    // Let's leave this public for debugging reasons!
    public Vector3 LastSeenPosition = Vector3.zero;
    public bool PlayerInSight = false;

    private Vector3 _rayCastOrigin;

	void Start ()
    {
        LastSeenPosition = Vector3.zero;
    }

    // The player has entered our sight cone
    private void OnTriggerStay(Collider other)
    {
        // If vision is switched off exit immediately
        if (!VisionToggle) return;

        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            _rayCastOrigin = RayCastOrigin.transform.position;
            RaycastHit hit;
            
            // Check if we have direct line of sight
            Debug.DrawRay(_rayCastOrigin, player.transform.position - _rayCastOrigin);
            if (Physics.Raycast(_rayCastOrigin, player.transform.position - _rayCastOrigin, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    PlayerInSight = true;
                    LastSeenPosition = player.transform.position;
                }
                else
                {
                    PlayerInSight = false;
                }
            }
            
        }
    }
}
