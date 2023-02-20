using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerUIManager)), RequireComponent(typeof(TowerAnimationController)), RequireComponent(typeof(TowerHealthManager)), RequireComponent(typeof(TowerAI)), RequireComponent(typeof(Animator)), RequireComponent(typeof(TowerParticleSystemController))]
public class Tower : MonoBehaviour
{
    private TowerAnimationController _towerAnimationController;
    private TowerHealthManager _towerHealthManager;
    private TowerUIManager _towerUIManager;
    private TowerAI _towerAI;
    private TowerParticleSystemController _towerParticleSystemController;
    private Animator _animator;

    [SerializeField] private GameObject _towerHead;
    [SerializeField] private GameObject _towerBody;

    private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _detectRange;
    [SerializeField] private float _multipleAttackDamage;

    public enum TowerAttackType
    {
        Single,
        Multiple
    }
    public TowerAttackType towerAttackType;

    [SerializeField] private ParticleSystem _attackParticleSystem;
    [SerializeField] private ParticleSystem _deathParticleSystem;

    // Getter and Setter for _health, _maxHealth, _attackRange, _attackDamage, _attackSpeed, _detectRange property
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
    public float MultipleAttackDamage
    {
        get => _multipleAttackDamage;
    }
    // Getter for _attackParticleSystem and _deathParticleSystem property
    public ParticleSystem AttackParticleSystem
    {
        get => _attackParticleSystem;
    }
    public ParticleSystem DeathParticleSystem
    {
        get => _deathParticleSystem;
    }

    // Getter for _towerAnimationController, _towerHealthManager, _towerUIManager, _towerAI, _animator, _towerParticleSystemController property
    public TowerAnimationController TowerAnimationController
    {
        get => _towerAnimationController;
    }
    public TowerHealthManager TowerHealthManager
    {
        get => _towerHealthManager;
    }
    public TowerUIManager TowerUIManager
    {
        get => _towerUIManager;
    }
    public TowerAI TowerAI
    {
        get => _towerAI;
    }
    public TowerParticleSystemController TowerParticleSystemController
    {
        get => _towerParticleSystemController;
    }
    public Animator Animator
    {
        get => _animator;
    }

    // Getter for _towerHead and _towerBody property
    public GameObject TowerHead
    {
        get => _towerHead;
    }
    public GameObject TowerBody
    {
        get => _towerBody;
    }
    private void Start()
    {
        // Get all the components
        _towerAnimationController = GetComponent<TowerAnimationController>();
        _towerHealthManager = GetComponent<TowerHealthManager>();
        _towerUIManager = GetComponent<TowerUIManager>();
        _towerAI = GetComponent<TowerAI>();
        _towerParticleSystemController = GetComponent<TowerParticleSystemController>();
        _animator = GetComponent<Animator>();

        // Set the health to max health
        Health = MaxHealth;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
