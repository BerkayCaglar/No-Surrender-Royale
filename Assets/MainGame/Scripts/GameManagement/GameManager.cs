using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance { get; private set; }

    public Characters _scriptableCharacters;

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
    /*
    private (GameObject, GameObject) ReturnCharacterAndItsPreview()
    {
        foreach (GameObject character in _scriptableCharacters._characters)
        {
            if (character.CompareTag())
            {
                return (character, character.transform.GetChild(0).gameObject);
            }
        }
    }
    */
}
