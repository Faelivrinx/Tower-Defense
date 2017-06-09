using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour {

	[SerializeField]
	private TowerController tower;

	[SerializeField]
	private Sprite dragSprite;

	[SerializeField]
	private int price;

	public TowerController Tower
	{
		get
		{
			return tower;
		}
	}

	public Sprite DragSprite
	{
		get 
		{
			return dragSprite;
		}	
	}

	public int Price 
	{
		get 
		{
			return price;
		}
	}
}
