using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class SkeletonWarriorEntitiyController : EntityBehaviour<ISkeletonWarrior>
{
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        state.SetTransforms(state.SkeletonWarriorTransform, transform);
        state.SetAnimator(GetComponent<Animator>());
    }
}
