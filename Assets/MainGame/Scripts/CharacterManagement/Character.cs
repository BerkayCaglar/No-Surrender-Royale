using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterAI)), RequireComponent(typeof(CharacterAnimationController)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator)), RequireComponent(typeof(CharacterCollisionController)), RequireComponent(typeof(CharacterUIManager)), RequireComponent(typeof(CharacterHealthManager)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CharacterParticleSystemController))]
public class Character : MonoBehaviour
{
    // This is to store the character AI, character animation controller, nav mesh agent, animator, character collision controller, character UI manager, character health manager, rigidbody and character particle system controller
    private CharacterAI _characterAI;
    private CharacterAnimationController _characterAnimationController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private CharacterCollisionController _characterCollisionController;
    private CharacterUIManager _characterUIManager;
    private CharacterHealthManager _characterHealthManager;
    private Rigidbody _rigidbody;
    private CharacterParticleSystemController _characterParticleSystemController;

    // This is to store the character's name
    [SerializeField] private string _characterName;

    // This is to store the character's health, max health, attack range, attack damage, attack speed and detect range
    private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _detectRange;

    #region For Support Character
    [SerializeField] private float _buffSize;
    [SerializeField] private float _buffSpeed;
    [SerializeField] private float _buffAmount;

    #endregion


    #region Layers

    private int _enemyTowerLayer = 3;
    private int _playerTowerLayer = 10;
    private int _enemyLayer = 8;
    private int _playerLayer = 9;

    #endregion

    // This is to store the character's type
    public enum CharacterType { Melee, Ranged, AirUnit, Support };
    public CharacterType characterType;

    // This is to store the character's attack type
    public enum AttackType { Single, Multiple, None };
    public AttackType attackType;

    // This is to store the character's target type
    public enum TargetType { Air, Ground, Both, None };
    public TargetType targetType;

    // This is to store the character's speed type
    public enum CharacterSpeedType { Slow, Medium, Fast };
    public CharacterSpeedType characterSpeedType;

    // Getter for _characterName property
    public string CharacterName
    {
        get => _characterName;
    }

    // Getter for _enemyTowerLayer, _playerTowerLayer, _enemyLayer and _playerLayer property
    public int EnemyTowerLayer
    {
        get => _enemyTowerLayer;
    }
    public int PlayerTowerLayer
    {
        get => _playerTowerLayer;
    }
    public int EnemyLayer
    {
        get => _enemyLayer;
    }
    public int PlayerLayer
    {
        get => _playerLayer;
    }

    // Getter for _health, _maxHealth, _attackRange, _attackDamage, _attackSpeed, _detectRange, _buffSize, _buffSpeed and _buffAmount property
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

    #region Getters for Support Character
    public float BuffSize
    {
        get => _buffSize;
    }
    public float BuffSpeed
    {
        get => _buffSpeed;
    }
    public float BuffAmount
    {
        get => _buffAmount;
    }

    #endregion

    // Getter for _characterAI, _characterAnimationController, _navMeshAgent, _animator, _characterCollisionController, _characterUIManager, _characterHealthManager, _rigidbody and _characterParticleSystemController property
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
    public CharacterCollisionController CharacterCollisionController
    {
        get => _characterCollisionController;
    }
    public CharacterUIManager CharacterUIManager
    {
        get => _characterUIManager;
    }
    public CharacterHealthManager CharacterHealthManager
    {
        get => _characterHealthManager;
    }
    public Rigidbody Rigidbody
    {
        get => _rigidbody;
    }
    public CharacterParticleSystemController CharacterParticleSystemController
    {
        get => _characterParticleSystemController;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Get character AI, character animation controller, nav mesh agent, animator, character collision controller, character UI manager, character health manager, rigidbody and character particle system controller
        _characterAI = GetComponent<CharacterAI>();
        _characterAnimationController = GetComponent<CharacterAnimationController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _characterCollisionController = GetComponent<CharacterCollisionController>();
        _characterUIManager = GetComponent<CharacterUIManager>();
        _characterHealthManager = GetComponent<CharacterHealthManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _characterParticleSystemController = GetComponent<CharacterParticleSystemController>();

        // Set the character's health to max health
        Health = _maxHealth;

        // Set the character's speed based on the character's speed type
        switch (characterSpeedType)
        {
            case CharacterSpeedType.Slow:
                _navMeshAgent.speed = 2f;
                break;
            case CharacterSpeedType.Medium:
                _navMeshAgent.speed = 3f;
                break;
            case CharacterSpeedType.Fast:
                _navMeshAgent.speed = 4f;
                break;
        }
    }
}
