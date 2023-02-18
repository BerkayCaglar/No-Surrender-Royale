using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthManager : MonoBehaviour
{
    private Character _character;

    private void Start()
    {
        _character = GetComponent<Character>();
    }
    private void Update()
    {
        if (_character.Health <= 0f)
        {
            Transform _parentTransform = transform.parent;
            if (_parentTransform != null)
            {
                if ((_parentTransform.CompareTag("Knights") || _parentTransform.CompareTag("SkeletonWarriors")) && _parentTransform.childCount == 1)
                {
                    Destroy(_parentTransform.gameObject);
                    return;
                }
            }
            Destroy(gameObject);
        }
    }
}
