using UnityEngine;
using System.Collections;

public class BaseUnitView {

    protected UnitViewPresenter unitViewPresenter;

    public void ShowSelectedEffect () {
        unitViewPresenter.ShowCircle();
    }

    public void HideSelectedEffect () {
        unitViewPresenter.HideCircle();
    }

    public UnitViewPresenter GetUnitViewPresenter () {
        return unitViewPresenter;
    }

    public BaseUnitView ( UnitViewPresenter unitViewPresenter, BaseUnitController.SelectUnit selectUnit, BaseUnit.DamageDelegate damageDelegate ) {
        this.unitViewPresenter = unitViewPresenter;
        unitViewPresenter.AddSelectDelegate( selectUnit );
        unitViewPresenter.AddDamageDelegate( damageDelegate );
    }

}
