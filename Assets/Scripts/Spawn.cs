using UnityEngine;
using System.Collections;

public class Spawn {

    private string PREFAB_LOAD_PATH = "Prefabs/Units/";
    private EntityController entityController;
    private GameObject[] prefabs;

    public Spawn ( EntityController entityController ) {
        prefabs = ResourseLoad();
        this.entityController = entityController;
    }

    private GameObject GetPrefabByType (BaseUnit.UnitType unitType) {

        foreach ( var key in prefabs ) {
            if ( key.GetComponent<UnitViewPresenter>().unitType == unitType ) {
                return key.gameObject;
            }
        }

        if ( prefabs.Length < 1) {
            Debug.LogError("Units prefabs is empty!");
        }

        return prefabs[0].gameObject;
    }

    private GameObject[] ResourseLoad () {
        Object[] tempLoadRes = Resources.LoadAll( PREFAB_LOAD_PATH );

        GameObject[] resPrefabs = new GameObject[tempLoadRes.Length];

        for ( int i = 0; i < tempLoadRes.Length; ++i ) {
            if ( ((GameObject)tempLoadRes[i]).GetComponent<UnitViewPresenter>() == null ) {
                Debug.LogError( "UnitViewPresenter not found!" );
            } else {
                resPrefabs[i] = (GameObject)tempLoadRes[i];
            }
        }

        return resPrefabs;
    }

    public void CreateUnitByType ( BaseUnit.UnitType unitType, Vector3 spawnPosition, BaseUnit.UnitCharacteristics characteristics, EntityController.Faction faction) {

        GameObject tempUnit = (GameObject)GameObject.Instantiate( GetPrefabByType( unitType ), spawnPosition, new Quaternion(0,0,0,0) );

        entityController.CreateUnit( tempUnit.GetComponent<UnitViewPresenter>(), characteristics, faction );
    }

}
