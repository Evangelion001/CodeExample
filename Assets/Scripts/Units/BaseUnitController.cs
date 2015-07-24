using UnityEngine;
using System.Collections;

public class BaseUnitController {

    public delegate void SelectUnit ();

    private EntityController.Select entityControllerSelect;

    private BaseUnitView baseUnitView;
    private BaseUnit baseUnitModel;
    private BaseUnitBehaviour baseUnitBehaviour;

    public UnitViewPresenter GetUnitViewPresenter () {
        return baseUnitView.GetUnitViewPresenter();
    }

    public BaseUnitController (EntityController.Select entityControllerSelect, UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics, EntityController.GetTarget getTarget ) {

        this.entityControllerSelect = entityControllerSelect;
        baseUnitModel = new BaseUnit( "Unit", unitCharacteristics );
        baseUnitBehaviour = new BaseUnitBehaviour( unitViewPresenter.navMeshAgent, getTarget, GetFaction(), unitViewPresenter );
        baseUnitView = new BaseUnitView( unitViewPresenter, Selected, baseUnitModel.GetDamage );

    }

    protected virtual void Selected () {
        entityControllerSelect( this );
        baseUnitView.ShowSelectedEffect();
    }

    public virtual void Unselected () {
        Debug.Log( "Unselected: " );
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
