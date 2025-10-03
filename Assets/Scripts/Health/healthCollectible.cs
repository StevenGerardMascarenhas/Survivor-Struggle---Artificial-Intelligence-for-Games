using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private PlayerStats playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(playerHealth.currentHealth < playerHealth.maxHealth)
            {
                playerHealth.IncreaseHealth(healthValue);
                gameObject.SetActive(false);
            }
        }
    }
}