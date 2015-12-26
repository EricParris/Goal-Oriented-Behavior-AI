//Debug.log(" ");
using UnityEngine;
using System.Collections;
//[RequireComponent(typeof(CharacterController))]
public class FollowPlayer : MonoBehaviour {
	Transform target;
	Transform thisTransform;
	public float minDistance = 10;
	public float runSpeed = 30;
	
	void Awake() {
		thisTransform = transform;
	}
	
	void Start() {
		

	}
	
	void Update() {
	//	CharacterController controller = GetComponent<CharacterController>();
		thisTransform = transform;
		target = GameObject.FindGameObjectWithTag("Player").transform;
		if(Vector3.Distance(target.position, thisTransform.position) < minDistance) {
			
			Vector3 direction = target.position;
			//direction.Normalize();
			//thisTransform.position = Vector3.MoveTowards(thisTransform.position, direction * minDistance, Time.deltaTime * runSpeed);
			//controller.SimpleMove(direction * runSpeed);
			//thisTransform.Translate(target.position * );
			Vector3 distance = target.position - thisTransform.position;
			float timeToReach = distance.magnitude / runSpeed;
			Vector3 newTarget = direction * timeToReach;
			direction.Normalize();
			direction = direction * runSpeed;
			direction.y = 0.05f;
			newTarget.Normalize();
			newTarget = newTarget * runSpeed;
			thisTransform.position = Vector3.MoveTowards(thisTransform.position, newTarget * minDistance, Time.deltaTime);
		}
	}
}