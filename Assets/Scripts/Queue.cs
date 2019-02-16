using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour {

	public Vector3 orientation = new Vector3(-1.0f, 0.0f, 0.1f);
	public Attraction attraction;
	public Transform start;
	public float spaceBetween = 2;
	private bool[] placesTaken;
	private int latestPlaceIndex = 0;

	// Use this for initialization
	void Start () {
		placesTaken = new bool[100];
		for (int i = 0; i < placesTaken.Length; ++i) {
			placesTaken[i] = false;
		}
	}

	// Return true if a new visitor can go to the queue (if places are available)
	public bool CanGo() {
		if (latestPlaceIndex == 99) {
			return false;
		}
		if (latestPlaceIndex > 30) {
			if (Random.Range(1,4) == 4)
				return true;
			return false;
		}
		return true; // always return true in this version (no limit)
	}

	public int GetLatestPlaceIndex() {
		return latestPlaceIndex;
	}

	// Return the position for the visitor to head to
	public Vector3 NextPlace() {
		return GetPlacePosition(latestPlaceIndex);
	}

	// Retrun a vector3 from an index
	public Vector3 GetPlacePosition(int index) {
		return start.position + orientation * index * spaceBetween;
	}

	public void TakesPlace() {
		++latestPlaceIndex;
		placesTaken[latestPlaceIndex - 1] = true;
	}

	public Attraction GetAttraction() {
		return attraction;
	}

	public void RemoveFirstPlace() {
		placesTaken[0] = false;
		--latestPlaceIndex;
	}

	private bool IsNextPlaceTaken(int index) {
		if (index > 0) {
			return placesTaken[index-1];
		}
		return true;
	}

	public int GetNewPlaceIndex(int index) {
		// If next place is not taken
		if (!IsNextPlaceTaken(index) && index > 0) {
			placesTaken[index] = false;
			placesTaken[index-1] = true;
			return index-1; // change
		}
		return index; // No change
	}

	// Update is called once per frame
	void Update () {

	}
}
