using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour {

	public GameObject cannonBullet;
	public Transform cannonTip;
	public Slider powerLevel;
	public AudioClip cannonShot;
	public int maxShot;
	public bool readyToShoot;
	public int shot;

	private int minLevel, maxLevel, timeDelay;
	private bool isMax, isCharging;
	private AudioSource audioSource;
	private float minRot, maxRot;
	private Vector3 currentRotation, touchPos;
	private int counter = 0;
	private Vector2 direction = Vector2.zero;

	void Awake(){
		audioSource = GetComponent<AudioSource> ();
	}

	// Inicializamos las variables necesarias para la jugabilidad
	void Start () {
		readyToShoot = true;
		shot = maxShot;
		timeDelay = 50;
		minLevel = 0;
		maxLevel = 50;
		powerLevel.maxValue = maxLevel;
		powerLevel.value = minLevel;
		maxRot = 20;
		minRot = -25;
		Vector3 currentRotation = transform.rotation.eulerAngles;
	}
	

	void Update () {
		if (GameplayController.instance.gameInProgress) {
			if (readyToShoot) {
				if(Application.platform == RuntimePlatform.Android){
					TouchCannonShoot ();
				}else if(Application.platform == RuntimePlatform.WindowsEditor){
					CannonShoot ();
				}

			}

			if(Application.platform == RuntimePlatform.Android){
				TouchCannonMovement ();
			}else if(Application.platform == RuntimePlatform.WindowsEditor){
				CannonMovement ();
			}
				
		}
	}

	void TouchCannonShoot(){
		if(Input.touchCount > 0){
			Touch touch = Input.GetTouch (0);

			touchPos = Camera.main.ScreenToWorldPoint (touch.position);

			Vector2 touchRayHit = new Vector2 (touchPos.x, touchPos.y);

			RaycastHit2D hit = Physics2D.Raycast (touchRayHit , Vector2.zero);

			if(hit.collider != null){
				if(hit.collider.CompareTag("Player")){
					if(touch.phase == TouchPhase.Stationary){
						if (shot != 0) {
							UpdatePowerLevel ();
							isCharging = true;
						}
					}else if(touch.phase == TouchPhase.Ended){
						if (shot != 0) {
							GameObject newCannonBullet = Instantiate (cannonBullet, cannonTip.position, Quaternion.identity) as GameObject;
							newCannonBullet.transform.GetComponent<Rigidbody2D> ().AddForce (cannonTip.right * powerLevel.value, ForceMode2D.Impulse);
							if (GameController.instance != null && MusicController.instance != null) {
								if (GameController.instance.estaEncendidaMusica) {
									audioSource.PlayOneShot (cannonShot);
								}
							}
							shot--;
							powerLevel.value = 0;
							readyToShoot = false;
							isCharging = false;
						}
					}
				}
			}
		}
	}

	void CannonShoot(){
		if (Input.GetKey (KeyCode.Space)) {
			if(shot != 0){
				UpdatePowerLevel ();
			}
		}else if(Input.GetKeyUp(KeyCode.Space)){
			if (shot != 0) {
				GameObject newCannonBullet = Instantiate (cannonBullet, cannonTip.position, Quaternion.identity) as GameObject;
				newCannonBullet.transform.GetComponent<Rigidbody2D> ().AddForce (cannonTip.right * powerLevel.value, ForceMode2D.Impulse);
				if (GameController.instance != null && MusicController.instance != null) {
					if (GameController.instance.estaEncendidaMusica) {
						audioSource.PlayOneShot (cannonShot);
					}
				}
				shot--;
				powerLevel.value = 0;
				readyToShoot = false;
			}
		}
	}

	void UpdatePowerLevel(){
		if (!isMax) {
			powerLevel.value += timeDelay * Time.deltaTime;

			if(powerLevel.value == maxLevel){
				isMax = true;
			}

		} else {
			powerLevel.value -= timeDelay * Time.deltaTime;

			if(powerLevel.value == minLevel){
				isMax = false;
			}
		}
	}

	void CannonMovement(){
		if (Input.GetKey (KeyCode.UpArrow)) {
			currentRotation.z += 50f * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			currentRotation.z -= 50f * Time.deltaTime;
		}
		currentRotation.z = Mathf.Clamp(currentRotation.z, minRot, maxRot);
		transform.rotation = Quaternion.Euler (currentRotation);
	}

	void TouchCannonMovement(){
		if(Input.touchCount > 0){
			Touch touch = Input.GetTouch (0);

			if (touch.position.x < 300 && !isCharging && touch.position.y > 160) {
				if (touch.phase == TouchPhase.Moved) {
					Vector2 touchDeltaPosition = touch.deltaPosition;
					direction = touchDeltaPosition.normalized;

					if (direction.y > 0 && touchDeltaPosition.x > -10 && touchDeltaPosition.x < 10) {
						currentRotation.z += 50f * Time.deltaTime;
					} else if (direction.y < 0 && touchDeltaPosition.x > -10 && touchDeltaPosition.x < 10) {
						currentRotation.z -= 50f * Time.deltaTime;
					}
				}
			}
		}

		currentRotation.z = Mathf.Clamp(currentRotation.z, minRot, maxRot);
		transform.rotation = Quaternion.Euler (currentRotation);
	}
}
