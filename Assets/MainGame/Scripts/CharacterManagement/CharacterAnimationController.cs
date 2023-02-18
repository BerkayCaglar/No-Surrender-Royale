using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Character _character;

    private void Start()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        MoveAnimator();
    }

    private void MoveAnimator()
    {
        _character.Animator.SetFloat("Speed", _character.NavMeshAgent.velocity.magnitude);
    }
    public void Attack()
    {
        _character.Animator.SetTrigger("Attack");
    }
    public void Heal()
    {
        _character.Animator.SetTrigger("Heal");
    }
}
