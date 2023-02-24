using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class EagleEntitiyController : EntityBehaviour<IEagle>
{
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        state.SetTransforms(state.EagleTransform, transform);
        state.AddAnimator(GetComponent<Animator>());
    }
}
