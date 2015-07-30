using UnityEngine;
using System.Collections;

public class Player {

    public delegate void GetGold(int gold);

    private CenterUIView cuiv;

    public Player () {
        cuiv = new CenterUIView( GameObject.FindObjectOfType<CenterUIViewPresenter>(), GameObject.FindObjectOfType<UnitUIViewPresentor>()  );
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

    public void ShowBuildActionButtons () {
        cuiv.AddBuildActionButtons();
    }

    public void ShowActionButtons ( BaseUnit.UnitType unitType ) {

        switch ( unitType ) {
            case BaseUnit.UnitType.hero:
                cuiv.AddHeroActionButton();
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
