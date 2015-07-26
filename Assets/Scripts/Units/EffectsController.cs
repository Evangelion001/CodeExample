using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsController {

    public delegate void RemoveCoroutineEffect (Effect effect);

    private EffectsConstructor effectsConstructor;

    //FIXME init with effectsArray
    public EffectsController () {
        effectsConstructor = new EffectsConstructor();
    }

    public void AddEffectToArray (Effect effect) {
        effectsConstructor.AddEffect( effect );
    }

    public void AddCoroutineToEffect ( RemoveCoroutineEffect  removeEffect, TimeEffect timeEffect ) {
        SceneManager.Instance.CoroutineManager.InvokeBuf(removeEffect,timeEffect);
    }

}
