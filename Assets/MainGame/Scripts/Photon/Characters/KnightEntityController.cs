using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class KnightEntityController : EntityBehaviour<IKnight>
{
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        state.SetTransforms(state.KnightTransform, transform);
        state.AddAnimator(GetComponent<Animator>());
    }
}
