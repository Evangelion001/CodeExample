using UnityEngine;

public class CreepUnit : BaseUnit {

    private int gold;
    private int xp;

    private CreepUnit ( string name, UnitCharacteristics characteristics, int gold, int xp ): base( name, characteristics ) {
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
