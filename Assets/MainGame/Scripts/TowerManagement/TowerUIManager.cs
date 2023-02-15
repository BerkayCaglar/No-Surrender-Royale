using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerUIManager : MonoBehaviour
{
    private Tower _tower;
    [SerializeField] private GameObject _healthBar;
    private Slider _healthBarSlider;

    private void Start()
    {
        _tower = GetComponent<Tower>();
        _healthBarSlider = _healthBar.GetComponent<Slider>();

        _healthBarSlider.maxValue = _tower.MaxHealth;
    }
    private void Update()
    {
        SetHealthOnBar();
    }

    private void SetHealthOnBar()
    {
        _healthBarSlider.value = _tower.Health;
    }
}
