using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyHearing : MonoBehaviour
{
    public bool HearingToggle;

    public float HearingThreshold;
    public Vector3 LastHeardPosition;
    public float Noise;
    public bool PlayerHeard = false;

    // Player has entered our hearing range
    private void OnTriggerStay(Collider other)
    {
        // If vision is switched off exit immediately
        if (!HearingToggle) return;

        if (other.CompareTag("Player"))
        {
            // Check if the player is making enough noise to be heard at the players distance
            Noise = other.GetComponent<FirstPersonController>().GetNoise();
            Noise /= Vector3.Distance(transform.position, other.transform.position);

            if(Noise > HearingThreshold)
            {
                LastHeardPosition = other.transform.position;
                PlayerHeard = true;
            }
            else
            {
                PlayerHeard = false;
            }
        }
    }
}
