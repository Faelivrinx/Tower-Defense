using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

	[SerializeField]
	private AudioClip menuMusic;

	[SerializeField]
	private AudioClip backgroundMusic;

	[SerializeField]
	private AudioClip arrow;

	[SerializeField]
	private AudioClip death;

	[SerializeField]
	private AudioClip fireball;

	[SerializeField]
	private AudioClip gameOver;

	[SerializeField]
	private AudioClip hit;

	[SerializeField]
	private AudioClip buildTower;

	[SerializeField]
	private AudioClip buildWoodTower;

	[SerializeField]
	private AudioClip buildMageTower;

	public AudioClip Arrow{ get {return arrow; } }

	public AudioClip Death{ get {return death; } }

	public AudioClip FireBall{ get {return fireball; } }

	public AudioClip Hit{ get {return hit; } }

	public AudioClip BuildTower{ get {return buildTower; } }

	public AudioClip BuildWoodTower{ get {return buildWoodTower; } }

	public AudioClip BuildMageTower{ get {return buildMageTower; } }
}
