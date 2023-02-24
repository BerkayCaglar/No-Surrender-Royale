using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class EnemyWizardEntityController : EntityBehaviour<IEnemyWizard>
{
    private Character _character;
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        _character = GetComponent<Character>();

        state.SetTransforms(state.EnemyWizardTransform, transform);
        state.AddAnimator(GetComponent<Animator>());
    }
}
