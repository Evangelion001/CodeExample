using UnityEngine;
using System;
using System.Collections;

public class Player {

    public delegate void GetGold(int gold);

    private CenterUIView cuiv;

    private BaraksModel baraksModel;

    public Player ( 
        Action<Action<UnitViewPresenter>> currentTargetSpell, 
        Action<Action<Vector3>> currentPositionSpell,
        Action<InputController.CursorsType> cursorAction ) {

        cuiv = new CenterUIView( GameObject.FindObjectOfType<CenterUIViewPresenter>(), GameObject.FindObjectOfType<UnitUIViewPresentor>(), currentTargetSpell,  currentPositionSpell,  cursorAction );
    }

    public void ShowBuildDescription ( int level, BaseUnit.UnitType unitType, int timeRecruitment, int updateCost ) {
        cuiv.ShowBuildDescription( level, unitType, timeRecruitment, updateCost );
    }

    public void ShowUnitDescription ( BaseUnit.UnitType unitType, float armor, int attack, float attackSpeed, float attackRange, float speed, int level ) {
        cuiv.ShowUnitDescription( unitType, armor, attack, attackSpeed, attackRange, speed, level );
    }

    public void HideUnitDescription () {
        cuiv.HideUnitDescription();
    }

    public void HideBuildDescription () {
        cuiv.HideBuildDescription();
    }

    public void ShowUnitsIcon (BaseUnit.UnitType unitType) {

        switch ( unitType ) {
            case BaseUnit.UnitType.swordman:
                cuiv.AddSwordIcon();
                break;
            case BaseUnit.UnitType.hero:
                cuiv.AddHeroIcon();
                break;
            case BaseUnit.UnitType.archer:
                cuiv.AddArcherIcon();
                break;
        }

    }

    public void ShowBuildActionButtons ( BaraksModel baraksModel ) {

        this.baraksModel = baraksModel;

        cuiv.AddBuildActionButtons( BuildUpgrade );
    }

    public void BuildUpgrade () {
        if ( Gold >= baraksModel.GetUpgradeCost() ) {
            Gold = -baraksModel.GetUpgradeCost();
            baraksModel.UpgradeBuilding();
            ShowBuildDescription( baraksModel.buildLevel, baraksModel.spawnUnitType, baraksModel.baraksUnitConstructor[baraksModel.buildLevel].trainingTime, baraksModel.GetUpgradeCost() );
        } else {
            Debug.Log( "Need more gold" );
        }
    }

    public void ShowActionButtons ( BaseUnit.UnitType unitType, HeroUnit.ActionSpell actionSpell ) {

        switch ( unitType ) {
            case BaseUnit.UnitType.hero:
                cuiv.AddHeroActionButton( actionSpell );
                break;
            default:
                cuiv.HideActionButton();
                break;
        }
    }

    public void HideActionButtons () {
        cuiv.HideActionButton();
    }

    public void HideUnitIcon () {
        cuiv.UnselectUnits();
    }

    private int gold = 0;

    public int Gold {
        get {
            return gold;
        }
        set {
            gold += value;
            cuiv.UpdateGold( gold );
        }
    }


}
