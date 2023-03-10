using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Bolt;

public class CharacterAI : EntityBehaviour
{

    private Character _character;

    private GameObject _currentTarget, _mainTarget;
    private bool _isAttacking, _isHealing, _isHealingCooldown;
    private float elapsed = 0.0f;
    private bool _targetInRange;
    private List<Collider> _colliders = new List<Collider>();
    private GameObject _nearestTarget = null;
    private float _nearestDistance = Mathf.Infinity;

    private void Start()
    {
        _character = GetComponent<Character>();

        if (_character.Card.characterType == Cards.CharacterType.Support)
        {
            SupportFindNearlyFriendTarget();
            InvokeRepeating("SupportFindNearlyFriendTarget", 0f, 0.5f);
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

        if (_character.Card.characterType == Cards.CharacterType.Support)
        {
            // If the character is a support character
            SupportFindFriendlyTargetsAndHealThem();
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

    private void SupportFindNearlyFriendTarget()
    {
        _colliders.Clear();

        // Check this character is friendly or enemy. If friendly, find friendly targets in range. If enemy, find enemy targets in range.
        if (gameObject.layer == _character.PlayerLayer)
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.Card.DetectRange, LayerMask.GetMask("Friendly")));
        }
        else if (gameObject.layer == _character.EnemyLayer)
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.Card.DetectRange, LayerMask.GetMask("Enemy")));
        }

        _colliders.RemoveAll(collider => collider.gameObject == gameObject);

        if (_colliders.Count == 0) return;

        foreach (Collider collider in _colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);

            if (distance < _nearestDistance)
            {
                _nearestDistance = distance;
                _nearestTarget = collider.gameObject;
            }
        }
        _currentTarget = _nearestTarget;
    }
    private void SupportMove()
    {
        if (_currentTarget != null)
        {
            _character.NavMeshAgent.SetDestination(_currentTarget.transform.position);
        }
    }
    private void SupportFindFriendlyTargetsAndHealThem()
    {
        // If there are no targets in range or the character is already healing, return
        if (_colliders.Count == 0) return;

        if (!_isHealing && !_isHealingCooldown)
        {
            StartCoroutine(SupportHeal());
        }
    }
    private IEnumerator SupportHeal()
    {
        _isHealing = true;

        _character.NavMeshAgent.ResetPath();

        _character.CharacterAnimationController.HealPrepare();

        yield return new WaitForSeconds(_character.Card.BuffSpeed);

        _character.CharacterAnimationController.Heal();

        _colliders.Clear();

        // Check this character is friendly or enemy. If friendly, find friendly targets in range. If enemy, find enemy targets in range.
        if (gameObject.layer == _character.PlayerLayer)
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.Card.DetectRange, LayerMask.GetMask("Friendly")));
        }
        else if (gameObject.layer == _character.EnemyLayer)
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.Card.DetectRange, LayerMask.GetMask("Enemy")));
        }

        foreach (Collider collider in _colliders)
        {
            Character otherCharacter = collider.GetComponent<Character>();

            if (otherCharacter != null)
            {
                otherCharacter.Health += _character.Card.BuffAmount;
                otherCharacter.CharacterParticleSystemController.PlayHealParticleSystem();
            }
        }
        _isHealing = false;
        StartCoroutine(SupportHealCooldown());
    }
    private IEnumerator SupportHealCooldown()
    {
        _isHealingCooldown = true;
        yield return new WaitForSeconds(4);
        _isHealingCooldown = false;
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
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.Card.DetectRange, LayerMask.GetMask("Friendly", "PlayerTowers")));
        }
        else
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _character.Card.DetectRange, LayerMask.GetMask("Enemy", "EnemyTowers")));
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
                Cards.CharacterType _colliderCharacterType = collider.GetComponent<Character>().Card.characterType;

                // If the collider is an air unit and the character target type is ground, continue
                if (_character.Card.targetType == Cards.TargetType.Ground && _colliderCharacterType == Cards.CharacterType.AirUnit) continue;
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

        if (Vector3.Distance(transform.position, _currentTarget.transform.position) <= _character.Card.AttackRange && !_isAttacking)
        {
            _targetInRange = true;
            _character.NavMeshAgent.ResetPath();
            StartCoroutine(OtherAttackCoroutine());
        }
        else
        {
            _targetInRange = false;
        }
    }
    private IEnumerator OtherAttackCoroutine()
    {
        _isAttacking = true;
        _character.CharacterAnimationController.Attack();

        _character.NavMeshAgent.ResetPath();
        transform.LookAt(_currentTarget.transform);

        yield return new WaitForSeconds(_character.Card.AttackSpeed);

        _isAttacking = false;

        if (_currentTarget == null) yield break;

        if (_currentTarget.layer == _character.EnemyTowerLayer || _currentTarget.layer == _character.PlayerTowerLayer)
        {
            _currentTarget.GetComponent<Tower>().Health -= _character.Card.AttackDamage;
            yield break;
        }

        _currentTarget.GetComponent<Character>().Health -= _character.Card.AttackDamage;
    }

    #endregion

    #region Gizmos and Debug

    private void OnDrawGizmosSelected()
    {
        /*
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Character>().Card.DetectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Character>().Card.AttackRange);
        */
    }

    #endregion

}
