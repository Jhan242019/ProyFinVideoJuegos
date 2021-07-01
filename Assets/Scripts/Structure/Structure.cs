using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Structure : MonoBehaviour {

	public int maxHitPoints;
	public Sprite[] sprite;
	public float damageCounter;
	public GameObject disassemble;
	public AudioClip stoneSound, woodSound, glassSound;
	public enum StructureType{
		Wood,
		Stone,
		Glass
	}

	public StructureType structure;

	public int hitpoints;
	private SpriteRenderer spriteRenderer;
	private Vector3 bounce;
	private int maxScore;
	private int counter;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
		InitializeVariables ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void InitializeVariables(){
		hitpoints = maxHitPoints;
		maxScore = 500;
		counter = 2;
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.relativeVelocity.magnitude > damageCounter){
			hitpoints -= Mathf.RoundToInt (collision.relativeVelocity.magnitude);
			UpdateScoreStatus (Mathf.RoundToInt (collision.relativeVelocity.magnitude));
		}


		if (hitpoints <= 50) {
			spriteRenderer.sprite = sprite [0];
			if(counter == 2){
				AudioManager ();
				counter--;
			}

		}

		if(hitpoints <= 30){
			spriteRenderer.sprite = sprite [1];
			if(counter == 1){
				AudioManager ();
				counter--;
			}
		}


		if(hitpoints <= 0){
			Destroyed ();

			if(collision.gameObject.CompareTag("Player Bullet")){
				bounce = collision.transform.GetComponent<Rigidbody2D> ().velocity;
				bounce.y = 0f;
				collision.transform.GetComponent<Rigidbody2D> ().velocity = bounce;
			}
		}
	}
		
	void Destroyed(){
		Destroy (gameObject);
		GameObject newDisassemble = Instantiate (disassemble, transform.position, Quaternion.identity) as GameObject;
		Destroy (newDisassemble, 3f);
		if(GameController.instance != null){
			GameController.instance.puntaje += maxScore;
		}

		GameObject scoreText = Instantiate (Resources.Load("Score Text Canvas"), new Vector3 (transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity) as GameObject;
		scoreText.transform.GetChild (0).transform.GetComponent<Text> ().text = maxScore.ToString();
		Destroy (scoreText, 2f);
	}

	void UpdateScoreStatus(int hitScore){
		if(GameController.instance != null){
			GameController.instance.puntaje += hitScore; 
		}
	}

	void AudioManager(){
		switch (structure) {
		case StructureType.Wood:
			if(GameController.instance != null && MusicController.instance != null){
				if(GameController.instance.estaEncendidaMusica){
					if (gameObject != null) {
						AudioSource.PlayClipAtPoint (woodSound, transform.position);
					}
				}
			}
			break;

		case StructureType.Stone:
			if(GameController.instance != null && MusicController.instance != null){
				if(GameController.instance.estaEncendidaMusica){
					if (gameObject != null) {
						AudioSource.PlayClipAtPoint (stoneSound, transform.position);
					}
				}
			}
			break;

		case StructureType.Glass:
			if(GameController.instance != null && MusicController.instance != null){
				if(GameController.instance.estaEncendidaMusica){
					if (gameObject != null) {
						AudioSource.PlayClipAtPoint (glassSound, transform.position);
					}
				}
			}
			break;
		}
	}
}
