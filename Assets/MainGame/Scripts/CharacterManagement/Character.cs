using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterAI)), RequireComponent(typeof(CharacterAnimationController)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator)), RequireComponent(typeof(CharacterCollisionController)), RequireComponent(typeof(CharacterUIManager))]
public class Character : MonoBehaviour
{
    // This is to store the character AI, character animation controller and navmesh agent component
    private CharacterAI _characterAI;
    private CharacterAnimationController _characterAnimationController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    // This is to store the character's health, max health, attack range, attack damage, attack speed and detect range
    private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _detectRange;

    private int _enemyTowerLayer = 3;
    private int _playerTowerLayer = 10;

    // This is to store the character's type
    public enum CharacterType { Melee, Ranged, AirUnit };
    public CharacterType characterType;

    // Getter for _enemyTowerLayer and _playerTowerLayer property
    public int EnemyTowerLayer
    {
        get => _enemyTowerLayer;
    }
    public int PlayerTowerLayer
    {
        get => _playerTowerLayer;
    }

    // Getter for _health, _maxHealth, _attackRange, _attackDamage and _detectRange property
    public float Health
    {
        get => _health;
        set { _health = Mathf.Clamp(value, 0f, _maxHealth); }
    }
    public float MaxHealth
    {
        get => _maxHealth;
    }
    public float AttackRange
    {
        get => _attackRange;
    }
    public float AttackDamage
    {
        get => _attackDamage;
    }
    public float DetectRange
    {
        get => _detectRange;
    }
    public float AttackSpeed
    {
        get => _attackSpeed;
    }
    // Getter for _characterAI, _characterAnimationController and _navMeshAgent property
    public CharacterAI CharacterAI
    {
        get => _characterAI;
    }
    public CharacterAnimationController CharacterAnimationController
    {
        get => _characterAnimationController;
    }
    public NavMeshAgent NavMeshAgent
    {
        get => _navMeshAgent;
    }
    public Animator Animator
    {
        get => _animator;
    }

    private void Update()
    {
        if (Health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Get character AI, character animation controller and navmesh agent component
        _characterAI = GetComponent<CharacterAI>();
        _characterAnimationController = GetComponent<CharacterAnimationController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        // Set the character's health to max health
        Health = _maxHealth;
    }
}
