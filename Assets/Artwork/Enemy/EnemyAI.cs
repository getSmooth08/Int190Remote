using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

	//what to chase?
	public Transform target;

	//how many times per second we will update our path
	public float updateRate = 2.0f;

	private Seeker seeker;
	private Rigidbody2D rb;

	//store the calculated path
	public Path path;

	//The AI Speed per second
	public float speed = 300f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathEnded = false;
	//max distance from AI to waypoint for it to continue to the next way point
	public float nextWaypointDistance = 3f;

	//waypoint we are currently moving towards
	private int currentWaypoint = 0;

	private bool searchingForPlayer = false;

	void Start(){
		seeker = GetComponent<Seeker> (); 
		rb = GetComponent<Rigidbody2D> ();

		if(target == null){

				if(searchingForPlayer)
				{
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer());
				}
				return;
		}

		seeker.StartPath (transform.position, target.position, OnPathComplete);
		//write some more

		StartCoroutine (UpdatePath());
	}

	void Update()
	{

	}

	IEnumerator SearchForPlayer(){
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");
		if(sResult == null){
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(SearchForPlayer());
		}else{
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath ());
			return false;
		}
	}


	IEnumerator UpdatePath(){
		if(target == null){

				if(searchingForPlayer)
				{
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer());
				}
				return false;
		}
		//start new path to the target position
		seeker.StartPath(transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds (1f / updateRate);

		StartCoroutine (UpdatePath ());
	}

	public void OnPathComplete(Path p){
		Debug.Log("We have a path, did it have an error? "+p.error);
		if(!p.error){
			path = p;
			currentWaypoint = 0;
		}
	}

	void FixedUpdate(){
		

		if(target == null){

				if(searchingForPlayer)
				{
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer());
				}
				return;
		}

		//TODO: Always looks at player?

		if(path == null){
			return;
		}
		if(currentWaypoint >= path.vectorPath.Count) {
			if(pathEnded)
				return;

			Debug.Log ("End of Path reached.");
			pathEnded = true;
			return;
		}

		pathEnded = false;

		//direction to the next waypoint
		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		//move the AI
		rb.AddForce (dir, fMode);

		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);

		if(dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}

	}

}
