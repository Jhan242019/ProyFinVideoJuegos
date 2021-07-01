using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour {

	
	//establecemosd los objetos y valores que se pasaran atraves del inspecto en unity
	public Transform objetivo;
	public Transform limiteIzquierdo;
	public Transform limiteDerecho;
	public float speed = 5f;
	public bool estaSiguiendo;

	private float arrowX;
	public bool allowToMove;

	private float dragSpeed = 0.05f;
	private float timeSinceShot;
	private Vector3 velocity = Vector3.zero;
	private Vector3 startPosition;
	private Vector3 camPos;
	
	void Start () {
		startPosition = transform.localPosition;
	}
	

	void Update () {
		if (GameplayController.instance.gameInProgress) {
			if (estaSiguiendo) {
				if (GameObject.FindGameObjectWithTag ("Player Bullet") != null) {
					MoveCameraFollow ();
				}
			} else { 
				if (!GameplayController.instance.player.GetChild (0).transform.GetComponent<Cannon> ().readyToShoot) {
					MoveCameraBackToStart ();
					AfterShotMoveAgain ();
					allowToMove = false;
				} else {
					timeSinceShot = 0;
					allowToMove = true;
				}
				
			}
			
			if (Application.platform == RuntimePlatform.Android) {
				TouchMoveCamera ();
			} else if (Application.platform == RuntimePlatform.WindowsEditor) {
				MoveCamera ();
			}
		}

	}

	void MoveCameraFollow(){
		if(objetivo != null){
			transform.position = Vector3.Lerp (new Vector3 (transform.position.x, 0, -10f), new Vector3 (Mathf.Clamp (objetivo.transform.position.x, limiteIzquierdo.position.x, limiteDerecho.position.x), 0, transform.position.z), Time.deltaTime * 10);
		}
	}

	void MoveCameraBackToStart(){
		transform.position = Vector3.MoveTowards (transform.position, startPosition, Time.deltaTime * 5f);
	}

	void AfterShotMoveAgain(){
		timeSinceShot += Time.deltaTime;
		if (timeSinceShot > 2f) {
			if(startPosition == transform.position){
				GameplayController.instance.player.GetChild (0).transform.GetComponent<Cannon> ().readyToShoot = true;
			}
		}
	}

	void MoveCamera(){
		arrowX = Input.GetAxis ("Horizontal");

		Transform cam = gameObject.transform;

		if (allowToMove) {
			if (arrowX != 0) {
				cam.position = cam.position + (new Vector3 (arrowX, 0, 0) * speed * Time.deltaTime);
				float camX = cam.position.x;
				camX = Mathf.Clamp (camX, limiteIzquierdo.transform.position.x, limiteDerecho.transform.position.x);
				cam.position = new Vector3 (camX, cam.position.y, cam.position.z);
			}
		}
	}

	void TouchMoveCamera(){
		if(allowToMove){

		

			if(Input.touchCount > 0){
				Touch touch = Input.GetTouch (0);

				Transform cam = gameObject.transform;
				if (touch.position.x > 300) {
					if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary){
						camPos = touch.position;
					}else if(touch.phase == TouchPhase.Moved){
						cam.position = cam.position + (new Vector3 ((camPos.x - touch.position.x), 0, 0) * dragSpeed * Time.deltaTime);
						float camX = cam.position.x;
						camX = Mathf.Clamp (camX, limiteIzquierdo.transform.position.x, limiteDerecho.transform.position.x);
						cam.position = new Vector3 (camX, cam.position.y, cam.position.z);
					}
				}
					
			}
				
		}
	}


		
}
