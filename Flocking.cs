using UnityEngine;
using System.Collections;

public class Flocking : MonoBehaviour {
	Transform thisTransform;
	public float seperationRadius;
	public float cohesionRadius;
	public float fleeRadius;
	public float runSpeed = 3;
	public float runAwaySpeed;
	public Vector3 actualMovement;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CharacterController controller = GetComponent<CharacterController>();
		thisTransform = transform;
		Vector3 objectPosition = thisTransform.position;
		Vector3 cohesionVector = cohesion(objectPosition,cohesionRadius);
		Vector3 seperationVector = seperation (objectPosition, seperationRadius);
		Vector3 mimicVector = mimic (objectPosition, cohesionRadius);
		flee(objectPosition, fleeRadius, runAwaySpeed, controller);
		cohesionVector.y = 0.0f;
		seperationVector.y = 0.0f;
		mimicVector.y = 0.0f;
		actualMovement = cohesionVector * 4.0F + seperationVector * 1.5f + mimicVector * 1.0f;
		actualMovement.Normalize ();
		controller.Move(actualMovement* runSpeed * Time.deltaTime);
	}

	Vector3 cohesion(Vector3 objectLocation, float radius1){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius1);
		Vector3 centerPosition = new Vector3 (0,0,0);
		int herdCount = 0;
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "pickup"){
				Vector3 target = hitColliders[i].transform.position - transform.position;
				target.Normalize();
				centerPosition += target;
				herdCount++;
			}
			i++;
		}
		centerPosition = centerPosition / herdCount;
		centerPosition = centerPosition;
		//print (centerPosition);
		return centerPosition;
	}

	Vector3 seperation(Vector3 objectLocation, float radius2){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius2);
		Vector3 moveAway = new Vector3 (0,0,0);
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "pickup"){
				Vector3 target = hitColliders[i].transform.position - transform.position;
				target.Normalize ();
				target = target* -1;
				moveAway += target;
			}
			else if(hitColliders[i].tag == "Player"){
				Vector3 target = hitColliders[i].transform.position - transform.position;
				target = (target * -1) * 10;
				moveAway += target;
			}
			i++;
		}
		moveAway = moveAway * runSpeed;
		//print (moveAway);
		return moveAway;
		
	}
	Vector3 mimic(Vector3 objectLocation, float radius1){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius1);
		Vector3 avgVelocity = new Vector3 (0,0,0);
		Vector3 herdMember;
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "pickup"){
				herdMember = GetComponent<Flocking>().actualMovement;
				herdMember.Normalize();
				avgVelocity += herdMember;
			}
			i++;
		}
		return avgVelocity;
	}
	void flee(Vector3 objectLocation, float radius1, float fleeSpeed, CharacterController controller1){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius1);
		int herdCount = 0;
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "Player" || hitColliders[i].tag == "Predator"){
				Vector3 predator = hitColliders[i].transform.position - transform.position;
				//predator.Normalize();
				predator = predator * -1;
				controller1.Move(predator * fleeSpeed * Time.deltaTime);
			}
			i++;
		}
	}
}
