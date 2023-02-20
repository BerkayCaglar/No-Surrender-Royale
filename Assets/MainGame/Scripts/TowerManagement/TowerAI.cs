using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour
{
    private Tower _tower;

    private bool _isAttacking;

    private GameObject _nearestEnemy, _currentEnemy;
    private float _nearestEnemyDistance = float.MaxValue;

    List<Collider> _colliders = new List<Collider>();

    private void Start()
    {
        _tower = GetComponent<Tower>();
    }
    private void FixedUpdate()
    {
        FindTarget();
        LookAtTarget();
        Attack();
    }
    private void LookAtTarget()
    {
        if (_currentEnemy != null)
        {
            Vector3 direction = _currentEnemy.transform.position - _tower.TowerHead.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(_tower.TowerHead.transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            _tower.TowerHead.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        else
        {
            if (gameObject.layer == TowerManager.Instance.EnemyTowerLayer)
            {
                _tower.TowerHead.transform.rotation = Quaternion.Lerp(_tower.TowerHead.transform.rotation, Quaternion.Euler(0f, 180f, 0f), Time.deltaTime * 10f);
            }
            else if (gameObject.layer == TowerManager.Instance.PlayerTowerLayer)
            {
                _tower.TowerHead.transform.rotation = Quaternion.Lerp(_tower.TowerHead.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 10f);
            }
        }
    }

    private void FindTarget()
    {
        if (_currentEnemy != null) return;

        _colliders.Clear();

        if (gameObject.layer == TowerManager.Instance.EnemyTowerLayer)
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _tower.DetectRange, LayerMask.GetMask("Friendly")));
        }
        else if (gameObject.layer == TowerManager.Instance.PlayerTowerLayer)
        {
            _colliders.AddRange(Physics.OverlapSphere(transform.position, _tower.DetectRange, LayerMask.GetMask("Enemy")));
        }

        if (_colliders.Count > 0)
        {
            foreach (Collider collider in _colliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < _nearestEnemyDistance)
                {
                    _nearestEnemyDistance = distance;
                    _nearestEnemy = collider.gameObject;
                }
                _currentEnemy = _nearestEnemy;
            }
        }
    }
    private void Attack()
    {
        if (_currentEnemy != null && !_isAttacking)
        {
            if (Vector3.Distance(transform.position, _currentEnemy.transform.position) <= _tower.AttackRange)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true;

        yield return new WaitForSeconds(_tower.AttackSpeed);

        _isAttacking = false;

        if (_currentEnemy == null) yield break;

        _tower.TowerAnimationController.PlayAttackAnimation();

        _tower.TowerParticleSystemController.PlayAttackParticleSystem(_currentEnemy);

        if (_tower.towerAttackType == Tower.TowerAttackType.Multiple)
        {
            List<Collider> enemiesAtAttackPoint = new List<Collider>();

            if (gameObject.layer == TowerManager.Instance.EnemyTowerLayer)
            {
                enemiesAtAttackPoint.AddRange(Physics.OverlapSphere(_currentEnemy.transform.position, 3f, LayerMask.GetMask("Friendly")));
            }
            else if (gameObject.layer == TowerManager.Instance.PlayerTowerLayer)
            {
                enemiesAtAttackPoint.AddRange(Physics.OverlapSphere(_currentEnemy.transform.position, 3f, LayerMask.GetMask("Enemy")));
            }

            enemiesAtAttackPoint.RemoveAll(x => x.gameObject == _currentEnemy);

            foreach (Collider enemy in enemiesAtAttackPoint)
            {
                enemy.GetComponent<Character>().Health -= _tower.MultipleAttackDamage;
            }
        }

        float remainingHealth = _currentEnemy.GetComponent<Character>().Health - _tower.AttackDamage;

        if (remainingHealth <= 0f)
        {
            _currentEnemy.GetComponent<Character>().Health -= _tower.AttackDamage;

            _currentEnemy = null;
            _nearestEnemyDistance = float.MaxValue;

            yield break;
        }
        _currentEnemy.GetComponent<Character>().Health -= _tower.AttackDamage;
    }

}
