using UnityEngine;

public class HeroUnit : BaseUnit {

    int[] xpLevels = { 100, 200, 300, 400, 500 };
    int freeAbilityPoints = 1;
    int currentLevel = 0;
    int currentXp = 0;

    private CreepUnit ( string name, 
        UnitCharacteristics characteristics, 
        Spell[] spells, 
        EntityController.Faction faction, 
        EffectsController effectsController, 
        BaseUnitController.UpdateCharacteristics updateCharacteristics, 
        BaseUnitController.Death updateDeath ) : base( name, characteristics, spells, faction, effectsController, updateCharacteristics, updateDeath ) {



    }

    public void GetXp (int xp) {
        currentXp += xp;

        if ( currentXp >= xpLevels[currentLevel] ) {
            while ( currentXp >= xpLevels[currentLevel] ) {
                currentXp = currentXp - xpLevels[currentLevel];
                ++freeAbilityPoints;
                if ( currentLevel < ( xpLevels.Length - 1 ) ) {
                    ++currentLevel;
                }
            }
        }

        //FIXME UpdateController;

    }


}
