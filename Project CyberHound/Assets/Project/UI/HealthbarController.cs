using Project.Game_Entities.Enemies;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        var damageTaker = GetComponentInParent<DamageTaker>();
        slider.maxValue = damageTaker.maxHeath;
        slider.value = damageTaker.currentHealth;
        slider.minValue = 0;
        
        damageTaker.OnHit.AddListener(OnHealthChange);
    }

    private void OnHealthChange(int currentValue)
    {
        slider.value = currentValue;
    }
}
