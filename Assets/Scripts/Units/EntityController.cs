using UnityEngine;
using System.Collections.Generic;

public class EntityController {

    public delegate bool Select ( BaseUnitController selectedUnit, Faction faction );

    private UnitViewPresenter redMainTarget;
    private Vector3 blueMainPosition;

    //FIXME move to entityController (model)
    public enum Faction {
        Blue,
        Red
    }

    public delegate UnitViewPresenter GetTarget ( Faction myFaction, UnitViewPresenter unit);
    public delegate void HeroResurrect ( HeroUnitController unit );

    private UnitViewPresenter GetUnitTarget(Faction myFaction, UnitViewPresenter unit ) {

        float distance = 0;

        UnitViewPresenter resUnitViewPresenter = null;

        List<BaseUnitController> tempBaseUnitControllers = new List<BaseUnitController>();

        BaseUnitController tempBaseUnitController;

        if ( myFaction == Faction.Blue ) {
            tempBaseUnitControllers = unitsControllersRed;
            tempBaseUnitController = unitsControllersBlue.Find( x => x.GetUnitViewPresenter() == unit );
        } else {
            tempBaseUnitControllers = unitsControllersBlue;
            tempBaseUnitController = unitsControllersRed.Find( x => x.GetUnitViewPresenter() == unit );
        }

        foreach ( var key in tempBaseUnitControllers ) {
            if ( !key.GetInvulnerability() ) {

                float tempDistance = Vector3.Distance( key.GetUnitViewPresenter().transform.position, unit.transform.position );

                if ( resUnitViewPresenter == null || tempDistance < distance ) {
                    resUnitViewPresenter = key.GetUnitViewPresenter();
                    distance = tempDistance;
                }
            }
        }

        if ( resUnitViewPresenter == null ) {
            if ( myFaction == Faction.Blue ) {
                tempBaseUnitController.MoveToPosition( blueMainPosition );
            } else {
                return redMainTarget;
            }
        }

        return resUnitViewPresenter;
    }

    private List<BaseUnitController> unitsControllersRed;
    private List<BaseUnitController> unitsControllersBlue;

    private List<BaseUnitController> unitsControllersSelectedRed;
    private List<BaseUnitController> unitsControllersSelectedBlue;

    Player player;
    BuildController buildController;
    BuildViewPresenter buildViewPresenter;

    public EntityController ( Player player, UnitViewPresenter redMainTarget, Vector3 blueMainPosition, BuildViewPresenter buildViewPresenter ) {
        this.redMainTarget = redMainTarget;
        this.blueMainPosition = blueMainPosition;
        unitsControllersBlue = new List<BaseUnitController>();
        unitsControllersRed = new List<BaseUnitController>();
        unitsControllersSelectedRed = new List<BaseUnitController>();
        unitsControllersSelectedBlue = new List<BaseUnitController>();
        this.player = player;
        this.buildViewPresenter = buildViewPresenter;
    }

    private void _HeroResurrect (HeroUnitController heroUnitController ) {
        unitsControllersBlue.Add( heroUnitController );
    }

    public void CreateUnit ( UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics, Faction faction, BaraksModel.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate ) {

        unitViewPresenter.faction = faction;

        unitViewPresenter.GetPlayer( player );

        BaseUnitController unitController;

        if ( unitViewPresenter.unitType == BaseUnit.UnitType.hero ) {
            unitController = new HeroUnitController( SelectUnit, (HeroViewPresentor)unitViewPresenter, unitCharacteristics, GetUnitTarget, faction, DestroyUnit, _HeroResurrect, setUpdeteCharacteristicsDelegate );
            buildController = new BuildController( SelectUnit, buildViewPresenter, unitCharacteristics, GetUnitTarget, faction, DestroyUnit, setUpdeteCharacteristicsDelegate );
        } else {
            unitController = new BaseUnitController( SelectUnit, unitViewPresenter, unitCharacteristics, GetUnitTarget, faction, DestroyUnit, setUpdeteCharacteristicsDelegate );
        }

        if ( faction == Faction.Blue ) {
            unitsControllersBlue.Add( unitController );
        } else {
            unitsControllersRed.Add( unitController );
        }

    }

    public void SetTarget (UnitViewPresenter unit) {
        foreach ( var key in unitsControllersSelectedBlue ) {
            if ( unit.faction != key.GetUnitViewPresenter().faction) {
                key.SetPlayerTarget( unit );
            }             
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

    private bool SelectUnit (BaseUnitController selectedUnit, Faction faction) {
        if ( Faction.Blue == faction ) {

            unitsControllersSelectedBlue.Add( selectedUnit );

            if ( unitsControllersSelectedBlue.Count == 1 ) {

                int level = -1;

                if ( selectedUnit.GetUnitViewPresenter().unitType == BaseUnit.UnitType.hero ) {
                    level = ( (HeroUnitController)selectedUnit ).GetLevel();
                }

                player.ShowUnitDescription(
                    selectedUnit.GetUnitViewPresenter().unitType,
                    selectedUnit.GetCharacteristics().armor,
                    (int)selectedUnit.GetCharacteristics().attack,
                    selectedUnit.GetCharacteristics().attackSpeed,
                    selectedUnit.GetCharacteristics().attackRange,
                    selectedUnit.GetCharacteristics().speed,
                    level );
            } else {
                player.HideUnitDescription();
                if ( unitsControllersSelectedBlue.Count == 2 ) {
                    player.ShowUnitsIcon( unitsControllersSelectedBlue[0].GetUnitViewPresenter().unitType );
                }
                player.ShowUnitsIcon( selectedUnit.GetUnitViewPresenter().unitType );
            }

            if ( selectedUnit.GetUnitViewPresenter().unitType == BaseUnit.UnitType.hero ) {
                Debug.Log( "((HeroUnitController)selectedUnit).AbilityDelegate(): " + ( (HeroUnitController)selectedUnit ).AbilityDelegate().spells.Length );
                player.ShowActionButtons( selectedUnit.GetUnitViewPresenter().unitType, ((HeroUnitController)selectedUnit).AbilityDelegate() );
            }

            return true;
        } else {
            return false;
        }
    }

}
