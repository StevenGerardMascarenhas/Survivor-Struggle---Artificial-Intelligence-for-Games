using UnityEngine;
using UnityEngine.UI;
//in this script we update the health bar images
public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = 1;
        totalhealthBar.fillOrigin = 1;
        currenthealthBar.fillOrigin = 1;
    }
    private void Update()
    {
        currenthealthBar.fillAmount = bossHealth.currentHealth / bossHealth.maxHealth;
    }
}