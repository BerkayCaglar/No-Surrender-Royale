using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerMainTarget"))
        {
            Destroy(gameObject);
        }
    }
}
