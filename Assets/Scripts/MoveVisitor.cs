using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveVisitor : MonoBehaviour {

	private Vector3 goal;
	public QueuesManager qm;
	private Queue currentQueue;
	private NavMeshAgent agent;
	private int queueIndex;
	private int placeInQueueIndex;
	private Attraction attraction;
	private bool inQueue; // the visitor is in a queue
	private bool inAttraction; // the visitor is in a running attraction
	private bool lookingForQueue; // the visitor is looking for a new queue
	private bool reachingAttraction; // the visitor was first in the queue and is now going in the attraction
	private float timeStartLooking;
	private float maximumLookingTime = 25f;
	private GameObject visitor;

	// Use this for initialization
	void Start () {
		inQueue = false;
		reachingAttraction = false;
		inAttraction = false;
		lookingForQueue = true;
		agent = GetComponent<NavMeshAgent>();
		queueIndex = qm.GetRandomIndex();
		placeInQueueIndex = -1;
		currentQueue = qm.GetQueue(queueIndex);
		if (AgentReady())
			MoveToNextQueue();
	}

	// Update is called once per frame
	void Update () {

		// visitor in queue
		if (inQueue) {
			if (placeInQueueIndex == 0 && attraction.CanStart()) { // If the visitor is in first place
				attraction.TakesPlace();
				Move(attraction.GetEntryLocation());
				inQueue = false;
				reachingAttraction = true;
				currentQueue.RemoveFirstPlace();
				placeInQueueIndex = -1;
			} else { // If no
				int newPosition = currentQueue.GetNewPlaceIndex(placeInQueueIndex);
				placeInQueueIndex = newPosition;
				Move(currentQueue.GetPlacePosition(placeInQueueIndex));
			}
		}

		// visitor reaching attraction
		else if (reachingAttraction) {
			 if (agent.remainingDistance <= agent.stoppingDistance) {
				 reachingAttraction = false;
				 inAttraction = true;
				 agent.enabled = false;
				 attraction.StartAttraction(this.gameObject);
			 }
		}

		// visitor in attraction
		else if (inAttraction) {
			if (attraction.IsOver() || attraction.CanVisitorGo(this.gameObject)) {
				agent.enabled = true;
				inAttraction = false;
				inQueue = false;
				lookingForQueue = true;
				currentQueue = null;
				MoveToNextQueue();
			} else {
				this.gameObject.transform.position = attraction.GetVisitorPosition();
			}
		}

		// visitor looking for a queue
		else if (lookingForQueue) {
			// S'il a atteint sa destination
			if (agent.enabled && !agent.pathPending) {
			     if (agent.isOnNavMesh && /*agent.remainingDistance <= agent.stoppingDistance &&*/ IsCloseEnough()) {
			         //if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
								 	 placeInQueueIndex = currentQueue.GetLatestPlaceIndex();
									 attraction = currentQueue.GetAttraction();
			             currentQueue.TakesPlace();
									 inQueue = true;
			         //}
			     }
				}
			// S'il n'a pas atteint sa destination
			if (agent.isOnNavMesh && currentQueue.NextPlace() != goal && !inQueue) {

					 if (currentQueue.CanGo()) {
							 goal = currentQueue.NextPlace();
							 agent.destination = goal;
					 } else if (Time.time - timeStartLooking > maximumLookingTime) { // S'il cherche depuis trop longtemps (il est potentiellement coincé)
						 	MoveToNextQueue();
					 } else {
						 	NewQueue();
					 }
			}
			// vérifie que l'agent est sur un navmesh et le détruit sinon
			AgentReady();

		}


	}

	private void ResetTimeLooking() {
		timeStartLooking = Time.time;
	}

	private bool AgentReady() {
		if (agent.isOnNavMesh) {
			return true;
		} else {
			Destroy(this.gameObject, 1);
		}
		return false;
	}

	private bool IsCloseEnough() {
		float epsilon = 1f;
		var distance = Vector3.Distance(
				new Vector3(agent.nextPosition.x, 0, agent.nextPosition.z),
				new Vector3(goal.x, 0, goal.z)
				);
		return (distance < epsilon);
	}

	void NewQueue() {
		int newQueueIndex = 0;
		while (newQueueIndex == queueIndex) {
			newQueueIndex = qm.GetRandomIndex();
		}
		queueIndex = newQueueIndex;
		currentQueue = qm.GetQueue(queueIndex);
		ResetTimeLooking();
	}

	void UpdateCurrentQueue() {
		currentQueue = qm.GetQueue(queueIndex);
	}

	void MoveToNextQueue() {
		if (currentQueue && currentQueue.CanGo()) {
			Move(currentQueue.NextPlace());
		} else {
			NewQueue();
		}
	}

	void Move(Vector3 goal) {
		agent.destination = goal;
	}
}
