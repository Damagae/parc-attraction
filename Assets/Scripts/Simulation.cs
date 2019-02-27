using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Simulation : MonoBehaviour {

	public int visitors;
	public int wanderingVistors;
	public GameObject visitorGO;
	public GameObject wanderingVisitorGO;
	public Transform visitorsSpawn;
	public Transform wanderingVistorsSpawn;
	public Transform visitorsParent;
	public Transform wanderingVisitorsParent;
	public float timeBetweenSpawns = 1f;

	private bool visitorsInstantiated = false;
	private bool isCorrecting = false;
	private List<Vector3> randomPositions;
	private float mapWidth = 250;
	private float mapLength = 300;

	// Use this for initialization
	void Start () {
		LoadRandomPositions();
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
			yield return new WaitForSeconds(timeBetweenSpawns);
		}
		isCorrecting = false;
	}

	IEnumerator InstantiateWanderingVisitors() {
		for (int i = 0; i < wanderingVistors; ++i) {
			var visitor = Instantiate(wanderingVisitorGO, wanderingVistorsSpawn.position, new Quaternion(), wanderingVisitorsParent);
			yield return new WaitForSeconds(timeBetweenSpawns);
		}
	}

	int GetNbrOfChildren(Transform parent) {
		int children = 0;
		foreach (Transform t in parent) {
			++children;
		}
		return children;
	}


	// Mausaise implémentation
	public List<Vector3> LoadRandomPositions() {
		randomPositions = new List<Vector3>();
		// int it = 0;
		// while (randomPositions.Count < 200 || it < 400) {
		// 	var randVector = new Vector3 (Random.Range(0.0f, mapWidth), 0, Random.Range(0.0f, mapLength));
		// 	NavMeshHit hit;
		// 	if (NavMesh.SamplePosition(randVector, out hit, 1f, 30)) {
		// 		randomPositions.Add(randVector);
		// 		Debug.Log("add");
		// 	}
		// 	++it;
		// }
		return randomPositions;
	}

	// Update is called once per frame
	void Update () {
		if (visitorsInstantiated) {
			var children = GetNbrOfChildren(visitorGO.transform);
			// Corrects the total number of visitors
			// Keeps instantiating until the number is right
			if (children < visitors && !isCorrecting) {
				isCorrecting = true;
				StartCoroutine(InstantiateVisitors(visitors - children));
			}
		}
	}
}
