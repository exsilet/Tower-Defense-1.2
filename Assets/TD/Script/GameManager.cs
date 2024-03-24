﻿/// <summary>
/// Game manager. 
/// Handle all the actions, parameter of the game
/// You can easy get the state of the game with the IListener script.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using YG;

public class GameManager: MonoBehaviour {
	public static GameManager Instance{ get; private set;}

	public enum GameState{Menu,Playing, GameOver, Success, Pause};
	public GameState State{ get; set; }
    
    [ReadOnly] public int levelStarGot;
    [ReadOnly] public bool isWatchingAd;
    [HideInInspector]
	public List<IListener> listeners;

    //add listener called by late actived object
    public void AddListener(IListener _listener)
    {
        if (!listeners.Contains(_listener))     //check if this added or not
            listeners.Add(_listener);
    }
    //remove listener when Die or Disable
    public void RemoveListener(IListener _listener)
    {
        if (listeners.Contains(_listener))      //check if this added or not
            listeners.Remove(_listener);
    }

    public int Point { get; set; }

    public int Death { get; set; }

    void Awake(){
		Instance = this;
		State = GameState.Menu;
		listeners = new List<IListener> ();
	}

	IEnumerator Start(){
		yield return new WaitForEndOfFrame ();

		//menuManager = FindObjectOfType<MenuManager> ();

		//soundManager = FindObjectOfType<SoundManager> ();
//		if (AdsController.instance)
//			AdsController.instance.HideBanner ();

		
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ShowFloatingText(string text, Vector2 positon, Color color){
		//if (GameManager.Instance.State != GameState.Playing)
		//	return;
		
		//GameObject floatingText = Instantiate (FloatingText) as GameObject;
		//var _position = Camera.main.WorldToScreenPoint (positon);

		//floatingText.transform.SetParent (menuManager.transform,false);
		//floatingText.transform.position = _position;
			
		//var _FloatingText = floatingText.GetComponent<FloatingText> ();
		//_FloatingText.SetText (text, color);
	}

    //called by MenuManager
    public void StartGame(){
		State = GameState.Playing;

		//Get all objects that have IListener

		var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
		foreach (var _listener in listener_) {
			listeners.Add (_listener);
		}

		foreach (var item in listeners) {
			item.IPlay ();
		}
	}

	public void Gamepause(){
		State = GameState.Pause;
		foreach (var item in listeners)
			item.IPause ();
	}

	public void UnPause(){
		State = GameState.Playing;
		foreach (var item in listeners)
			item.IUnPause ();
	}

	public void Victory(){
        //if (AdsManager.Instance)
        //    AdsManager.Instance.ShowNormalAd(GameState.Success);

        SoundManager.Instance.PauseMusic(true);
        SoundManager.PlaySfx(SoundManager.Instance.soundVictory, 0.6f);
        State = GameState.Success;

        foreach (var item in listeners)
        {
            if (item != null) 
                item.ISuccess();
        }
        GlobalValue.SavedDeath += Death;
        GlobalValue.SavedCoins += Point;
        Debug.Log(GlobalValue.SavedDeath);
        YandexGame.NewLeaderboardScores("CountEnemy", GlobalValue.SavedDeath);

        //save level and save star
        if (GlobalValue.levelPlaying > GlobalValue.LevelPass)
            GlobalValue.LevelPass = GlobalValue.levelPlaying;

        //if (point >= levelmanager.instance.point3stars)
        //    globalvalue.levelstar(scenemanager.getactivescene().name, 3);
        //else if (point >= levelmanager.instance.point2stars)
        //    globalvalue.levelstar(scenemanager.getactivescene().name, 2);
        //else
        //    globalvalue.LevelStar(SceneManager.GetActiveScene().name, 1);
    }
   
    private void OnDisable()
    {
        if (State == GameState.Success)
            GlobalValue.LevelStar(GlobalValue.levelPlaying, levelStarGot);
    }

    //IEnumerator GameFinishCo()
    //{
    //yield return new WaitForSeconds(.5f);
    //MenuManager.Instance.Gamefinish();
    //SoundManager.PlaySfx(SoundManager.Instance.soundGamefinish, 0.5f);

    //save coins and points
    //GlobalValue.SavedCoins = Coin;

    //save level and save star
    //GlobalValue.UnlockLevel(SceneManager.GetActiveScene().name);


    //if (Point >= LevelManager.Instance.point3Stars)
    //    GlobalValue.LevelStar(SceneManager.GetActiveScene().name, 3);
    //else if (Point >= LevelManager.Instance.point2Stars)
    //    GlobalValue.LevelStar(SceneManager.GetActiveScene().name, 2);
    //else
    //    GlobalValue.LevelStar(SceneManager.GetActiveScene().name, 1);
    //}

    public void UnlockNextLevel()
    {
        //if (GlobalValue.levelPlaying > GlobalValue.LevelPass)
        //    GlobalValue.LevelPass = GlobalValue.levelPlaying;
    }

    public void GameOver(){
        //if (AdsManager.Instance)
        //    AdsManager.Instance.ShowNormalAd(GameState.GameOver);

        SoundManager.Instance.PauseMusic(true);
        //Debug.LogError("GameOver");
        if (State == GameState.GameOver)
            return;
		
		State = GameState.GameOver;
        //Debug.LogError("CALL");

        foreach (var item in listeners)
			item.IGameOver ();
	}

    [HideInInspector]
    public List<GameObject> enemyAlives;
    [HideInInspector]
    public List<GameObject> listEnemyChasingPlayer;

    public void RigisterEnemy(GameObject obj)
    {
        enemyAlives.Add(obj);
    }

    public void RemoveEnemy(GameObject obj)
    {
        enemyAlives.Remove(obj);
        //		Debug.LogError ("ENEMY LEFT: " + enemyAlives.Count);
    }

    public int EnemyAlive()
    {
        return enemyAlives.Count;
    }

    
}
