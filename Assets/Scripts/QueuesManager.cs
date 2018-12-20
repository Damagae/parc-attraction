using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuesManager : MonoBehaviour {

	public Queue[] queuesList;

	// Use this for initialization
	void Start () {

	}

	public int GetRandomIndex() {
		return Random.Range(0, queuesList.Length);
	}

	public Queue GetQueue(int index) {
		return queuesList[index];
	}

	public void AddToQueue(int index) {
		queuesList[index].TakesPlace();
	}

	// Update is called once per frame
	void Update () {

	}
}
