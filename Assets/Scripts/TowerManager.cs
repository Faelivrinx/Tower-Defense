using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> 
{

	public TowerButton towerButtonPressed{get; set;}

	private SpriteRenderer spriteRenderer;
	private List<TowerController> TowerList = new List<TowerController>();
	private List<Collider2D> BuildList = new List<Collider2D>();
	private Collider2D buildTile;
	private GameObject[] freePositions;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		buildTile = GetComponent<Collider2D> ();

	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0))
		{
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);
			if (hit.collider.tag == "BuildSite")
			{
				buildTile = hit.collider;
				PlaceTower (hit);	
				if (towerButtonPressed != null) {
					buildTile.tag = "BuildSiteFull";
					RegisterBuilding (buildTile);
					towerButtonPressed = null;
				}
			}
		}

		if (spriteRenderer.enabled) 
		{
			followMouse ();
		}
	}	

	public void RegisterTower(TowerController tower)
	{
		TowerList.Add (tower);
	}

	public void RegisterBuilding(Collider2D build)
	{
		BuildList.Add (build);
	}

	public void UnregisterBuilding()
	{
		foreach(Collider2D build in BuildList)
		{
			build.tag = "BuildSite";	
		}
		BuildList.Clear ();
	}

	public void RemoveTowers(){
		foreach (TowerController tower in TowerList) 
		{
			Destroy (tower.gameObject);
		}

		TowerList.Clear ();
	}

	public void PlaceTower(RaycastHit2D hit)
	{	
		if (!EventSystem.current.IsPointerOverGameObject () && towerButtonPressed != null) 
		{
			TowerController newTower = Instantiate (towerButtonPressed.Tower);
			newTower.transform.position = hit.transform.position;
			disableDragSprite ();
			buyTower (towerButtonPressed.Price);
			RegisterTower (newTower);
			GameManager.Instance.Audio.PlayOneShot (SoundManager.Instance.BuildTower, 0.05f);
		}
	}

	public void buyTower(int price)
	{
		GameManager.Instance.substractMoney (price);
	}

	public void selectTowerButton(TowerButton towerButton) 
	{
		
		if (towerButton.Price <= GameManager.Instance.TotalMoney) 
		{
			if (GameManager.Instance.CurrentGameState == gameState.build) {
				towerButtonPressed = towerButton;
				enableDragSprite (towerButtonPressed.DragSprite);
				freePositions = FindObjectsOfType<GameObject> ();
			} 


//			foreach (GameObject objectCurrent in freePositions) {
//				if (objectCurrent.tag == "BuildSite") {
//					Debug.Log ("Działa!");
//				}
//			}

		}else {
			towerButtonPressed = null;
		}

	} 

	public void followMouse()
	{
		transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector2 (transform.position.x, transform.position.y);
	}

	public void enableDragSprite(Sprite sprite)
	{
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}

	public void disableDragSprite()
	{
		spriteRenderer.enabled = false;
	}
}
