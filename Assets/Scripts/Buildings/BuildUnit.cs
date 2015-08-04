using UnityEngine;
using System;

public class BuildUnit : BaseUnit {

    public BuildUnit (
        UnitCharacteristics characteristics, 
        EntityController.Faction faction, 
        EffectsController effectsController, 
        BaseUnitController.UpdateCharacteristics updateCharacteristics, 
        BaseUnitController.Death updateDeath, 
        BaraksModel.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate,
        Action deleteVisualEffect ) : base (
            characteristics, 
            faction, 
            effectsController, 
            updateCharacteristics, 
            updateDeath, 
            setUpdeteCharacteristicsDelegate,
            deleteVisualEffect ) {
        
    }

    public override void GetDamage ( Influence influence ) {
        if ( currentHp > 0 ) {
            currentHp -= (int)( influence.damage * ( 1 - currentCharacteristics.armor ) );

            if ( currentHp <= 0 ) {
                currentHp = 0;
                setUpdeteCharacteristicsDelegate( UpdateBaseCharacteristics, true );
                updateDeath();
            }
        } 
    }

}
