using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    public CardStats[] cardStats;
    private void OnEnable()
    {

    }
}

[System.Serializable]
public class CardStats
{
    [Header("Card Data")]
    public Cards Card;

    //[Header("Card Components")]
    //public CardComponents CardComponents;

    [Header("Draggable Card Data")]
    public DraggableCardData DraggableCardDatas;
}

[System.Serializable]
public class Cards
{
    // This is to store the character's name
    public string CharacterName;

    // This is to store the character's health, max health, attack range, attack damage, attack speed and detect range
    public float MaxHealth;
    public float AttackRange;
    public float AttackDamage;
    public float AttackSpeed;
    public float DetectRange;


    #region For Support Character

    public float BuffSize;
    public float BuffSpeed;
    public float BuffAmount;

    #endregion
    public enum TheCharacter { Knight, CptPrice, FriendlyWizard, Eagle, SkeletonArcher, SkeletonWarrior, WolfRunner, EnemyWizard }

    // This is to store the character's type
    public enum CharacterType { Melee, Ranged, AirUnit, Support };

    // This is to store the character's attack type
    public enum AttackType { Single, Multiple, None };

    // This is to store the character's target type
    public enum TargetType { Air, Ground, Both, None };

    // This is to store the character's speed type
    public enum CharacterMovementType { Slow, Medium, Fast };

    //public TheCharacter theCharacter;
    public CharacterType characterType;
    public AttackType attackType;
    public TargetType targetType;
    public CharacterMovementType movementType;
}

/*
[System.Serializable]
public class CardComponents
{
    // This is to store the character AI, character animation controller, nav mesh agent, animator, character collision controller, character UI manager, character health manager, rigidbody and character particle system controller
    public CharacterAI CharacterAI;
    public CharacterAnimationController CharacterAnimationController;
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;
    public CharacterCollisionController CharacterCollisionController;
    public CharacterUIManager CharacterUIManager;
    public CharacterHealthManager CharacterHealthManager;
    public Rigidbody Rigidbody;
    public CharacterParticleSystemController CharacterParticleSystemController;
}
*/

[System.Serializable]
public class DraggableCardData
{
    // This is the original card prefab. We need this to instantiate the original card when we drop the card.
    public GameObject OriginalCardPrefab;

    // This is the preview card prefab. We need this to instantiate the preview card when we drag the card.
    public GameObject PreviewCardPrefab;

    // This is to store the cost of the card. We need this to check if the player has enough mana to play the card.
    public float CardCost;
}

// This is to store the character's name



