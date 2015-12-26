
using UnityEngine;
using System.Collections;
public class RunAway : MonoBehaviour {
	Transform target;
	Transform thisTransform;
	public float minDistance = .1f;
	public float runSpeed = .01f;
	
	void Awake() {
		thisTransform = transform;
	}
	
	void Start() {
	}
	
	void Update() {
		thisTransform = transform;
		target = GameObject.FindGameObjectWithTag("Player").transform;
		if(Vector3.Distance(target.position, thisTransform.position) < minDistance) {
			Vector3 direction = target.position;
			direction.Normalize();
			direction = direction * -1 * runSpeed;
			direction.y = 0.5f;
			thisTransform.position = Vector3.MoveTowards(thisTransform.position, direction, Time.deltaTime);
			//transform.position += direction * Time.deltaTime;
		}
	}
}