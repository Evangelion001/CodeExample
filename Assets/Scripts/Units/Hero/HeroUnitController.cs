using UnityEngine;
using System.Collections;

public class HeroUnitController : BaseUnitController {

    public HeroUnitController ( EntityController.Select entityControllerSelect, 
        HeroViewPresentor unitViewPresenter, 
        BaseUnit.UnitCharacteristics unitCharacteristics, 
        EntityController.GetTarget getTarget, 
        EntityController.Faction faction,
        DeathDestroy updateDeath, Player.GetGold GetGold = null ):base ( entityControllerSelect, unitViewPresenter, unitCharacteristics, getTarget, faction, updateDeath ) {

        EffectsController effectsController = new EffectsController();

        unitModel = new HeroUnit( "Unit", unitCharacteristics, SpellInit( effectsController ), faction, effectsController, _UpdateCharacteristics, UpdateDeath );
        unitView = new HeroView( unitViewPresenter, Selected, unitModel.GetDamage, ((HeroUnit)unitModel).GetXp );
    }

    private Spell[] SpellInit ( EffectsController effectsController ) {

        GameObject freezeEffect = (GameObject)Resources.Load( "Prefabs/Particles/Freeze" );

        Spell[] spells = new Spell[1];
        spells[0] = new Spell();

        Effect effect = new Effect( effectsController );
        effect.characteristicsModifiers.attackSpeed = 0.5f;
        effect.characteristicsModifiers.speed = 0.5f;
        effect.visualPrefab = freezeEffect;

        spells[0].aoeRadius = 0;
        spells[0].attackRange = 1;
        spells[0].damage = 15;
        spells[0].healing = 0;
        spells[0].needTarget = false;
        spells[0].effect = effect;

        return spells;
    }

}
