using UnityEngine;

public class HeroUnit : BaseUnit {

    public delegate void GetXpDelegate( int xp );

    int[] xpLevels = { 100, 200, 300, 400, 500 };
    int freeAbilityPoints = 1;
    int currentLevel = 0;
    int currentXp = 0;
    private Spell[] spells;

    public HeroUnit ( string name, 
        UnitCharacteristics characteristics, 
        Spell[] spells, 
        EntityController.Faction faction, 
        EffectsController effectsController, 
        BaseUnitController.UpdateCharacteristics updateCharacteristics, 
        BaseUnitController.Death updateDeath ) : base( name, characteristics, faction, effectsController, updateCharacteristics, updateDeath ) {

    }

    public void GetXp (int xp) {
        Debug.Log( "xp: " + xp );
        currentXp += xp;

        if ( currentXp >= xpLevels[currentLevel] ) {
            while ( currentXp >= xpLevels[currentLevel] ) {
                currentXp -= xpLevels[currentLevel];
                ++freeAbilityPoints;
                if ( currentLevel < ( xpLevels.Length - 1 ) ) {
                    ++currentLevel;
                }
            }
        }
        Debug.Log( "currentLevel: " + currentLevel );
        //FIXME UpdateController;

    }


}
