using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class CharacterHealthManager : EntityBehaviour
{
    private Character _character;
    private bool _isDead;
    private void Start()
    {
        _character = GetComponent<Character>();
    }
    private void Update()
    {
        if (!BoltNetwork.IsServer || !entity.IsOwner) return;

        if (_character.Health <= 0f && !_isDead)
        {
            _isDead = true;
            Transform _parentTransform = transform.parent;
            if (_parentTransform != null)
            {
                if ((_parentTransform.CompareTag("Knights") || _parentTransform.CompareTag("SkeletonWarriors")) && _parentTransform.childCount == 1)
                {
                    BoltNetwork.Destroy(_parentTransform.gameObject);
                    return;
                }
            }

            var evnt = DestroyObjectEvent.Create();
            evnt.NetworkIDToDestroy = GetComponent<BoltEntity>().NetworkId;
            evnt.Send();

            //BoltNetwork.Destroy(gameObject);
        }
    }

}
