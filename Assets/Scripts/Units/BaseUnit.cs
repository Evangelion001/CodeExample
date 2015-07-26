using UnityEngine;
using System.Collections.Generic;
using System;

public class BaseUnit : IUnit {

    public enum UnitType {
        swordman,
        archer,
        hero
    }

    public delegate void DamageDelegate ( Influence damage );

    [Serializable]
    public class UnitCharacteristics {
        [SerializeField]
        public int hp;
        [SerializeField]
        public float armor;
        [SerializeField]
        public float attack;
        [SerializeField]
        public float attackSpeed;
        [SerializeField]
        public float speed;
        [SerializeField]
        public float attackRange;

        public static UnitCharacteristics operator *( UnitCharacteristics first, UnitCharacteristics second ) {
            UnitCharacteristics temp = new UnitCharacteristics();

            temp.hp = first.hp * second.hp;
            temp.armor = first.armor * second.armor;
            temp.attackSpeed = first.attackSpeed * second.attackSpeed;
            temp.attackSpeed = first.attackSpeed * second.attackSpeed;
            temp.speed = first.speed * second.speed;
            temp.attackRange = first.attackRange * second.attackRange;

            return temp;
        }

        public static UnitCharacteristics operator +( UnitCharacteristics first, UnitCharacteristics second ) {
            UnitCharacteristics temp = new UnitCharacteristics();

            temp.hp = first.hp + second.hp;
            temp.armor = first.armor + second.armor;
            temp.attackSpeed = first.attackSpeed + second.attackSpeed;
            temp.attackSpeed = first.attackSpeed + second.attackSpeed;
            temp.speed = first.speed + second.speed;
            temp.attackRange = first.attackRange + second.attackRange;

            return temp;
        }
    }

    private string name;
    private int currentHp;
    private EntityController.Faction faction;
    private UnitCharacteristics baseCharacteristics;
    private UnitCharacteristics currentCharacteristics;
    private EffectsController effectsController;
    private List<Effect> currentEffects = new List<Effect>();
    //FIXME Move to hero
    private Spell[] spells;
    BaseUnitController.UpdateCharacteristics updateCharacteristicsDelegate;

    public EntityController.Faction GetFaction () {
        return faction;
    }

    public virtual string Name {
       get {
            return name;
       }
       set {
            name = value;
       }
    }

    private void RemoveEffect ( TimeEffect effect ) {
        currentEffects.Remove( effect );
        UpdateAppliedEffects();
        Debug.Log( "baseCharacteristics.speed: " + baseCharacteristics.speed + " currentCharacteristics.speed: " + currentCharacteristics.speed );
    }

    private void AddEffect ( TimeEffect timeEffect ) {

        for ( int i = 0; i < currentEffects.Count; ++i ) {
            if ( currentEffects[i].id == timeEffect.id) {
                currentEffects.Remove( currentEffects[i] );
            }
        }
        currentEffects.Add( timeEffect );

        ApplyEffect( timeEffect );
    }

    private void ApplyEffect ( TimeEffect timeEffect ) {
        effectsController.AddCoroutineToEffect( RemoveEffect, timeEffect );
        UpdateAppliedEffects();
        Debug.Log( "baseCharacteristics.speed: " + baseCharacteristics.speed + " currentCharacteristics.speed: " + currentCharacteristics.speed );
    }

    private void UpdateAppliedEffects () {
        UnitCharacteristics tempCharacteristics = baseCharacteristics;
        for ( int i = 0; i < currentEffects.Count; ++i ) {
            Debug.Log( "currentEffects[i].characteristicsModifiers: " + currentEffects[i].characteristicsModifiers );
            tempCharacteristics = baseCharacteristics * currentEffects[i].characteristicsModifiers;
        }
        UpdateCharacteristics( tempCharacteristics );
    }

    public virtual void GetDamage ( Influence influence ) {
        currentHp -= (int)( influence.damage * (1 - currentCharacteristics.armor));
        currentHp += (int) influence.healing;

        //-> targetController->View.Updatehp
        //->targetController.Hit->stateModel.Hit->( если возможно )->controller->view->HitAnimation

        AddEffect( influence.timeEffect );

    }

    public virtual float Attack {
        get {
            return currentCharacteristics.attack;
        }
    }

    public virtual float HP {
        get {
            return currentCharacteristics.hp;
        }
    }

    public virtual void UpdateCharacteristics ( UnitCharacteristics characteristics ) {
        currentCharacteristics = characteristics;
        updateCharacteristicsDelegate( currentCharacteristics );
    }

    public BaseUnit ( string name, UnitCharacteristics characteristics, Spell[] spells, EntityController.Faction faction, EffectsController effectsController, BaseUnitController.UpdateCharacteristics updateCharacteristics ) {
        this.updateCharacteristicsDelegate = updateCharacteristics;
        this.name = name;
        baseCharacteristics = characteristics;
        currentHp = baseCharacteristics.hp;
        UpdateCharacteristics(characteristics);
        this.spells = spells;
        this.faction = faction;
        this.effectsController = effectsController;
        Debug.Log( "baseCharacteristics.speed: " + baseCharacteristics.speed + " currentCharacteristics.speed: " + currentCharacteristics.speed );
    }


}
