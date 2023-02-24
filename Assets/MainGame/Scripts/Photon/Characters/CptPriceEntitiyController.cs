using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class CptPriceEntitiyController : EntityBehaviour<ICptPrice>
{
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        state.SetTransforms(state.CptPriceTransform, transform);
        state.AddAnimator(GetComponent<Animator>());
    }
}
