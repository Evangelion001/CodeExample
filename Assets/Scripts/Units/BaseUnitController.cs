using UnityEngine;
using System.Collections;

public class BaseUnitController {

    public delegate void SelectUnit ();
    public delegate void UpdateCharacteristics ( BaseUnit.UnitCharacteristics unitCharacteristics, Influence influence );
    public delegate void Death ();
    public delegate void DeathDestroy (BaseUnitController baseUnitController );

    private EntityController.Select entityControllerSelect;

    protected DeathDestroy updateDeath;

    protected BaseUnitView unitView;
    protected BaseUnit unitModel;
    protected BaseUnitBehaviour unitBehaviour;
    protected AnimationController animationController;

    public UnitViewPresenter GetUnitViewPresenter () {
        return unitView.GetUnitViewPresenter();
    }

    private NavMeshAgent tempNavMeshAgent;

    public BaseUnitController (
        EntityController.Select entityControllerSelect, 
        UnitViewPresenter unitViewPresenter, 
        BaseUnit.UnitCharacteristics unitCharacteristics, 
        EntityController.GetTarget getTarget, 
        EntityController.Faction faction, 
        DeathDestroy updateDeath, 
        BuildView.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate ) {

        this.updateDeath = updateDeath;

        animationController = new AnimationController( unitViewPresenter._animation );

        EffectsController effectsController = new EffectsController();

        tempNavMeshAgent = unitViewPresenter.navMeshAgent;

        this.entityControllerSelect = entityControllerSelect;
        unitBehaviour = new BaseUnitBehaviour( getTarget, faction, unitViewPresenter, animationController );
        unitModel = new BaseUnit( "Unit", unitCharacteristics, faction, effectsController, _UpdateCharacteristics, UpdateDeath, setUpdeteCharacteristicsDelegate );
        unitView = new BaseUnitView( unitViewPresenter, Selected, unitModel.GetDamage );
    }

    protected virtual void Selected () {
        if ( entityControllerSelect( this, unitModel.GetFaction() ) ) {
            unitView.ShowSelectedEffect();
            unitBehaviour.isSelected = true;
            unitBehaviour.ShowTarget();
        }
    }

    public virtual void Unselected () {
        unitView.HideSelectedEffect();
        unitBehaviour.isSelected = false;
        unitBehaviour.HideTarget();
    }

    public virtual void MoveToPosition (Vector3 postion) {
        unitBehaviour.SetTargetPosition( postion );
    }

    public virtual void SetPlayerTarget ( UnitViewPresenter unitViewPresenter ) {
        unitBehaviour.SetPlayerTarget( unitViewPresenter );
    }

    public void GetDamage (Influence influence ) {
        unitModel.GetDamage( influence );
    }

    protected void _UpdateCharacteristics (BaseUnit.UnitCharacteristics newCharacteristics, Influence influence ) {

        tempNavMeshAgent.speed = newCharacteristics.speed;
        unitBehaviour.SetAttackParam( newCharacteristics.attackSpeed, newCharacteristics.attackRange );
        unitBehaviour.SetInfluence( influence );
        
    }

    public virtual BaseUnit.UnitCharacteristics GetCharacteristics () {
        return unitModel.GetCharacteristics();
    }

    public virtual void UpdateDeath () {
        unitBehaviour.CallDeathFSMEvent();
        GameObject.Destroy( unitView.GetUnitViewPresenter().gameObject );
        updateDeath(this);
    }

    public bool GetInvulnerability () {
        return unitModel.Invulnerability;
    }

}
