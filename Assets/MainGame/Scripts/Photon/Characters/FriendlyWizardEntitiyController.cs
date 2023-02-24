using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class FriendlyWizardEntitiyController : EntityBehaviour<IFriendlyWizard>
{
    public override void Attached()
    {
        if (!entity.IsOwner) return;

        state.SetTransforms(state.FriendlyWizardTransform, transform);
        state.AddAnimator(GetComponent<Animator>());

    }
}
