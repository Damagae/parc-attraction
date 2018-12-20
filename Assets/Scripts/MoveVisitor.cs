using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveVisitor : MonoBehaviour {

	private Transform goal;
	public QueuesManager qm;
	private Queue currentQueue;
	private NavMeshAgent agent;
	private int queueIndex;
	private int placeInQueueIndex;
	private Attraction attraction;
	private bool inQueue;
	private bool inAttraction;
	private bool lookingForQueue;
	private bool reachingAttraction;
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
		MoveToNextQueue();
	}

	// Update is called once per frame
	void Update () {

		// visitor in queue
		if (inQueue) {
			//Debug.Log(this.gameObject.name + " is in queue position " + placeInQueueIndex);
			if (placeInQueueIndex == 0 && attraction.CanStart()) { // If the visitor is in first place
				attraction.TakesPlace();
				Move(attraction.GetEntryLocation());
				inQueue = false;
				reachingAttraction = true;
				currentQueue.RemoveFirstPlace();
				placeInQueueIndex = -1;
				// Debug.Log(this.gameObject.name + " starts the attraction");
			} else { // If no
				int newPosition = currentQueue.GetNewPlaceIndex(placeInQueueIndex);
				// if (placeInQueueIndex != newPosition)
					// Debug.Log(this.gameObject.name + " has update, was "+ placeInQueueIndex +" now is " + newPosition);
				placeInQueueIndex = newPosition;
				Move(currentQueue.GetPlacePosition(placeInQueueIndex));
			}
		}

		// visitor in attraction
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
			if (attraction.IsOver()) {
				agent.enabled = true;
				// Debug.Log("C'est fini");
				inAttraction = false;
				inQueue = false;
				lookingForQueue = true;
				MoveToNextQueue();
			} else {
				this.gameObject.transform.position = attraction.GetVisitorPosition();
			}
		}

		// visitor looking for a queue
		else if (lookingForQueue) {
			// S'il a atteint sa destination
			 if (agent.enabled && !agent.pathPending) {
			     if (agent.remainingDistance <= agent.stoppingDistance) {
			         if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
								 	 placeInQueueIndex = currentQueue.GetLatestPlaceIndex();
									 attraction = currentQueue.GetAttraction();
			             currentQueue.TakesPlace();
									 inQueue = true;
									 // Debug.Log(this.gameObject.name + " enters queue position " + placeInQueueIndex);
			         }
			     }
				}
			// S'il n'a pas atteint sa destination
			 if (currentQueue.NextPlace().position != goal.position && !inQueue) {
						if (currentQueue.CanGo()) {
			 					goal = currentQueue.NextPlace();
			 					agent.destination = goal.position;
			 			} else {
			 					agent.destination = new Vector3(125.0f,15.87f,221.5f); // position random
			 			}
			 	}
		}

	}

	void NewQueue() {
		queueIndex = qm.GetRandomIndex();
		currentQueue = qm.GetQueue(queueIndex);
	}

	void UpdateCurrentQueue() {
		currentQueue = qm.GetQueue(queueIndex);
	}

	void MoveToNextQueue() {
		if (currentQueue.CanGo()) {
			Move(currentQueue.NextPlace());
		} else {
			NewQueue();
		}
	}

	void Move(Transform transform) {
		goal = transform;
		agent.destination = goal.position;
	}
}
