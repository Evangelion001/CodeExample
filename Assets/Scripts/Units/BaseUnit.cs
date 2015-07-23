using UnityEngine;

public class BaseUnit : IUnit {

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

    public virtual string Name {
       get {
            return name;
       }
       set {
            name = value;
       }
    }

    public virtual void GetDamage ( Damage damage ) {
        currentCharacteristics.hp -= damage.value * (1 - currentCharacteristics.armor);

        //-> targetController->View.Updatehp
        //->targetController.Hit->stateModel.Hit->( если возможно )->controller->view->HitAnimation
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
        baseCharacteristics = characteristics;
    }

    public BaseUnit ( string name, UnitCharacteristics characteristics ) {
        this.name = name;
        UpdateCharacteristics(characteristics);
    }


}
