using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    private Character _character;
    private GameObject _currentTarget, _mainTarget;
    private bool _isAttacking;
    private float elapsed = 0.0f;
    private bool _targetInRange;
    private Collider[] _colliders = new Collider[0];

    private void Start()
    {
        _character = GetComponent<Character>();

        FindMainTarget();

        InvokeRepeating("FindMainTarget", 0f, 0.5f);

        InvokeRepeating("FindCurrentTarget", 0f, 0.1f);
    }

    private void FixedUpdate()
    {
        Attack();
        Move();
    }

    private void Move()
    {
        if (_currentTarget == null && _character != null && _mainTarget != null)
        {
            _character.NavMeshAgent.SetDestination(_mainTarget.transform.position + _mainTarget.transform.forward);

            elapsed += Time.deltaTime;
            if (elapsed > 1.0f)
            {
                elapsed -= 1.0f;
                NavMesh.CalculatePath(transform.position, _mainTarget.transform.position, NavMesh.AllAreas, _character.NavMeshAgent.path);
            }
        }
        else
        {
            if (!_targetInRange && !_isAttacking && _currentTarget != null)
            {
                // If the target is a tower, move to the tower's position + the tower's forward direction
                if (_currentTarget.layer == _character.EnemyTowerLayer || _currentTarget.layer == _character.PlayerTowerLayer)
                {
                    _character.NavMeshAgent.SetDestination(_currentTarget.transform.position + _currentTarget.transform.forward);
                    return;
                }
                _character.NavMeshAgent.SetDestination(_currentTarget.transform.position);
            }
        }
    }

    private void FindCurrentTarget()
    {
        // If the current target is not null, return. This is to prevent the character from finding a new target while attacking
        if (_currentTarget != null) return;

        if (CompareTag("Enemy"))
        {
            _colliders = Physics.OverlapSphere(transform.position, _character.DetectRange, LayerMask.GetMask("Friendly", "PlayerTowers"));
        }
        else
        {
            _colliders = Physics.OverlapSphere(transform.position, _character.DetectRange, LayerMask.GetMask("Enemy", "EnemyTowers"));
        }

        foreach (Collider collider in _colliders)
        {
            // If the collider is not a tower
            if (collider.gameObject.layer != _character.EnemyTowerLayer && collider.gameObject.layer != _character.PlayerTowerLayer)
            {
                // If the collider is an air unit and the character is a melee unit, continue
                if (_character.characterType == Character.CharacterType.Melee && collider.GetComponent<Character>().characterType == Character.CharacterType.AirUnit) continue;
            }

            GameObject _nearestTarget = null;
            float _nearestDistance = float.MaxValue;

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < _nearestDistance)
            {
                _nearestDistance = distance;
                _nearestTarget = collider.gameObject;
            }
            _currentTarget = _nearestTarget;

            return;
        }

        _currentTarget = null;
    }

    private void FindMainTarget()
    {
        GameObject _nearestTower = null;
        float _nearestDistance = float.MaxValue;

        if (CompareTag("Enemy"))
        {
            foreach (GameObject tower in TowerManager.Instance.PlayerTowers)
            {
                float distance = Vector3.Distance(transform.position, tower.transform.position);
                if (distance < _nearestDistance)
                {
                    _nearestDistance = distance;
                    _nearestTower = tower;
                }
            }
            _mainTarget = _nearestTower;
        }
        else
        {
            foreach (GameObject tower in TowerManager.Instance.EnemyTowers)
            {
                float distance = Vector3.Distance(transform.position, tower.transform.position);
                if (distance < _nearestDistance)
                {
                    _nearestDistance = distance;
                    _nearestTower = tower;
                }
            }
            _mainTarget = _nearestTower;
        }
    }
    private void Attack()
    {
        if (_currentTarget == null) return;

        if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= _character.AttackRange && !_isAttacking)
        {
            _targetInRange = true;
            _character.NavMeshAgent.ResetPath();
            StartCoroutine(AttackCoroutine());
        }
        else
        {
            _targetInRange = false;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true;
        _character.CharacterAnimationController.Attack();

        _character.NavMeshAgent.ResetPath();
        transform.LookAt(_currentTarget.transform);

        yield return new WaitForSeconds(_character.AttackSpeed);

        _isAttacking = false;

        if (_currentTarget == null) yield break;

        if (_currentTarget.layer == _character.EnemyTowerLayer || _currentTarget.layer == _character.PlayerTowerLayer)
        {
            _currentTarget.GetComponent<Tower>().Health -= _character.AttackDamage;
            yield break;
        }

        _currentTarget.GetComponent<Character>().Health -= _character.AttackDamage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Character>().DetectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Character>().AttackRange);
    }
}
