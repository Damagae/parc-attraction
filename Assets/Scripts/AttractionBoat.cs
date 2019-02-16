using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionBoat : Attraction {

	public GameObject boat;

	public override void StartAttraction(GameObject go) {
		isRunning = true;
		var visitor = go;
		visitors.Add(visitor);
		visitor.transform.position = boat.transform.position;
		startTimes.Add(Time.time);
	}

	public override Vector3 GetVisitorPosition() {
		Vector3 bp = boat.transform.position;
		return new Vector3(bp[0], bp[1] + 2, bp[2]);
	}

	// Update is called once per frame
	void Update () {

		if (isRunning) {
			for (int i = 0; i < visitors.Count; ++i) {
				if (HasDurationElapsed(i)) {
					Vector3 dest = new Vector3(6.77f, 100, 2.08f);
					boat.transform.position = dest;
					visitors[i].transform.position = end.position;
					visitors.Remove(visitors[i]);
					startTimes.Remove(startTimes[i]);
					isRunning = false;
					canStart = true;
					--takenPlaces;
				}
			}
		}

	}
}
