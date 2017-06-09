using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour {

	[SerializeField]
	private int attack;

	[SerializeField]
	private ProjectileType projectileType;

	public int Attack 
	{
		get 
		{
			return attack;
		}
	}

	public ProjectileType ProjectileType
	{
		get 
		{
			return projectileType;
		}	
	}
}

public enum ProjectileType
{
	arrow, bigArrow, magicShot
};
