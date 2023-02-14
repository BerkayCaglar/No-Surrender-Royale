using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    private Character _character;
    private GameObject _currentTarget;
    private bool _isAttacking;
    private void Start()
    {
        _character = GetComponent<Character>();

        if (CompareTag("Enemy")) return;

        InvokeRepeating("FindTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (CompareTag("Enemy")) return;

        Move();
    }

    private void Move()
    {
        if (_currentTarget == null)
        {
            _character.NavMeshAgent.SetDestination(_character.MainTarget.transform.position);
            //_character.CharacterAnimationController.Move();
        }
        else
        {
            if (_character.NavMeshAgent.hasPath)
            {
                _character.NavMeshAgent.ResetPath();
            }
            transform.LookAt(_currentTarget.transform);
        }
    }

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _character.DetectRange, LayerMask.GetMask("Enemy"));
        foreach (Collider collider in colliders)
        {
            _currentTarget = collider.gameObject;
            Attack();

            return;
        }
        _currentTarget = null;
    }

    private void Attack()
    {
        if (_currentTarget != null && !_isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true;
        _character.CharacterAnimationController.Attack();
        yield return new WaitForSeconds(_character.AttackSpeed);

        if (_currentTarget == null) yield break;

        _currentTarget.GetComponent<Character>().Health -= _character.AttackDamage;
        _isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Character>().DetectRange);
    }
}
