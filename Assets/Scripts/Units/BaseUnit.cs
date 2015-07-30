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
    public delegate void UpdateBaseUnitCharacteristics ( UnitCharacteristics unitCharacteristics );

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
    protected int currentHp;
    protected EntityController.Faction faction;
    protected UnitCharacteristics baseCharacteristics = new UnitCharacteristics();
    protected UnitCharacteristics currentCharacteristics = new UnitCharacteristics();
    protected EffectsController effectsController;
    protected List<Effect> currentEffects = new List<Effect>();
    protected BaseUnitController.Death updateDeath;
    protected bool invulnerability = false;

    //FIXME add to Characteristics
    protected int gold = 100;
    protected int xp = 100;

    public UnitCharacteristics GetCharacteristics () {
        return currentCharacteristics;
    }

    BaseUnitController.UpdateCharacteristics updateCharacteristicsDelegate;

    public bool Invulnerability  {
       get {
            return invulnerability;
        }
        set {
            invulnerability = value;
        }
    }

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

    protected void AddEffect ( TimeEffect timeEffect ) {

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
                if ( influence.owner.unitType == UnitType.hero ) {
                    influence.owner.GetGold( gold );
                    ((HeroViewPresentor)influence.owner).GetXP( xp );
                }
                setUpdeteCharacteristicsDelegate( UpdateBaseCharacteristics, true );
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

    protected void UpdateBaseCharacteristics ( UnitCharacteristics characteristics ) {
        baseCharacteristics = characteristics;
        UpdateAppliedEffects();
    }

    private BuildView.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate;

    public BaseUnit ( 
        string name, 
        UnitCharacteristics characteristics, 
        EntityController.Faction faction, 
        EffectsController effectsController, 
        BaseUnitController.UpdateCharacteristics updateCharacteristics, 
        BaseUnitController.Death updateDeath,
        BuildView.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate ) {
        this.setUpdeteCharacteristicsDelegate = setUpdeteCharacteristicsDelegate;
        setUpdeteCharacteristicsDelegate( UpdateBaseCharacteristics, false );
        updateCharacteristicsDelegate = updateCharacteristics;
        this.updateDeath = updateDeath;
        this.name = name;
        baseCharacteristics = characteristics;
        currentHp = baseCharacteristics.hp;
        this.faction = faction;
        this.effectsController = effectsController;
        UpdateCharacteristics( baseCharacteristics );
    }


}
