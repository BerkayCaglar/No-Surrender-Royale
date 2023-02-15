using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterUIManager : MonoBehaviour
{
    private Character _character;
    [SerializeField] private GameObject _healthBar;
    private Slider _healthBarSlider;

    private void Start()
    {
        _character = GetComponent<Character>();
        _healthBarSlider = _healthBar.GetComponent<Slider>();

        _healthBarSlider.maxValue = _character.MaxHealth;
    }
    private void Update()
    {
        SetHealthOnBar();

        _healthBar.transform.rotation = Quaternion.LookRotation(_healthBar.transform.position - Camera.main.transform.position);
    }

    private void SetHealthOnBar()
    {
        _healthBarSlider.value = _character.Health;
    }
}
