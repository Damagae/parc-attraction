using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveWanderingVisitor : MonoBehaviour {

	private NavMeshAgent agent;
	private float referenceSpeed;
	private float mapWidth = 250;
	private float mapLength = 300;
	private Vector3 destination;
	private int waterMask;

	// Use this for initialization
	void Start () {
		waterMask = 1 << NavMesh.GetAreaFromName("Slow");
		agent = GetComponent<NavMeshAgent>();
		referenceSpeed = agent.speed;
		destination = GetRandomDestination();
		Move(destination);
	}

	void Move(Vector3 goal) {
		agent.destination = goal;
	}

	Vector3 GetRandomDestination() {
		return new Vector3 (Random.Range(0.0f, mapWidth), 0, Random.Range(0.0f, mapLength));
	}

	// Update is called once per frame
	void Update () {
		if (agent.enabled && !agent.pathPending) {
				if (agent.isOnNavMesh && agent.remainingDistance <= agent.stoppingDistance) {
						if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
								destination = GetRandomDestination();
								Move(destination);
						}
				}
		 }

		NavMeshHit hit;
    // Check all areas one length unit ahead.
    if (!agent.SamplePathPosition(NavMesh.AllAreas, 1.0F, out hit)) {
			// if hit.mask is water Mask
			if ((hit.mask & waterMask) != 0)
			{
				// Water detected along the path...
				agent.speed = referenceSpeed / 3;
			} else {
				agent.speed = referenceSpeed;
			}
		}

	}
}
