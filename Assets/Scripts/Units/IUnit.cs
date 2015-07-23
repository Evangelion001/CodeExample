interface IUnit {

    string Name {
        get;
        set;
    }

    float HP {
        get;
    }

    void GetDamage (Damage damage);

    void UpdateCharacteristics ( BaseUnit.UnitCharacteristics characteristics );
}
