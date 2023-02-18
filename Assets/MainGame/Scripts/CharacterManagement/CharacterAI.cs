using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    private Character _character;
    private GameObject _currentTarget, _mainTarget;
    private bool _isAttacking, _isHealing;
    private float elapsed = 0.0f;
    private bool _targetInRange;
    private List<Collider> _colliders = new List<Collider>();
    private GameObject _nearestTarget = null;
    private float _nearestDistance = Mathf.Infinity;

    private void Start()
    {
        _character = GetComponent<Character>();

        if (_character.characterType == Character.CharacterType.Support)
        {
            FindNearlyFriendTarget();
            InvokeRepeating("FindNearlyFriendTarget", 0f, 0.5f);
            return;
        }
        else
        {
            OtherFindMainTarget();
            InvokeRepeating("OtherFindMainTarget", 0f, 1f);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance._gameState != GameManager.GameState.Game) return;

        if (_character.characterType == Character.CharacterType.Support)
        {
            // If the character is a support character
            FindFriendlyTargetsAndHealThem();
            SupportMove();
        }
        else
        {
            // If the character is not a support character
            OtherFindCurrentTarget();
            OtherAttack();
            OtherMove();
        }

    }

    #region Support Character

    private void FindNearlyFriendTarget()
    {
        if (_colliders.Count == 0 || _currentTarget != null) return;

        _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.DetectRange, LayerMask.GetMask("Friendly")));

        foreach (Collider collider in _colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);

            if (distance < _nearestDistance)
            {
                _nearestDistance = distance;
                _nearestTarget = collider.gameObject;
            }
        }
        Debug.Log(_nearestTarget.name);
        _currentTarget = _nearestTarget;
    }
    private void SupportMove()
    {
        if (_currentTarget != null && !_isHealing)
        {
            _character.NavMeshAgent.SetDestination(_currentTarget.transform.position);
        }
    }
    private void FindFriendlyTargetsAndHealThem()
    {
        // If the character is already healing, return
        if (_isHealing) return;

        _colliders.Clear();

        // Find all friendly targets in range
        _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.DetectRange, LayerMask.GetMask("Friendly")));

        // Remove the this character from the list
        _colliders.RemoveAll(collider => collider.gameObject == gameObject);

        // If there are no targets in range, return
        if (_colliders.Count == 0) return;

        if (!_isHealing)
        {
            StartCoroutine(Heal());
        }
    }

    private IEnumerator Heal()
    {
        _isHealing = true;

        _character.NavMeshAgent.ResetPath();

        yield return new WaitForSeconds(_character.BuffSpeed);

        foreach (Collider collider in _colliders)
        {
            Character otherCharacter = collider.GetComponent<Character>();

            if (otherCharacter != null)
            {
                otherCharacter.Health += _character.BuffAmount;
            }
        }
        _character.CharacterAnimationController.Heal();
        _isHealing = false;
    }

    #endregion

    #region Other Characters

    private void OtherMove()
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

    private void OtherFindCurrentTarget()
    {
        // If the current target is not null, return. This is to prevent the character from finding a new target while attacking
        if (_currentTarget != null) return;

        _colliders.Clear();

        if (gameObject.layer == _character.EnemyLayer)
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.DetectRange, LayerMask.GetMask("Friendly", "PlayerTowers")));
        }
        else
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.DetectRange, LayerMask.GetMask("Enemy", "EnemyTowers")));
        }

        if (_colliders.Count == 0) return;

        _nearestTarget = null;
        _nearestDistance = float.MaxValue;

        foreach (Collider collider in _colliders)
        {
            if (collider.gameObject == gameObject) continue;

            // If the collider is not a tower
            if (collider.gameObject.layer != _character.EnemyTowerLayer && collider.gameObject.layer != _character.PlayerTowerLayer)
            {
                // Get the collider's character type
                Character.CharacterType _colliderCharacterType = collider.GetComponent<Character>().characterType;

                // If the collider is an air unit and the character target type is ground, continue
                if (_character.targetType == Character.TargetType.Ground && _colliderCharacterType == Character.CharacterType.AirUnit) continue;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < _nearestDistance)
            {
                _nearestDistance = distance;
                _nearestTarget = collider.gameObject;
            }
            _currentTarget = _nearestTarget;

        }
    }

    private void OtherFindMainTarget()
    {
        if (GameManager.Instance._gameState != GameManager.GameState.Game) return;

        GameObject _nearestTower = null;
        float _nearestDistance = float.MaxValue;

        if (gameObject.layer == _character.EnemyLayer)
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
    private void OtherAttack()
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

    #endregion


    #region Gizmos and Debug

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Character>().DetectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Character>().AttackRange);
    }

    #endregion
}
