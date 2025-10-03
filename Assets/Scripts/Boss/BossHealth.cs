using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BossHealth : MonoBehaviour
{

	public int maxHealth = 500;

    public GameObject deathChunkParticle;
    public GameObject deathBloodParticle;

    public bool isInvulnerable = false;
    public float currentHealth { get; private set; }

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
	{
		if (isInvulnerable)
			return;

		currentHealth -= damage;
		Debug.Log(currentHealth);

		if (currentHealth <= 200)
		{
			GetComponent<Animator>().SetBool("IsEnraged", true);
		}

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void Die()
    {
        GameObject.Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GameObject.Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Destroy(gameObject);
    }

}
