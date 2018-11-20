using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAi : MonoBehaviour
{
    public GameObject HearingSphere;
    public GameObject SightCone;

    private EnemyHearing _hearing;
    private EnemySights _sight;

    private NavMeshAgent _navAgent;
    private Animator _walkAnimation;

    private Vector3 _currentPosition;

    // Use this for initialization
    void Start ()
	{
	    _hearing = HearingSphere.GetComponent<EnemyHearing>();
	    _sight = SightCone.GetComponent<EnemySights>();

	    _navAgent = transform.GetComponentInParent<NavMeshAgent>();
	    _walkAnimation = transform.GetComponentInParent<Animator>();
	    _currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        _walkAnimation.SetBool("IsMoving", _navAgent.velocity != Vector3.zero);

        // Check hearing is enabled
        if (_hearing.HearingToggle)
        {
            // Move toward where we last heard the player
            if (_hearing.PlayerHeard && _currentPosition != _hearing.LastHeardPosition)
            {
                _navAgent.SetDestination(_hearing.LastHeardPosition);
            }
        }

        // Check visibility is enabled
        if (_sight.VisionToggle)
        {
            // Go to where we last saw the player
            _currentPosition = transform.position;
            if (_sight.PlayerInSight && _currentPosition != _sight.LastSeenPosition)
            {
                _navAgent.SetDestination(_sight.LastSeenPosition);
            }
        }
    }
}
