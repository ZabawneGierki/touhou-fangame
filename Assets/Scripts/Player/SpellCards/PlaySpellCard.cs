using System;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaySpellCard : MonoBehaviour
{

    public SpellCard spellCard;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    private void OnEnable()
    {
        InputManager.Instance.BombAction.action.performed += OnSpellPeformed;


    }

    private void OnSpellPeformed(InputAction.CallbackContext context)
    {
        Debug.Log("Spell Card Played");
        StartCoroutine(spellCard.PlaySpellCard());
    }

    private void OnDisable()
    {
        InputManager.Instance.BombAction.action.performed -= OnSpellPeformed;

    }
    
    
}
