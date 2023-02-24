using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParticleSystemController : MonoBehaviour
{
    private ParticleSystem _healParticleSystem;

    private void Start()
    {
        _healParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayHealParticleSystem()
    {
        _healParticleSystem.Play();
    }
}
