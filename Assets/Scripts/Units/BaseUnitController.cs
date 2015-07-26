﻿using UnityEngine;
using System.Collections;

public class BaseUnitController {

    public delegate void SelectUnit ();
    public delegate void UpdateCharacteristics ( BaseUnit.UnitCharacteristics unitCharacteristics );

    private EntityController.Select entityControllerSelect;

    private BaseUnitView baseUnitView;
    private BaseUnit baseUnitModel;
    private BaseUnitBehaviour baseUnitBehaviour;

    public UnitViewPresenter GetUnitViewPresenter () {
        return baseUnitView.GetUnitViewPresenter();
    }

    public BaseUnitController (EntityController.Select entityControllerSelect, UnitViewPresenter unitViewPresenter, BaseUnit.UnitCharacteristics unitCharacteristics, EntityController.GetTarget getTarget, EntityController.Faction faction ) {

        EffectsController effectsController = new EffectsController();

        Spell[] spells = new Spell[1];
        spells[0] = new Spell();

        Effect effect = new Effect( effectsController );
        effect.characteristicsModifiers.attackSpeed = 0.5f;
        effect.characteristicsModifiers.speed = 0.5f;

        spells[0].aoeRadius = 0;
        spells[0].attackRange = 1;
        spells[0].damage = 15;
        spells[0].healing = 0;
        spells[0].needTarget = false;
        spells[0].effect = effect;

        this.entityControllerSelect = entityControllerSelect;
        baseUnitModel = new BaseUnit( "Unit", unitCharacteristics, spells, faction, effectsController, _UpdateCharacteristics );
        baseUnitBehaviour = new BaseUnitBehaviour( unitViewPresenter.navMeshAgent, getTarget, GetFaction(), unitViewPresenter );
        baseUnitView = new BaseUnitView( unitViewPresenter, Selected, baseUnitModel.GetDamage );

        Influence influence = new Influence();
        influence.damage = 10;
        influence.healing = 0;

        TimeEffect timeEffect = new TimeEffect( effectsController );
        timeEffect.characteristicsModifiers.attackSpeed = 0.1f;
        timeEffect.characteristicsModifiers.speed = 0.1f;
        timeEffect.duration = 5;

        influence.timeEffect = timeEffect;

        GetDamage( influence );
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

    public void GetDamage (Influence influence ) {
        baseUnitModel.GetDamage( influence );
    }

    private void _UpdateCharacteristics (BaseUnit.UnitCharacteristics newCharacteristics ) {

        baseUnitBehaviour.navMeshAgent.speed = newCharacteristics.speed;

    }

    //public virtual void Attack (BaseUnit unit) {
    //    unit.GetDamage();
    //}

}
