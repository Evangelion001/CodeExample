using UnityEngine;
using System.Collections;

public class BuildView : BaseUnitView {

    public BuildView ( BuildViewPresenter unitViewPresenter, BaseUnitController.SelectUnit selectUnit, BaseUnit.DamageDelegate damageDelegate ) :base(
        unitViewPresenter, selectUnit, damageDelegate
        ) {
        unitViewPresenter.AddDamageDelegate( damageDelegate );
    }

}
