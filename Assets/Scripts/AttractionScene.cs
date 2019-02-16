using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionScene : Attraction {

	public Transform[] places;

	public override void StartAttraction(GameObject go) {
		isRunning = true;
		var visitor = go;
		visitors.Add(visitor);
		visitor.transform.position = GetVisitorPosition();
		startTimes.Add(Time.time);
	}

	public Vector3 GetVisitorPosition(int index) {
		return places[index].position;
	}
}
