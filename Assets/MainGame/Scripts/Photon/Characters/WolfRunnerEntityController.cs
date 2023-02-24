using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class WolfRunnerEntityController : EntityBehaviour<IWolfRunner>
{
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        state.SetTransforms(state.WolfRunnerTransfrom, transform);
        state.AddAnimator(GetComponent<Animator>());
    }
}
