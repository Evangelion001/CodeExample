using UnityEngine;
using System.Collections.Generic;

public class EntityController {

    public delegate void Select ( BaseUnitController selectedUnit );

    //FIXME move to entityController (model)
    public enum Faction {
        Blue,
        Red
    }

    public delegate UnitViewPresenter GetTarget ( Faction myFaction, UnitViewPresenter unit);

    private UnitViewPresenter GetUnitTarget(Faction myFaction, UnitViewPresenter unit ) {

        float distance = 0;

        UnitViewPresenter resUnitViewPresenter = null;

        List<BaseUnitController> tempBaseUnitControllers = new List<BaseUnitController>();

        if ( myFaction == Faction.Blue ) {
            tempBaseUnitControllers = unitsControllersRed;
        } else {
            tempBaseUnitControllers = unitsControllersBlue;
        }

        foreach ( var key in tempBaseUnitControllers ) {

            float tempDistance = Vector3.Distance( key.GetUnitViewPresenter().transform.position, unit.transform.position );
            if ( resUnitViewPresenter == null || tempDistance < distance ) {
                resUnitViewPresenter = key.GetUnitViewPresenter();
                distance = tempDistance;
            }

        }

        return resUnitViewPresenter;
    }

    private List<BaseUnitController> unitsControllersRed;
    private List<BaseUnitController> unitsControllersBlue;

    private List<BaseUnitController> unitsControllersSelectedRed;
    private List<BaseUnitController> unitsControllersSelectedBlue;

    Player player;

    public EntityController ( Player player) {
        unitsControllersBlue = new List<BaseUnitController>();
        unitsControllersRed = new List<BaseUnitController>();
        unitsControllersSelectedRed = new List<BaseUnitController>();
        unitsControllersSelectedBlue = new List<BaseUnitController>();
        this.player = player;
    }

    public void CreateUnit ( UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics, Faction faction ) {

        unitViewPresenter.GetPlayer( player );

        BaseUnitController unitController;

        if ( unitViewPresenter.unityType == BaseUnit.UnitType.hero ) {
            unitController = new HeroUnitController( SelectUnit, (HeroViewPresentor)unitViewPresenter, unitCharacteristics, GetUnitTarget, faction, DestroyUnit );
        } else {
            unitController = new BaseUnitController( SelectUnit, unitViewPresenter, unitCharacteristics, GetUnitTarget, faction, DestroyUnit );
        }

        if ( faction == Faction.Blue ) {
            unitsControllersBlue.Add( unitController );
        } else {
            unitsControllersRed.Add( unitController );
        }

    }

    public void SetTarget (UnitViewPresenter unit) {
        foreach ( var key in unitsControllersSelectedBlue ) {
            key.SetPlayerTarget( unit );
        }
    }

    public bool isSelected () {
        if ( unitsControllersSelectedBlue.Count > 0 ) {
            return true;
        }
        return false;
    }

    public void DestroyUnit (BaseUnitController unitController ) {
        if ( unitsControllersSelectedBlue.Contains( unitController ) ) {
            unitsControllersSelectedBlue.Remove( unitController );
        }

        if ( unitsControllersBlue.Contains( unitController ) ) {
            unitsControllersBlue.Remove( unitController );
        }
        if ( unitsControllersRed.Contains( unitController ) ) {
            unitsControllersRed.Remove( unitController );
        }
    }

    public void UnselectUints () {

        foreach ( var key in unitsControllersSelectedRed ) {
            key.Unselected();
        }
        unitsControllersSelectedRed.Clear();

        foreach ( var key in unitsControllersSelectedBlue ) {
            key.Unselected();
        }
        unitsControllersSelectedBlue.Clear();

    }

    public void MoveToPosition (Vector3 position) {
        foreach ( var key in unitsControllersSelectedBlue ) {
            key.MoveToPosition( position );
        }
    }

    private void SelectUnit (BaseUnitController selectedUnit) {
        unitsControllersSelectedBlue.Clear();
        unitsControllersSelectedBlue.Add(selectedUnit);
    }

}
