using System.Collections;
using UnityEngine;

public abstract class SpellCard : ScriptableObject
{
    public string cardName;

    public abstract  IEnumerator PlaySpellCard();




}
