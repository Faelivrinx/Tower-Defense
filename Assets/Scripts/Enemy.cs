using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int target = 0;
	public Transform exitPoint;
	public Transform[] waypoints;
	public float navigationUpdate;

	[SerializeField]
	private float hp;

	[SerializeField]
	private int rewardAmount;

	private Transform enemy;
	private float navigationTime = 0;
	private Collider2D colliderEnemy;
	private Animator animator;
    private bool isEnable = true;

   public float Hp { get { return this.hp; } }


	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform> ();
		colliderEnemy = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
		GameManager.Instance.RegisterEnemy (this);
	}
	
	// Update is called once per frame
	void Update () {	
		if (!GameManager.Instance.checkGameOver()) {
			if (waypoints != null && isEnable) {
				navigationTime += Time.deltaTime;
				if (navigationTime > navigationUpdate) {
					if (target < waypoints.Length) {
						enemy.position = Vector2.MoveTowards (enemy.transform.position, waypoints [target].transform.position, navigationTime);
					} else {
						enemy.position = Vector2.MoveTowards (enemy.transform.position, exitPoint.transform.position, navigationTime);
					}

					navigationTime = 0;
				}
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Checkpoint") {
			target++;
		} else if (other.tag == "Finish") {
			GameManager.Instance.Escaped++;
			GameManager.Instance.TotalEscaped++;
			GameManager.Instance.UnregisterEnemy (this);
			GameManager.Instance.isWaveOver ();
		} else if(other.tag == "Projectile"){
			Projectile projectile = other.GetComponent<Projectile> ();
			takeDamage (projectile.Attack);
			Destroy (other.gameObject);
		}
	}

	public void takeDamage(int damage)
	{
		if (hp - damage > 0) {
			hp -= damage;	
			GameManager.Instance.Audio.PlayOneShot (SoundManager.Instance.Hit, .02f);
			animator.Play("Hurt");
		}
		else
		{
			animator.SetTrigger("didDie");
			GameManager.Instance.Audio.PlayOneShot (SoundManager.Instance.Death, .02f);
			die();
		}

	}

	public void die()
	{
		isEnable = false;
		colliderEnemy.enabled = false;
		GameManager.Instance.TotalKilled++;
		GameManager.Instance.addMoney (rewardAmount);
		GameManager.Instance.isWaveOver ();
	}

	public bool IsEnable
	{
		get
		{
			return isEnable;
		}
	}
}
