using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;

	public bool gameInProgress;
	public Transform player;
	public CameraFollow camera;
	public Text scoreText, shotText, highscore;
	public GameObject gameOverPanel, gameWinPanel, pausePanel;

	[HideInInspector]
	public bool lastShot;

	private bool gameFinished, checkGameStatus;
	private List<GameObject> enemies;
	private List<GameObject> objects;
	private float timeAfterLastShot, distance, time, timeSinceStartedShot;
	private int prevLevel;

	void Awake(){
		CreateInstance ();
	}

	// Use this for initialization
	void Start () {
		InitializeVariables ();

		if (GameController.instance != null && MusicController.instance != null) {
			if (GameController.instance.estaEncendidaMusica) {
				MusicController.instance.GameplaySound ();
			} else {
				MusicController.instance.StopAllSounds ();
			}
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		if (gameInProgress) {
			GameIsOnPlay ();
			DistanceBetweenCannonAndBullet ();
		}


		if(GameController.instance != null){
			UpdateGameplayController ();
		}
			
	}

	void CreateInstance(){
		if(instance == null){
			instance = this;
		}
	}

	void UpdateGameplayController(){
		scoreText.text = GameController.instance.puntaje.ToString("N0");
		shotText.text = "X" + PlayerBullet ();
	}

	void InitializeVariables(){
		gameInProgress = true;
		enemies = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Enemy"));
		objects = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Object"));
		distance = 10f;
		if(GameController.instance != null){
			GameController.instance.puntaje = 0;
			prevLevel = GameController.instance.levelActual;
			highscore.transform.GetChild (0).transform.GetComponent<Text> ().text = GameController.instance.altasPuntuaciones [GameController.instance.levelActual - 1].ToString ("N0");

			if(GameController.instance.altasPuntuaciones[GameController.instance.levelActual - 1] > 0){
				highscore.gameObject.SetActive (true);
			}

		}
			
	}
		
	void GameIsOnPlay(){
	

		if(checkGameStatus){
			timeAfterLastShot += Time.deltaTime;
			if (timeAfterLastShot > 2f) {
				if (AllStopMoving () || Time.time - timeSinceStartedShot > 8f) {
					if (AllEnemiesDestroyed ()) {
						if (!gameFinished) {
							gameFinished = true;
							GameWin ();
							timeAfterLastShot = 0;
							checkGameStatus = false;
						}
					} else {
						if (PlayerBullet () == 0) {
							if (!gameFinished) {
								gameFinished = true;
								timeAfterLastShot = 0;
								checkGameStatus = false;
								GameLost ();
							}
						} else {
							checkGameStatus = false;
							camera.estaSiguiendo = false;
							timeAfterLastShot = 0;
						}
					}
				}
			}

		}

	}

	void GameWin(){
		if(GameController.instance != null && MusicController.instance != null){
			if(GameController.instance.estaEncendidaMusica){
				AudioSource.PlayClipAtPoint (MusicController.instance.winSound, Camera.main.transform.position);
			}

			if(GameController.instance.puntaje > GameController.instance.altasPuntuaciones[ GameController.instance.levelActual - 1]){
				GameController.instance.altasPuntuaciones [ GameController.instance.levelActual - 1] = GameController.instance.puntaje;
			}

			highscore.text = GameController.instance.altasPuntuaciones [GameController.instance.levelActual].ToString ("N0");

			int level = GameController.instance.levelActual;
			level++;
			if(!(level-1 >= GameController.instance.levels.Length)){
				GameController.instance.levels [level - 1] = true;
			}

			GameController.instance.Save ();
			GameController.instance.levelActual = level;
		}
		gameWinPanel.SetActive (true);

	}

	void GameLost(){
		if(GameController.instance != null && MusicController.instance != null){
			if(GameController.instance.estaEncendidaMusica){
				AudioSource.PlayClipAtPoint (MusicController.instance.loseSound, Camera.main.transform.position);
			}
		}
		gameOverPanel.SetActive (true);
	}


	public int PlayerBullet(){
		int playerBullet = GameObject.FindGameObjectWithTag ("Player").transform.GetChild (0).transform.GetComponent<Cannon> ().shot;
		return playerBullet;
	}

	
	private bool AllEnemiesDestroyed(){
		return enemies.All(x => x == null);
	}


	private bool AllStopMoving(){
		foreach (var item in objects.Union(enemies)) {
			if(item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 0){
				return false;
			}
				
		}

		return true;
	}

	void DistanceBetweenCannonAndBullet(){
		GameObject[] bullet = GameObject.FindGameObjectsWithTag ("Player Bullet");
		foreach (GameObject distanceToBullet in bullet) {
			if (!distanceToBullet.transform.GetComponent<CannonBullet> ().isIdle) {
				if (distanceToBullet.transform.position.x - player.position.x > distance) {
					camera.estaSiguiendo = true;
					checkGameStatus = true;
					timeSinceStartedShot = Time.time;
					TimeSinceShot ();
					camera.objetivo = distanceToBullet.transform;
				} else {
					if(PlayerBullet() == 0){
						camera.estaSiguiendo = true;
						checkGameStatus = true;
						timeSinceStartedShot = Time.time;
						TimeSinceShot ();
						camera.objetivo = distanceToBullet.transform;
					}
				}
			}
		}

	}

	void TimeSinceShot(){
		time += Time.deltaTime;
		if (time > 3f) {
			time = 0f;
			GameObject.FindGameObjectWithTag ("Player Bullet").transform.GetComponent<CannonBullet> ().isIdle = true;
		}
			
	}

	public void RestartGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		if(GameController.instance != null){
			GameController.instance.levelActual = prevLevel;
		}
	}

	public void BackToLevelMenu(){
		if(GameController.instance != null && MusicController.instance != null){
			if (GameController.instance.estaEncendidaMusica) {
				MusicController.instance.PlayBgMusic ();
			} else {
				MusicController.instance.StopAllSounds ();
			}
		}
		SceneManager.LoadScene ("Level Menu");
		Time.timeScale = 1;
	}

	public void ContinueGame(){
		if (GameController.instance != null) {
			SceneManager.LoadScene ("Level " + GameController.instance.levelActual);
		}
	}

	public void PauseGame(){
		if (gameInProgress) {
			gameInProgress = false;
			Time.timeScale = 0;
			pausePanel.SetActive (true);
		}
	}

	public void ResumeGame(){
		Time.timeScale = 1;
		gameInProgress = true;
		pausePanel.SetActive (false);
	}


		

}
