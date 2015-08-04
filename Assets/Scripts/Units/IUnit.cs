interface IUnit {

    float HP {
        get;
    }

    void GetDamage (Influence influence );

    void UpdateCharacteristics ( BaseUnit.UnitCharacteristics characteristics );
}
