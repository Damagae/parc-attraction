using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {

	public int visitors;
	public int wanderingVistors;
	public GameObject visitorGO;
	public GameObject wanderingVisitorGO;
	public Transform visitorsSpawn;
	public Transform wanderingVistorsSpawn;
	public Transform visitorsParent;
	public Transform wanderingVisitorsParent;

	private bool visitorsInstantiated = false;
	private bool isCorrecting = false;

	// Use this for initialization
	void Start () {
		StartCoroutine(InstantiateVisitors());
		StartCoroutine(InstantiateWanderingVisitors());
	}

	IEnumerator InstantiateVisitors() {
		yield return InstantiateVisitors(visitors);
		visitorsInstantiated = true;
	}

	IEnumerator InstantiateVisitors(int visitorsNbr) {
		for (int i = 0; i < visitorsNbr; ++i) {
			var visitor = Instantiate(visitorGO, visitorsSpawn.position, new Quaternion(), visitorsParent);
			yield return new WaitForSeconds(1f);
		}
		isCorrecting = false;
	}

	IEnumerator InstantiateWanderingVisitors() {
		for (int i = 0; i < wanderingVistors; ++i) {
			var visitor = Instantiate(wanderingVisitorGO, wanderingVistorsSpawn.position, new Quaternion(), wanderingVisitorsParent);
			yield return new WaitForSeconds(1f);
		}
	}

	int GetNbrOfChildren(Transform parent) {
		int children = 0;
		foreach (Transform t in parent) {
			++children;
		}
		return children;
	}

	// Update is called once per frame
	void Update () {
		if (visitorsInstantiated) {
			var children = GetNbrOfChildren(visitorGO.transform);
			// Corrects the total number of visitors
			// Keeps instantiating until the number is right
			if (children < visitors && !isCorrecting) {
				Debug.Log("correction");
				isCorrecting = true;
				StartCoroutine(InstantiateVisitors(visitors - children));
			}
		}
	}
}
