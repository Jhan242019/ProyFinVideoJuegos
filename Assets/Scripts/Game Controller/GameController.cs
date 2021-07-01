using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


//Controlador del juego

public class GameController : MonoBehaviour {
	public static GameController instance;

	public bool estaEncendidaMusica;
	public bool esJuegoIniciadoPrimeraVez;
	public bool[] levels;
	public int[] altasPuntuaciones;
	public int puntaje;
	public int levelActual;

	private GameData datos;

	void Awake(){
		CreateInstance ();
	}

	// Use this for initialization
	void Start () {
		InitializeGameVariables ();
	}

	void CreateInstance(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void InitializeGameVariables(){
		Load ();
		
		if (datos != null) {
			esJuegoIniciadoPrimeraVez = datos.GetIsGameStartedFirstTime ();
		} else {
			esJuegoIniciadoPrimeraVez = true;
		}

		if (esJuegoIniciadoPrimeraVez) {
			esJuegoIniciadoPrimeraVez = false;
			estaEncendidaMusica = true;
			levels = new bool[15];
			altasPuntuaciones = new int[levels.Length];

			levels [0] = true;
			for (int i = 1; i < levels.Length; i++) {
				levels [i] = false;
			}

			for (int i = 0; i < altasPuntuaciones.Length; i++) {
				altasPuntuaciones[i] = 0;
			}

			datos = new GameData ();

			datos.SetIsMusicOn (estaEncendidaMusica);
			datos.SetIsGameStartedFirstTime (esJuegoIniciadoPrimeraVez);
			datos.SetHighScore (altasPuntuaciones);
			datos.SetLevels (levels);

			Save ();

			Load ();
		} else {
			esJuegoIniciadoPrimeraVez = datos.GetIsGameStartedFirstTime ();
			estaEncendidaMusica = datos.GetIsMusicOn ();
			altasPuntuaciones = datos.GetHighScore ();
			levels = datos.GetLevels ();
		}

	}

	public void Save(){
		FileStream file = null;

		try{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Create(Application.persistentDataPath + "/data.dat");

			if(datos != null){
				datos.SetIsMusicOn(estaEncendidaMusica);
				datos.SetIsGameStartedFirstTime(esJuegoIniciadoPrimeraVez);
				datos.SetHighScore(altasPuntuaciones);
				datos.SetLevels(levels);
				bf.Serialize(file, datos);
			}

		}catch(Exception e){
			Debug.LogException (e, this);
		}finally{
			if(file != null){
				file.Close ();
			}
		}
	}

	public void Load(){
		FileStream file = null;

		try{
			BinaryFormatter bf = new BinaryFormatter();
			file = File.Open(Application.persistentDataPath + "/data.dat", FileMode.Open);
			datos = bf.Deserialize(file) as GameData;

		}catch(Exception e){
			Debug.LogException (e, this);
		}finally{
			if(file != null){
				file.Close ();
			}
		}
	}
}

[Serializable]
class GameData{
	private bool isGameStartedFirstTime;
	private bool isMusicOn;
	private bool[] levels;
	private int[]  highscore;

	public void SetIsGameStartedFirstTime(bool isGameStartedFirstTime){
		this.isGameStartedFirstTime = isGameStartedFirstTime;
	}

	public bool GetIsGameStartedFirstTime(){
		return this.isGameStartedFirstTime;
	}

	public void SetIsMusicOn(bool isMusicOn){
		this.isMusicOn = isMusicOn;
	}

	public bool GetIsMusicOn(){
		return this.isMusicOn;
	}

	public void SetHighScore(int[] highscore){
		this.highscore = highscore;
	}

	public int[] GetHighScore(){
		return this.highscore;
	}

	public void SetLevels(bool[] levels){
		this.levels = levels;
	}

	public bool[] GetLevels(){
		return this.levels;
	}
}
