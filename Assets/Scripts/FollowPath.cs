using UnityEngine;
using System.Collections;

 public class FollowPath : MonoBehaviour {

     private GameObject follower;
     public Transform[] pathNodes;
     public float movementSpeed = 0.05f;
     public bool continuouslyLoop = false;
     private float pathPosition = 0f;
     private int curNode = 0;
		 public Attraction attraction;
		 private Vector3 start;

     // Use this for initialization
     void Start () {
         follower = this.gameObject;
				 start = pathNodes[0].position;
				 //movementSpeed = attraction.duration * 0.05f/14;
     }

     // Update is called once per frame
     void Update () {
			 if (!attraction.IsOver()) {
				 Run();
			 } else {
				 follower.transform.position = start;
				 Reset();
			 }
     }

		 void Reset() {
			 pathPosition = 0f;
	     curNode = 0;
		 }

		 void Run() {
			 if (pathNodes != null)
			 {
					 pathPosition += Time.deltaTime * movementSpeed;
					 if (pathPosition > 1f)
					 {
							 if (pathNodes.Length > curNode+2)
							 {
									 pathPosition = 0f;
									 curNode += 1;
							 }
							 else
							 {
									// pathPosition = 0f;
									// curNode = 0;
							 }
					 }
					 follower.transform.position = Vector3.Lerp( pathNodes[curNode].position, pathNodes[curNode+1].position, pathPosition );
			 }
		 }
 }
