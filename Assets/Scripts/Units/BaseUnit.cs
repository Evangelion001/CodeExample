using UnityEngine;

public class BaseUnit : IUnit {

    public enum UnitType {
        swordman,
        archer,
        hero
    }

    public delegate void DamageDelegate (Damage damage);

    public EntityController.Faction GetFaction() {
        return currentCharacteristics.faction;
    }

    public struct UnitCharacteristics {
        public float hp;
        public float armor;
        public float attack;
        public float attackSpeed;
        public float speed;
        public float attackRange;
        public EntityController.Faction faction;
    }

    private string name;
    private UnitCharacteristics baseCharacteristics;
    private UnitCharacteristics currentCharacteristics;
    private EffectsController effectsController;
    private int currentHp;

    public virtual string Name {
       get {
            return name;
       }
       set {
            name = value;
       }
    }

    public virtual void GetDamage ( Damage damage ) {
        currentHp -= (int)(damage.value * (1 - currentCharacteristics.armor));

        //-> targetController->View.Updatehp
        //->targetController.Hit->stateModel.Hit->( если возможно )->controller->view->HitAnimation


        if ( damage.changeCharacteristics != null ) {
            EffectsController.TimeEffect timeEffect = new EffectsController.TimeEffect();

            timeEffect = damage.changeCharacteristics( baseCharacteristics );

            currentCharacteristics = timeEffect.unitCharacteristics;

            effectsController.Buff( TurnBackCharacteristics, timeEffect );
        } 

    }

    public virtual void TurnBackCharacteristics (UnitCharacteristics bufCharacteristics ) {
        currentCharacteristics.hp += bufCharacteristics.hp;
        currentCharacteristics.armor += bufCharacteristics.armor;
        currentCharacteristics.attack += bufCharacteristics.attack;
        currentCharacteristics.attackSpeed += bufCharacteristics.attackSpeed;
        currentCharacteristics.speed += bufCharacteristics.speed;
        currentCharacteristics.attackRange += bufCharacteristics.attackRange;
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
    }

    public BaseUnit ( string name, UnitCharacteristics characteristics ) {
        this.name = name;
        baseCharacteristics = characteristics;
        UpdateCharacteristics(characteristics);
    }


}
