using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class SkeletonArcherEntityController : EntityBehaviour<ISkeletonArcher>
{
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        state.SetTransforms(state.SkeletonArcherTransform, transform);
        state.AddAnimator(GetComponent<Animator>());
    }
}
