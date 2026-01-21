using TMPro;
using UnityEngine;
using VContainer;

public class HealthView : MonoBehaviour
{
    [field: SerializeField] private TMP_Text _healthText;

    public void SetHealth(int health, int maxHealth)
    {
        _healthText.text = $"{health}/{maxHealth}";
    }
}