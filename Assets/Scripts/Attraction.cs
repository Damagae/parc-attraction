using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour {

	public Transform entryLocation;
	private bool canStart;
	public bool isRunning;
	private GameObject visitor;
	public GameObject boat;
	public float duration;
	private float startTime;
	public Transform end;

	// Use this for initialization
	void Start () {
		canStart = true;
	}

	public Transform GetEntryLocation() {
		return entryLocation;
	}

	public void StartAttraction(GameObject go) {
		canStart = false;
		isRunning = true;
		visitor = go;
		visitor.transform.position = boat.transform.position;
		startTime = Time.time;
	}

	public Vector3 GetVisitorPosition() {
		Vector3 bp = boat.transform.position;
		return new Vector3(bp[0], bp[1] + 2, bp[2]);
	}

	public bool CanStart() {
		return canStart;
	}

	public void TakesPlace() {
		canStart = false;
	}

	private bool HasDurationElapsed() {
		float current = Time.time;
		return (current - startTime > duration);
	}

	public bool IsOver() {
		return !isRunning;
	}

	// Update is called once per frame
	void Update () {

		if (isRunning) {
			if (HasDurationElapsed()) {
				Vector3 dest = new Vector3(6.77f, 100, 2.08f);
				boat.transform.position = dest;
				visitor.transform.position = end.position;
				isRunning = false;
				canStart = true;

			}
		}

	}
}
