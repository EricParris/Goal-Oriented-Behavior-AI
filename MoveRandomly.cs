using UnityEngine;
using System.Collections;

public class MoveRandomly : MonoBehaviour {
	public float speed;
	public float interval;
	public float randomHorz;
	public float randomVert;
	public Vector3 movement;
	// Use this for initialization
	void Start () {
		float randomHorz = Random.Range (-1,1);
		float randomVert = Random.Range (-1,1);
		this.movement = new Vector3(randomHorz, 0.0f, randomVert);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (this.interval == 0) {
			changeDirection();
				}
		//Vector3 movement; = new Vector3(1, 0.0f, 1);
		transform.position += movement * speed * Time.deltaTime;
		//rigidbody.AddForce(movement * speed * Time.deltaTime);
		this.interval --;
	}
	void randomInterval(){
		this.interval = Random.Range (30, 150);
		}
	void changeDirection(){
		float randomHorz = Random.Range (-1.0f,1.0f);
		float randomVert = Random.Range (-1.0f,1.0f);
		this.movement = new Vector3(randomHorz, 0.0f, randomVert);
		randomInterval ();
		}
}
