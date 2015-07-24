using UnityEngine;
using System.Collections;

public class BaseUnitController {

    public delegate void SelectUnit ();

    private EntityController.Select entityControllerSelect;

    private BaseUnitView baseUnitView;
    private BaseUnit baseUnitModel;
    private BaseUnitBehaviour baseUnitBehaviour;
    private UnitViewPresenter unitViewPresenter;

    public UnitViewPresenter GetUnitViewPresenter () {
        return unitViewPresenter;
    }

    public BaseUnitController (EntityController.Select entityControllerSelect, UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics, EntityController.GetTarget getTarget ) {

        this.entityControllerSelect = entityControllerSelect;
        this.unitViewPresenter = unitViewPresenter;
        baseUnitModel = new BaseUnit( "Unit", unitCharacteristics );
        baseUnitBehaviour = unitViewPresenter.baseUnitBehaviour;
        baseUnitBehaviour.Init( unitViewPresenter.navMeshAgent, getTarget, GetFaction() );
        baseUnitView = new BaseUnitView( unitViewPresenter, Selected, baseUnitModel.GetDamage );

    }

    protected virtual void Selected () {
        entityControllerSelect( this );
        baseUnitView.ShowSelectedEffect();
    }

    public virtual void Unselected () {
        baseUnitView.HideSelectedEffect();
    }

    public virtual void MoveToPosition (Vector3 postion) {
        baseUnitBehaviour.SetTargetPosition( postion );
    }

    public virtual EntityController.Faction GetFaction () {
        return baseUnitModel.GetFaction();
    }

    //public virtual void Attack (BaseUnit unit) {
    //    unit.GetDamage();
    //}


}
