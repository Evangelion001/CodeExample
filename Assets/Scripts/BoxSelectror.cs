using UnityEngine;
using System.Collections.Generic;
using System;

public class BoxSelectror : MonoBehaviour {

    public List<GameObject> selectedObject;

    void OnTriggerEnter ( Collider other ) {
        if ( other.transform.gameObject.tag == "Unit" ) {
            selectedObject.Add( other.transform.gameObject );
        } 
    }

    void OnTriggerExit ( Collider other ) {
        if ( other.transform.gameObject.tag == "Unit" ) {
            selectedObject.Remove( other.transform.gameObject );
        }
    }

    public GameObject[] GetSelectedObjects () {
        return selectedObject.ToArray();
    }
 
}
