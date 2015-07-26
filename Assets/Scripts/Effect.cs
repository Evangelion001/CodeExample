using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Effect {

    public Effect ( EffectsController effectsController ) {

        stackRelationship = new Dictionary<Effect, bool>();

        List<Effect> tempEffects = effectsController.AddEffectToArray(this);

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
    public BaseUnit.UnitCharacteristics characteristicsModifiers = new BaseUnit.UnitCharacteristics();
    //Dot damage etc

}
