using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParticleSystemController : MonoBehaviour
{
    private Tower _tower;
    private void Start()
    {
        _tower = GetComponent<Tower>();
    }
    public void PlayAttackParticleSystem(GameObject target)
    {
        ParticleSystem attackParticleSystem = Instantiate(_tower.AttackParticleSystem, target.transform.position, Quaternion.identity);

        attackParticleSystem.Play();

        Destroy(attackParticleSystem.gameObject, 1f);

        //_tower.AttackParticleSystem.Play();
    }
    public void PlayDeathParticleSystem()
    {
        _tower.DeathParticleSystem.transform.parent = null;
        _tower.DeathParticleSystem.transform.position = _tower.transform.position;
        _tower.DeathParticleSystem.Play();

        Destroy(_tower.DeathParticleSystem.gameObject, 2f);
    }
}
