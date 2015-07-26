using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineManager : MonoBehaviour {

    public delegate bool BoolCoroutineMethodToCall ();
    public delegate void CoroutineMethodToCall ();

    private Dictionary<int,IEnumerator> currentCoroutines = new Dictionary<int, IEnumerator>();

    private int counter = 0;

    public int InvokeBuf ( EffectsController.RemoveCoroutineEffect  removeEffect, TimeEffect timeEffect ) {
        ++counter;
        IEnumerator tempIEnumerator = _InvokeBuf( removeEffect, timeEffect );
        currentCoroutines.Add( counter, tempIEnumerator );
        StartCoroutine( tempIEnumerator );
        return counter;
    }

    public int InvokeRepeating ( CoroutineMethodToCall method, float waitTime, float repeatRate ) {
        ++counter;
        IEnumerator tempIEnumerator = InvokeRepeatingMethod( method, waitTime, repeatRate );
        currentCoroutines.Add( counter, tempIEnumerator );
        StartCoroutine( tempIEnumerator );
        return counter;
    }

    public int InvokeRepeatingBool ( BoolCoroutineMethodToCall method, float repeatRate ) {
        ++counter;
        IEnumerator tempIEnumerator = InvokeRepeatingBoolMethod( method, repeatRate );
        currentCoroutines.Add( counter, tempIEnumerator );
        StartCoroutine( tempIEnumerator );
        return counter;
    }

    private IEnumerator _InvokeBuf ( EffectsController.RemoveCoroutineEffect  removeEffect, TimeEffect timeEffect ) {
        yield return new WaitForSeconds( timeEffect.duration );
        removeEffect( timeEffect );
        yield return null;
    }

    private IEnumerator InvokeRepeatingBoolMethod ( BoolCoroutineMethodToCall method, float waitTime ) {
        method();
        while ( true ) {
            yield return new WaitForSeconds( waitTime );
            if ( !method() ) {
                break;
            }
        }
        yield return null;
    }

    private IEnumerator InvokeRepeatingMethod ( CoroutineMethodToCall method, float waitTime, float repeatRate ) {
        yield return new WaitForSeconds( waitTime );
        method();

        while ( true ) {
            yield return new WaitForSeconds( repeatRate );
            method();
        }
    }

    public void StopInvoke ( int iEnumeratorId ) {
        StopCoroutine( currentCoroutines[iEnumeratorId] );
        currentCoroutines.Remove( iEnumeratorId );
    }

    public void StopInvoke ( UnityEngine.Object startedIEnumerator ) {
        StopCoroutine( (IEnumerator)startedIEnumerator );
    }

}
