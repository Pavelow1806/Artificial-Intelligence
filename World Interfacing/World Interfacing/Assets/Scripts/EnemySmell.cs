using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySmell : MonoBehaviour {

    public GameObject player;
    private ParticleSystem part;
    public NavMeshAgent NMA;

    // Use this for initialization
    void Start ()
    {
        part = player.GetComponent<ParticleSystem>();
	}

    private void OnParticleTrigger()
    {
        Debug.Log("Collision with particle!");
        NMA.SetDestination(player.transform.position);
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision with particle!");
        if (other.gameObject.GetComponentInParent<Transform>().name == "FPSController")
        {
            Debug.Log("I can smell you, filthy hobitses!");
            NMA.SetDestination(other.gameObject.GetComponentInParent<Transform>().position);
        }
    }
}
