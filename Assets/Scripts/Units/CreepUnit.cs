using UnityEngine;

public class CreepUnit : BaseUnit {

    private int gold;
    private int xp;

    private CreepUnit ( string name, UnitCharacteristics characteristics, int gold, int xp, Spell[] spells, EntityController.Faction faction, EffectsController effectsController, BaseUnitController.UpdateCharacteristics updateCharacteristics, BaseUnitController.Death updateDeath ) : base( name, characteristics, spells, faction, effectsController, updateCharacteristics, updateDeath ) {
        this.gold = gold;
        this.xp = xp;
    }

    public int Gold {
        get {
            return gold;
        }
    }

    public int Xp {
        get {
            return xp;
        }
    }

}
