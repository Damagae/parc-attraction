using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueV2 : MonoBehaviour {

	public Vector3 orientation;
	public int latestPlaceIndex;
	public Attraction attraction;
	public Transform start;
	public float spaceBetween;
	private bool[] placesTaken;

	// Use this for initialization
	void Start () {
		// latestPlaceIndex = 0;
		placesTaken = new bool[50];
		for (int i = 0; i < placesTaken.Length; ++i) {
			placesTaken[i] = false;
		}
	}

	public bool CanGo() {
		return true;
	}

	public Vector3 NextPlace() {
		return GetPlacePosition(latestPosition);
	}

	public Vector3 GetPlacePosition(int index) {

		return attraction.transform; // WRONG
	}

	public void TakesPlace() {
		// placesTaken[latestPlaceIndex] = true;
		// if (latestPlaceIndex < placesPos.Length - 1)
		// 	++latestPlaceIndex;
	}

	public Attraction GetAttraction() {
		return attraction;
	}

	public void RemoveFirstPlace() {
		placesTaken[0] = false;
		--latestPlaceIndex;
	}

	private bool IsNextPlaceTaken(int index) {
		if (index > 0)
			return placesTaken[index-1];
		else
			return true;
	}

	public int GetNewPlaceIndex(int index) {
		// If next place is not taken
		if (!IsNextPlaceTaken(index)) {
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
