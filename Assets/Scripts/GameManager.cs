using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum gameStatus {
	start, pause, gameover, win, next
}

public enum gameState{
	build, inGame
}

public class GameManager : Singleton<GameManager> {
	
	[SerializeField]
	private GameObject spawnEnemyPoint;

	[SerializeField]
	private Enemy[] enemies;

	[SerializeField]
	private int totalEnemiesCount = 3;

	[SerializeField]
	private int enemiesPerSpawn;

	[SerializeField]
	private int waves = 10;

	[SerializeField]
	private Text lbMoney;

	[SerializeField]
	private Text lbCurrentWave;

	[SerializeField]
	private Text lbPlay;

	[SerializeField]
	private Button btnPlay;

	[SerializeField]
	private Text lbEscaped;

	private int waveCurrent = 0;
	private int money = 10;
	private int totalEscaped = 0;
	private int escaped = 0;
	private int totalKilled = 0;
	private int enemyType = 0;
	private gameStatus currentState = gameStatus.start;
	private gameState currentGameState = gameState.build;

	public List<Enemy> EnemyList = new List<Enemy> ();
	const float waiting = 0.5f; 
	private AudioSource audio;
	private int enemyToSpawn;

	private int enemyCount = 0;




	void Start(){
		btnPlay.gameObject.SetActive (false);
		this.audio = GetComponent<AudioSource> ();
		showButton ();

		money = 100;
		lbMoney.text = money.ToString ();

	}

	void Update()
	{
		escapeButton ();
	}

	IEnumerator spawn() {
		
		if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemiesCount) {
			
			for (int i = 0; i < enemiesPerSpawn; i++) {
				if (EnemyList.Count <= totalEnemiesCount) {
					
					if (SceneManager.GetActiveScene ().buildIndex == 2) {
                        if (waveCurrent < 4)
                        {
                            Enemy newEnemy = Instantiate(enemies[Random.Range(0, 3)]) as Enemy;
                            newEnemy.transform.position = spawnEnemyPoint.transform.position;
                            enemyCount++;
                        }
                        else if (waveCurrent < 7) {
                            Enemy newEnemy = Instantiate(enemies[Random.Range(0, 2)]) as Enemy;
                            newEnemy.transform.position = spawnEnemyPoint.transform.position;
                            enemyCount++;
                        } else {
                            Enemy newEnemy = Instantiate(enemies[Random.Range(0, 4)]) as Enemy;
                            newEnemy.transform.position = spawnEnemyPoint.transform.position;
                            enemyCount++;
                        }
					} else {
						if (waveCurrent < 4) {
							Enemy newEnemy = Instantiate (enemies [Random.Range (0, 2)]) as Enemy;
							newEnemy.transform.position = spawnEnemyPoint.transform.position;
							enemyCount++;
						}
                        else if(waveCurrent < 9)
                        {
                            Enemy newEnemy = Instantiate(enemies[Random.Range(0, 2)]) as Enemy;
                            newEnemy.transform.position = spawnEnemyPoint.transform.position;
                            enemyCount++;
                        }
                        else {
							Enemy newEnemy = Instantiate (enemies [Random.Range (0, 3)]) as Enemy;
							newEnemy.transform.position = spawnEnemyPoint.transform.position;
							enemyCount++;
						}
					
					}
					
              

				}
			}
			yield return new WaitForSeconds (waiting);
			StartCoroutine (spawn ());
		}
			
	}

	public AudioSource Audio{ get { return audio; }}

	public void RegisterEnemy(Enemy enemy)
	{
		EnemyList.Add (enemy);
	}

	public void UnregisterEnemy(Enemy enemy)
	{
		EnemyList.Remove (enemy);
		Destroy (enemy.gameObject);
	} 

	public void RemoveAll()
	{
		foreach (Enemy enemy in EnemyList) 
		{
			Destroy (enemy.gameObject);
		}

		EnemyList.Clear ();
	}

	public void addMoney(int amount)
	{
		TotalMoney += amount;
	}

	public void substractMoney(int amount)
	{
		TotalMoney -= amount;
	}

	public void showButton()
	{
		switch (currentState) 
		{
		case gameStatus.start:
			lbPlay.text = "Play";
			break;
		case gameStatus.next:
			lbPlay.text = "Next Wave";
			currentGameState = gameState.build;
			break;
		case gameStatus.gameover:
			lbPlay.text = "You lose!";
			break;
		case gameStatus.win:
			lbPlay.text = "You Win!";
			break;
		}

		btnPlay.gameObject.SetActive (true);
	}

	private void escapeButton()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TowerManager.Instance.disableDragSprite ();
			TowerManager.Instance.towerButtonPressed = null;
		}
		else if (Input.GetMouseButtonDown (1))
		{
			TowerManager.Instance.disableDragSprite ();
			TowerManager.Instance.towerButtonPressed = null;
		}
	}

	public void isWaveOver()
	{
		Debug.Log ("totalEnemiesCount" + totalEnemiesCount);

		lbEscaped.text = "Escaped " + TotalEscaped + "/10";
		if ((Escaped + TotalKilled) == totalEnemiesCount) 
		{
			if (waveCurrent <= enemies.Length) 
			{
				enemyToSpawn = waveCurrent + 1;
				totalEnemiesCount += enemyToSpawn -1;
			}
			setGameState ();
			showButton ();
		}
	}

	public void setGameState()
	{
		if (TotalEscaped >= 10) {
			currentState = gameStatus.gameover;
		} else if (waveCurrent == 0 && (TotalKilled + Escaped) == 0) {
			currentState = gameStatus.start;
		} else if (waveCurrent >= waves) {
			currentState = gameStatus.win;
		} else {
			currentState = gameStatus.next;
		}
	}

	public bool checkGameOver(){
		
		if (currentState == gameStatus.gameover) {
			return true;
		}

		return false;
	}

	public void playBtnPressed()
	{
		switch (currentState) 
		{
		case gameStatus.next:
			waveCurrent++;
			totalEnemiesCount += waveCurrent;
			break;
		case gameStatus.gameover:
			SceneManager.LoadScene (0);
			break;
		case gameStatus.win:
			SceneManager.LoadScene (0);
			break;
		default:
			totalEnemiesCount = 3;
			TotalEscaped = 0;
			//TowerManager.Instance.RemoveTowers ();
			//TowerManager.Instance.UnregisterBuilding ();
			lbMoney.text = TotalMoney.ToString ();
			enemyToSpawn = 0; 
			lbEscaped.text = "Escaped " + TotalEscaped.ToString ()+"/10";
			break;
		}

		RemoveAll ();
		TotalKilled = 0;
		Escaped = 0;
		lbCurrentWave.text = "Wave " + (waveCurrent + 1) + "/"+ (waves+1);
		StartCoroutine (spawn ());
		currentGameState = gameState.inGame;
		btnPlay.gameObject.SetActive (false);
	}

	public gameState CurrentGameState{get {return currentGameState; } }

	public int TotalEscaped { get { return totalEscaped;} set { totalEscaped = value; }}

	public int Escaped { get { return escaped;} set {escaped = value; } }

	public int TotalKilled {get {return totalKilled;} set { totalKilled = value; } }

	public int TotalMoney
	{
		get {return money;}
		set 
		{
			money = value;
			lbMoney.text = money.ToString ();
		}
	}
}
