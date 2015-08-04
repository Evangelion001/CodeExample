using UnityEngine;
using System;

public class HeroUnit : BaseUnit {

    public delegate void GetXpDelegate( int xp );

    private HeroUnitController.LevelUpEffectDelegate levelUpEffect;

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
        BaseUnitController.Death updateDeath, 
        HeroUnitController.LevelUpEffectDelegate 
        levelUpEffect, BaraksModel.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate, Action deleteVisualEffect ) : base( 
            characteristics, 
            faction, 
            effectsController, 
            updateCharacteristics, 
            updateDeath, 
            setUpdeteCharacteristicsDelegate, deleteVisualEffect ) {

        this.spells = spells;
        this.levelUpEffect = levelUpEffect;
    }

    public struct ActionSpell {
        public Action<Spell, UnitViewPresenter> targetSpell;
        public Action<Spell, Vector3> positionSpell;
        public Spell[] spells;
    }

    public ActionSpell GetActionSpells () {
        ActionSpell resArray = new ActionSpell();

        resArray.spells = spells;

        return resArray;

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
                    levelUpEffect();
                }
            }
        }
        //FIXME UpdateController;

    }

    public int GetLevel () {
        return currentLevel;
    }

}
