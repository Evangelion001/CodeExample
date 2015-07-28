using UnityEngine;
using System.Collections;

public class BaseUnitController {

    public delegate void SelectUnit ();
    public delegate void UpdateCharacteristics ( BaseUnit.UnitCharacteristics unitCharacteristics, Influence influence );
    public delegate void Death ();
    public delegate void DeathDestroy (BaseUnitController baseUnitController );

    private EntityController.Select entityControllerSelect;

    private DeathDestroy updateDeath;

    protected BaseUnitView unitView;
    protected BaseUnit unitModel;
    private BaseUnitBehaviour baseUnitBehaviour;
    private AnimationController animationController;

    public UnitViewPresenter GetUnitViewPresenter () {
        return unitView.GetUnitViewPresenter();
    }

    private NavMeshAgent tempNavMeshAgent;

    public BaseUnitController (EntityController.Select entityControllerSelect, UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics, EntityController.GetTarget getTarget, EntityController.Faction faction, DeathDestroy updateDeath ) {
        this.updateDeath = updateDeath;
        animationController = new AnimationController( unitViewPresenter._animation );
        //Get 1 from contructor
        EffectsController effectsController = new EffectsController();

        tempNavMeshAgent = unitViewPresenter.navMeshAgent;

        this.entityControllerSelect = entityControllerSelect;
        baseUnitBehaviour = new BaseUnitBehaviour( getTarget, faction, unitViewPresenter, animationController );
        unitModel = new BaseUnit( "Unit", unitCharacteristics, faction, effectsController, _UpdateCharacteristics, UpdateDeath );
        unitView = new BaseUnitView( unitViewPresenter, Selected, unitModel.GetDamage );
    }

    protected virtual void Selected () {
        entityControllerSelect( this );
        unitView.ShowSelectedEffect();
        baseUnitBehaviour.isSelected = true;
        baseUnitBehaviour.ShowTarget();
    }

    public virtual void Unselected () {
        Debug.Log( "Unselected: " );
        unitView.HideSelectedEffect();
        baseUnitBehaviour.isSelected = false;
        baseUnitBehaviour.HideTarget();
    }

    public virtual void MoveToPosition (Vector3 postion) {
        baseUnitBehaviour.SetTargetPosition( postion );
    }

    public virtual void SetPlayerTarget ( UnitViewPresenter unitViewPresenter ) {
        baseUnitBehaviour.SetPlayerTarget( unitViewPresenter );
    }

    public void GetDamage (Influence influence ) {
        unitModel.GetDamage( influence );
    }

    protected void _UpdateCharacteristics (BaseUnit.UnitCharacteristics newCharacteristics, Influence influence ) {

        tempNavMeshAgent.speed = newCharacteristics.speed;
        baseUnitBehaviour.SetAttackParam( newCharacteristics.attackSpeed, newCharacteristics.attackRange );
        baseUnitBehaviour.SetInfluence( influence );

    }

    protected void UpdateDeath () {
        baseUnitBehaviour.CallDeathFSMEvent();
        GameObject.Destroy( unitView.GetUnitViewPresenter().gameObject );
        updateDeath(this);
        Debug.Log( "Daeth" );
    }

}
