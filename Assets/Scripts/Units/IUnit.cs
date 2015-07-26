interface IUnit {

    string Name {
        get;
        set;
    }

    float HP {
        get;
    }

    void GetDamage (Influence influence );

    void UpdateCharacteristics ( BaseUnit.UnitCharacteristics characteristics );
}
