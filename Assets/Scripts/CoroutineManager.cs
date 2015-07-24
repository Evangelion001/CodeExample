using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineManager : MonoBehaviour {

    public delegate void BoolMethodToCall ();

    private Dictionary<int,IEnumerator> currentCoroutines = new Dictionary<int, IEnumerator>();

    private int counter = 0;

    public int InvokeRepeating ( BoolMethodToCall method, float waitTime, float repeatRate ) {
        ++counter;
        IEnumerator tempIEnumerator = InvokeRepeatingMethod( method, waitTime, repeatRate );
        currentCoroutines.Add( counter, tempIEnumerator );
        StartCoroutine( tempIEnumerator );
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
        StopCoroutine( (IEnumerator)startedIEnumerator );
    }

}
