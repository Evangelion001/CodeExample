using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsController {

    public delegate TimeEffect ChangeCharacteristics ( BaseUnit.UnitCharacteristics unitCharacteristics );

    public struct TimeEffect {
        public BaseUnit.UnitCharacteristics unitCharacteristics;
        public float duration;
    }

    public void Buff ( CoroutineManager.CoroutineBuf turnBack, TimeEffect timeEffect ) {

        SceneManager.Instance.CoroutineManager.InvokeBuf(turnBack, timeEffect.unitCharacteristics, timeEffect.duration );

    }

    private Dictionary<int, Spell> spellDictionary;

    public class Spell {
        public string Name;
        public int id;
        public GameObject effect;

    }

}
