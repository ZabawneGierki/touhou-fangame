using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/Spell Cards/Star")]
public class Star : SpellCard
{
    public override IEnumerator PlaySpellCard()
    {
        Debug.Log("Star Spell Card Activated: " + cardName);
        // Implement the specific behavior of the Star spell card here.
        yield return null;
    }
}

 
