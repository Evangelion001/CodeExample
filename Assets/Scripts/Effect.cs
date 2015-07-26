using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Effect {

    public Effect (EffectsConstructor effectsConstructor ) {

        List<Effect> tempEffects = effectsConstructor.AddEffect(this);

        foreach ( var key in tempEffects ) {
            stackRelationship.Add( key, false );
        }

    }

    [SerializeField]
    public string name;

    [SerializeField]
    public GameObject visualPrefab;

    [SerializeField]
    public int id;

    [SerializeField]
    public Dictionary<Effect, bool> stackRelationship;

    [SerializeField]
    public BaseUnit.UnitCharacteristics characteristicsModifiers;
    //Dot damage etc

}
