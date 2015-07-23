using UnityEngine;
using System.Collections;

public class BaseUnitView {

    private UnitViewPresenter unitViewPresenter;

    public void ShowSelectedEffect () {
        unitViewPresenter.ShowCircle();
    }

    public void HideSelectedEffect () {
        unitViewPresenter.ShowCircle();
    }

    public BaseUnitView ( UnitViewPresenter unitViewPresenter, BaseUnitController.SelectUnit selectUnit, BaseUnit.DamageDelegate damageDelegate ) {
        this.unitViewPresenter = unitViewPresenter;
        unitViewPresenter.AddSelectDelegate( selectUnit );
        unitViewPresenter.AddDamageDelegate( damageDelegate );
    }

}
