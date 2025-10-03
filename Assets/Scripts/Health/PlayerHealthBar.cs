using UnityEngine;
using UnityEngine.UI;
//in this script we update the health bar images
public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private PlayerStats playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = 1;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}