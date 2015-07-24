using UnityEngine;
using System.Collections;

public class EntityController : MonoBehaviour {

    public delegate void Select ( BaseUnitController selectedUnit );

    //FIXME move to entityController (model)
    public enum Faction {
        Blue,
        Red
    }

    public delegate UnitViewPresenter GetTarget ( Faction myFaction );

    private UnitViewPresenter GetUnitTarget(Faction myFaction) {
        if ( myFaction == Faction.Blue ) {
            return unitsControllersRed[0].GetUnitViewPresenter();
        } else {
            return unitsControllersBlue[0].GetUnitViewPresenter();
        }
    }

    private BaseUnitController[] unitsControllersRed;
    private BaseUnitController[] unitsControllersBlue;

    private BaseUnitController[] unitsControllersSelectedRed;
    private BaseUnitController[] unitsControllersSelectedBlue;

    public void CreateUnit ( UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics ) {

        if ( unitsControllersBlue == null ) {
            unitsControllersBlue = new BaseUnitController[1];
        }
        if ( unitsControllersRed == null ) {
            unitsControllersRed = new BaseUnitController[1];
        }

        if ( unitCharacteristics.faction == Faction.Blue ) {
            unitCharacteristics.faction = Faction.Blue;
            unitsControllersBlue[0] = new BaseUnitController( SelectUnit, unitViewPresenter, unitCharacteristics, GetUnitTarget );
        } else {
            unitCharacteristics.faction = Faction.Red;
            unitsControllersRed[0] = new BaseUnitController( SelectUnit, unitViewPresenter, unitCharacteristics, GetUnitTarget );
        }

    }

    public void UnselectUints () {
        if ( unitsControllersSelectedRed != null ) {
            foreach ( var key in unitsControllersSelectedRed ) {
                key.Unselected();
            }
            unitsControllersSelectedRed = new BaseUnitController[0];
        }

        if ( unitsControllersSelectedBlue != null ) {
            foreach ( var key in unitsControllersSelectedBlue ) {
                key.Unselected();
            }
            unitsControllersSelectedBlue = new BaseUnitController[0];
        }
    }

    public void MoveToPosition (Vector3 position) {
        if ( unitsControllersSelectedBlue != null ) {
            foreach ( var key in unitsControllersSelectedBlue ) {
                key.MoveToPosition( position );
            }
        } else {
            Debug.Log( "Not have selected units!" );
        }
    }

    private void SelectUnit (BaseUnitController selectedUnit) {
        if ( unitsControllersSelectedBlue == null || unitsControllersSelectedBlue.Length == 0 ) {
            unitsControllersSelectedBlue = new BaseUnitController[1];
        }
        unitsControllersSelectedBlue[0] = selectedUnit;
        Debug.Log( "SelectUnit" );
    }

}
