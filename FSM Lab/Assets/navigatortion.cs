using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navigatortion : MonoBehaviour {

    public NavMeshAgent NM;
    public GameObject Target;

	// Use this for initialization
	void Start () {
        NM.SetDestination(Target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
