using UnityEngine;
using System.Collections;

public class BaseUnitController {

    public delegate void SelectUnit ();
    public delegate void UpdateCharacteristics ( BaseUnit.UnitCharacteristics unitCharacteristics, Influence influence );
    public delegate void Death ();
    public delegate void DeathDestroy (BaseUnitController baseUnitController );

    private EntityController.Select entityControllerSelect;

    private DeathDestroy updateDeath;

    private BaseUnitView baseUnitView;
    private BaseUnit baseUnitModel;
    private BaseUnitBehaviour baseUnitBehaviour;
    private AnimationController animationController;

    public UnitViewPresenter GetUnitViewPresenter () {
        return baseUnitView.GetUnitViewPresenter();
    }

    private NavMeshAgent tempNavMeshAgent;

    public BaseUnitController (EntityController.Select entityControllerSelect, UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics, EntityController.GetTarget getTarget, EntityController.Faction faction, DeathDestroy updateDeath ) {
        this.updateDeath = updateDeath;
        animationController = new AnimationController( unitViewPresenter._animation );

        EffectsController effectsController = new EffectsController();

        GameObject freezeEffect = (GameObject)Resources.Load( "Prefabs/Particles/Freeze" );
        Spell[] spells = new Spell[1];
        spells[0] = new Spell();

        Effect effect = new Effect( effectsController );
        effect.characteristicsModifiers.attackSpeed = 0.5f;
        effect.characteristicsModifiers.speed = 0.5f;
        effect.visualPrefab = freezeEffect;

        spells[0].aoeRadius = 0;
        spells[0].attackRange = 1;
        spells[0].damage = 15;
        spells[0].healing = 0;
        spells[0].needTarget = false;
        spells[0].effect = effect;

        tempNavMeshAgent = unitViewPresenter.navMeshAgent;

        this.entityControllerSelect = entityControllerSelect;
        baseUnitBehaviour = new BaseUnitBehaviour( getTarget, faction, unitViewPresenter, animationController );
        baseUnitModel = new BaseUnit( "Unit", unitCharacteristics, spells, faction, effectsController, _UpdateCharacteristics, UpdateDeath );
        baseUnitView = new BaseUnitView( unitViewPresenter, Selected, baseUnitModel.GetDamage );

        Influence influence = new Influence();
        influence.damage = 10;
        influence.healing = 0;

        TimeEffect timeEffect = new TimeEffect( effectsController );
        timeEffect.characteristicsModifiers.attackSpeed = 0.1f;
        timeEffect.characteristicsModifiers.speed = 0.1f;
        timeEffect.duration = 5;

        influence.timeEffect = timeEffect;

        //GetDamage( influence );
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

    public void GetDamage (Influence influence ) {
        baseUnitModel.GetDamage( influence );
    }

    private void _UpdateCharacteristics (BaseUnit.UnitCharacteristics newCharacteristics, Influence influence ) {

        tempNavMeshAgent.speed = newCharacteristics.speed;
        baseUnitBehaviour.SetAttackParam( newCharacteristics.attackSpeed, newCharacteristics.attackRange );
        baseUnitBehaviour.SetInfluence( influence );

    }

    private void UpdateDeath () {
        baseUnitBehaviour.CallDeathFSMEvent();
        GameObject.Destroy( baseUnitView.GetUnitViewPresenter().gameObject );
        updateDeath(this);
        Debug.Log( "Daeth" );
    }

}
