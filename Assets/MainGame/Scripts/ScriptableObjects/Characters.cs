using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "ScriptableObjects/Characters", order = 1)]
public class Characters : ScriptableObject
{
    /// <summary>
    /// List of characters. This list contains all characters in the game. Example: SkeletonArcher, SkeletonWarrior, Knight, Eagle etc.
    /// </summary>
    public List<GameObject> _characters;
}
