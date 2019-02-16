using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour {

	public Transform entryLocation;
	public int capacity = 1;
	public float duration;
	public Transform end;
	public Transform disparationLocation;
	protected int takenPlaces;
	protected bool canStart;
	protected bool isRunning;
	protected List<GameObject> visitors;
	protected List<float> startTimes;
	protected float timeBetweenEntries = 1;

	// Use this for initialization
	void Start () {
		visitors = new List<GameObject>();
		startTimes = new List<float>();
		takenPlaces = 0;
		canStart = true;
	}

	public Vector3 GetEntryLocation() {
		return entryLocation.position;
	}

	public virtual Vector3 GetVisitorPosition() {
		return disparationLocation ? disparationLocation.position : new Vector3(0,-100,0);
	}

	private bool CanGetIn() {
		float current = Time.time;
		if (startTimes.Count < 1) {
			return true;
		}
		return (current - startTimes[startTimes.Count - 1] > timeBetweenEntries);
	}

	public virtual void StartAttraction(GameObject go) {
		isRunning = true;
		var visitor = go;
		visitors.Add(visitor);
		visitor.transform.position = GetVisitorPosition();
		startTimes.Add(Time.time);
	}

	public bool CanVisitorGo(GameObject visitor) {
		return !visitors.Exists(x => x == visitor);
	}

	public bool CanStart() {
		if (takenPlaces >= capacity || !CanGetIn()) {
			return false;
		} else {
			return true;
		}
	}

	public void TakesPlace() {
		++takenPlaces;
		canStart = true;
		if (takenPlaces >= capacity) {
			canStart = false;
		}
	}

	protected bool HasDurationElapsed(int index) {
		float current = Time.time;
		return (current - startTimes[index] > duration);
	}

	public bool IsOver() {
		return !isRunning;
	}

	// Update is called once per frame
	void Update () {

		if (isRunning) {
			for (int i = 0; i < visitors.Count; ++i) {
				if (HasDurationElapsed(i)) {
					visitors[i].transform.position = end.position;
					visitors.Remove(visitors[i]);
					startTimes.Remove(startTimes[i]);
					--takenPlaces;
					if (takenPlaces <= 0) {
						isRunning = false;
					}
				}
			}
		} else {

		}

	}
}
