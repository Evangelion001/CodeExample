using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineManager : MonoBehaviour {

    public delegate void BoolMethodToCall ();

    public delegate void KillCorutineDelegate();

    private Dictionary<int,IEnumerator> currentCoroutines;

    private int counter = 0;

    public int InvokeRepeating ( BoolMethodToCall method, float waitTime, float repeatRate ) {
        IEnumerator tempIEnumerator = InvokeRepeatingMethod( method, waitTime, repeatRate );
        StartCoroutine( tempIEnumerator );
        currentCoroutines.Add( counter, tempIEnumerator );
        ++counter;
        return counter;
    }

    private IEnumerator InvokeRepeatingMethod ( BoolMethodToCall method, float waitTime, float repeatRate ) {
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
        //Debug.Log( "StopInvoke" );
        StopCoroutine( (IEnumerator)startedIEnumerator );
    }

    public void StopInvoke () {
        StopAllCoroutines();
    }

}
