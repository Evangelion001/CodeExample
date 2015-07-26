using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsController {

    public delegate void RemoveCoroutineEffect (TimeEffect effect);

    private EffectsConstructor effectsConstructor;

    //FIXME init with effectsArray
    public EffectsController () {
        effectsConstructor = new EffectsConstructor();
    }

    public List<Effect> AddEffectToArray (Effect effect) {
       return effectsConstructor.AddEffect( effect );
    }

    public void AddCoroutineToEffect ( RemoveCoroutineEffect  removeEffect, TimeEffect timeEffect ) {
        SceneManager.Instance.CoroutineManager.InvokeBuf(removeEffect,timeEffect);
    }

}
