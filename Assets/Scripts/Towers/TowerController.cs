using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

	[SerializeField]
	private float timeBetweenAttacks;

	[SerializeField]
	private float attackRadius;
	[SerializeField]
	private Projectile projectile;

    [SerializeField]
	private Enemy target = null;

    private float attackCounter;
	private bool haveATarget = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (haveATarget) {
			Debug.Log ("Mam target!");
		}
		attackCounter -= Time.deltaTime;
			
		if (target == null || !target.IsEnable) {
			Enemy enemyTarget = NearestEnemy ();
			if (enemyTarget != null && Vector2.Distance (transform.localPosition, enemyTarget.transform.localPosition) <= attackRadius) {
				target = enemyTarget;
			}
		}
		else 
		{
			if (attackCounter <= 0)
			{
				haveATarget = true;
				attackCounter = timeBetweenAttacks;
			} 
			else
			{
				//haveATarget = false;	
			} 
			if (Vector2.Distance (transform.localPosition, target.transform.localPosition) > attackRadius) 
			{
				target = null;	
			}	
		}
	}

	void FixedUpdate()
	{
		if (haveATarget) 
		{
			Attack ();
		}
	}

	public void Attack ()
	{
		haveATarget = false;
		Projectile newProjectile = Instantiate (projectile) as Projectile;
		newProjectile.transform.localPosition = transform.localPosition;

		playSoundAttack (newProjectile);

		StartCoroutine(MoveProjectileToEnemy(newProjectile));

	}

	private void playSoundAttack(Projectile projectile)
	{
		if (projectile.ProjectileType == ProjectileType.arrow || projectile.ProjectileType == ProjectileType.bigArrow) {
			GameManager.Instance.Audio.PlayOneShot (SoundManager.Instance.Arrow, .01f);
		} else if (projectile.ProjectileType == ProjectileType.magicShot) {
			GameManager.Instance.Audio.PlayOneShot (SoundManager.Instance.FireBall, .02f);
		}
	}

	IEnumerator MoveProjectileToEnemy(Projectile projectile)
	{
       

		Enemy actualTarget = target;

		while (getTargetDistance (target) > 0.2f && projectile != null) 
		{
			var distance = actualTarget.transform.localPosition - transform.localPosition;
			var angle = Mathf.Atan2 (distance.y, distance.x) * Mathf.Rad2Deg;
			projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			projectile.transform.localPosition = Vector2.MoveTowards (projectile.transform.localPosition, actualTarget.transform.localPosition, 5f * Time.deltaTime);
            if (!actualTarget.IsEnable) {
                Destroy(projectile.gameObject);
                yield return null;
            }

            yield return null;

		} 

		if (projectile != null && !target.IsEnable) 
		{
			Debug.Log ("Usuwam!");
			Destroy (projectile.gameObject);
		}
         
    }

	private float getTargetDistance(Enemy enemy)
	{
		if (enemy == null) 
		{
			enemy = NearestEnemy ();
			if (enemy == null) 
			{
				return 0f;
			}
		}
		return Mathf.Abs (Vector2.Distance (transform.localPosition, enemy.transform.localPosition));
	}

	private List<Enemy> GetEnemiesInRange()
	{
		List<Enemy> enemies = new List<Enemy> ();

		foreach(Enemy enemy in GameManager.Instance.EnemyList)
		{
			if (Vector2.Distance (transform.localPosition, enemy.transform.localPosition) <= attackRadius) 
			{
				enemies.Add (enemy);
			}
		}

		return enemies;
	}

	private Enemy NearestEnemy()
	{
		Enemy target = null;
		float smallestDistance = float.PositiveInfinity;

		foreach (Enemy enemy in GameManager.Instance.EnemyList)
		{
			if (Vector2.Distance (transform.localPosition, enemy.transform.localPosition) < smallestDistance) 
			{
				smallestDistance = Vector2.Distance (transform.localPosition, enemy.transform.localPosition);
				target = enemy;
			}	
		}

		return target;
	}
}
