using System;
using UnityEngine;
using System.Collections;

public class CoroutineManager : MonoBehaviour {

    public delegate void BoolMethodToCall ();

    public IEnumerator InvokeRepeating ( BoolMethodToCall method, float waitTime, float repeatRate ) {
        IEnumerator tempIEnumerator = InvokeRepeatingMethod( method, waitTime, repeatRate );
        StartCoroutine( tempIEnumerator );
        return tempIEnumerator;
    }

    private IEnumerator InvokeRepeatingMethod ( BoolMethodToCall method, float waitTime, float repeatRate ) {
        yield return new WaitForSeconds( waitTime );
        method();

        while ( true ) {
            yield return new WaitForSeconds( repeatRate );
            method();
        }
    }

    public void StopInvoke ( string startedIEnumerator ) {
        //Debug.Log( "StopInvoke" );
        StopCoroutine( startedIEnumerator );
    }


}
