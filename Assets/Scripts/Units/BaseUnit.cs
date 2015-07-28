using UnityEngine;
using System.Collections.Generic;
using System;

public class BaseUnit : IUnit {

    public enum UnitType {
        swordman,
        archer,
        hero,
        bigMob
    }

    public delegate void DamageDelegate ( Influence damage );
    public delegate Influence GetInfluenceDelegate ();

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
    private UnitCharacteristics baseCharacteristics = new UnitCharacteristics();
    private UnitCharacteristics currentCharacteristics = new UnitCharacteristics();
    private EffectsController effectsController;
    private List<Effect> currentEffects = new List<Effect>();
    private BaseUnitController.Death updateDeath;
    //FIXME add to Characteristics
    private int gold = 100;
    private int xp = 100;

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
    }

    private void UpdateAppliedEffects () {
        UnitCharacteristics tempCharacteristics = baseCharacteristics;
        for ( int i = 0; i < currentEffects.Count; ++i ) {
            tempCharacteristics = baseCharacteristics * currentEffects[i].characteristicsModifiers;
        }
        UpdateCharacteristics( tempCharacteristics );
    }

    private Influence GetInfluence () {

        Influence tempInfluence = new Influence();
        tempInfluence.damage = currentCharacteristics.attack;

        return tempInfluence;

    }

    public virtual void GetDamage ( Influence influence ) {
        if ( currentHp > 0 ) {
            currentHp -= (int)( influence.damage * ( 1 - currentCharacteristics.armor ) );
            currentHp += (int)influence.healing;

            //Debug.Log( "currentHp: " + currentHp );

            if ( currentHp <= 0 ) {
                currentHp = 0;
                if ( influence.owner.unityType == UnitType.hero ) {
                    influence.owner.GetGold( gold );
                    influence.owner.GetXP( xp );
                }
                updateDeath();
            }
            //-> targetController->View.Updatehp
            //->targetController.Hit->stateModel.Hit->( если возможно )->controller->view->HitAnimation

            if ( influence.timeEffect != null ) {
                AddEffect( influence.timeEffect );
            }
        }
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
        updateCharacteristicsDelegate( currentCharacteristics, GetInfluence() );
    }

    public BaseUnit ( string name, UnitCharacteristics characteristics, Spell[] spells, EntityController.Faction faction, EffectsController effectsController, BaseUnitController.UpdateCharacteristics updateCharacteristics, BaseUnitController.Death updateDeath ) {
        this.updateCharacteristicsDelegate = updateCharacteristics;
        this.updateDeath = updateDeath;
        this.name = name;
        baseCharacteristics = characteristics;
        currentHp = baseCharacteristics.hp;
        this.spells = spells;
        this.faction = faction;
        this.effectsController = effectsController;
        UpdateCharacteristics( baseCharacteristics );
    }


}
