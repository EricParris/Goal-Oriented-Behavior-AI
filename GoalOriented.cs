using UnityEngine;
using System.Collections;

public class GoalOriented : MonoBehaviour {
	Transform thisTransform;
	public float seperationRadius;
	public float cohesionRadius;
	public float fleeRadius;
	public float runSpeed = 3;
	public float runAwaySpeed;
	public Vector3 actualMovement;
	public float timeTimer;
	public float movementCheck;
	public int hunger;
	public int thirst;
	public int fatigue;
	public float sleepTimer;
	public float drinkTimer;
	public float eatTimer;
	public bool sleeping;
	public bool eating;
	public bool drinking;
	public float hungerMultiplier;
	public float thirstMultiplier;
	// Use this for initialization
	void Start () {
		sleepTimer = 0.0f;
		drinkTimer = 0.0f;
		eatTimer = 0.0f;
		timeTimer = 0.0f;
		movementCheck = 0.0f;
		hungerMultiplier = 1.0f;
		thirstMultiplier = 1.0f;
		hunger = Random.Range(0, 80);
		thirst = Random.Range(0, 80);
		fatigue = Random.Range(0, 80);
		sleeping = false;
		eating = false;
		drinking = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (timeTimer >=90) {
			hunger++;
			thirst++;
			timeTimer = 0.0f;
		}
		if (movementCheck >= 90) {
			fatigue++;
			movementCheck = 0;
				}
		CharacterController controller = GetComponent<CharacterController>();
		thisTransform = transform;
		Vector3 objectPosition = thisTransform.position;
		Vector3 cohesionVector = cohesion(objectPosition,cohesionRadius);
		Vector3 seperationVector = seperation (objectPosition, seperationRadius);
		Vector3 mimicVector = mimic (objectPosition, cohesionRadius);
		Vector3 hungerVector = food (objectPosition, 1000.0f);
		Vector3 thirstVector = water (objectPosition, 1000.0f);
		flee(objectPosition, fleeRadius, runAwaySpeed, controller);
			cohesionVector.y = 0.0f;
			seperationVector.y = 0.0f;
			mimicVector.y = 0.0f;
			hungerVector.y = 0.0f;
			
			getMultipliers ();
			actualMovement = cohesionVector * 10.0F + seperationVector * 1.5f + mimicVector * 1.0f + hungerVector * (hungerMultiplier) + thirstVector * thirstMultiplier;
			actualMovement.Normalize ();
			sleep ();
			eat (objectPosition, 1.0f);
			drink (objectPosition, 1.0f);
			if(sleeping == false && eating == false && drinking == false){
				controller.Move(actualMovement* runSpeed * Time.deltaTime);
				movementCheck++;
				timeTimer++;
			}
			else if(sleeping == true){
				sleepTimer += 1;
				if(sleepTimer >= 60){
					fatigue -= 10;
					sleepTimer = 0;
				}
				if(fatigue <= 0){
					sleeping = false;
					fatigue = 0;
				}
			}
			else if(eating == true){
				eatTimer += 1;
				if(eatTimer >= 60){
					hunger -= 10;
					eatTimer = 0;
				}
				if(hunger <= 0){
					eating = false;
					hunger = 0;
				}
			}
			else if(drinking == true){
				drinkTimer += 1;
				if(drinkTimer >= 60){
					thirst -= 10;
					drinkTimer = 0;
				}
				if(thirst <= 0){
					drinking = false;
					thirst = 0;
				}
			}
		
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
	Vector3 food(Vector3 objectLocation, float radius1){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius1);
		Vector3 foodDirection = new Vector3(0,0,0);
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "food"){
				foodDirection = hitColliders[i].transform.position - transform.position;
				foodDirection.Normalize();
			}
			i++;
		}
		//print (centerPosition);
		return foodDirection;
	}
	Vector3 water(Vector3 objectLocation, float radius1){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius1);
		Vector3 waterDirection = new Vector3(0,0,0);
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "water"){
				waterDirection = hitColliders[i].transform.position - transform.position;
				waterDirection.Normalize();
			}
			i++;
		}
		//print (centerPosition);
		return waterDirection;
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
	void sleep(){
		if(fatigue >= 100 && eating == false && drinking == false)
		sleeping = true;
	}
	void eat(Vector3 objectLocation, float radius1){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius1);
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "food" && sleeping == false && drinking == false && hunger >= 70){
				eating = true;
			}
			i++;
		}
	}
	void drink(Vector3 objectLocation, float radius1){
		Collider[] hitColliders = Physics.OverlapSphere(objectLocation, radius1);
		int i = 0;
		while (i < hitColliders.Length) {
			if(hitColliders[i].tag == "water" && sleeping == false && drinking == false && thirst >= 70){
				drinking = true;
			}
			i++;
		}
	}
	void getMultipliers(){
		if (hunger >= 80.0f) {
			hungerMultiplier = 15.0f;
				}
		if (hunger < 20.0f) {
			hungerMultiplier = 1.0f;
			}
		if (thirst >= 80.0f) {
			thirstMultiplier = 15.0f;
		}
		if (thirst < 20.0f) {
			thirstMultiplier = 1.0f;
		}
		if (hunger >= 80 && thirst >= 80) {
			hungerMultiplier = 1.0f;
			thirstMultiplier = 15.0f;
		}
	}
}
