using UnityEngine;
using System;
using System.Collections;

public class HeroUnitController : BaseUnitController {

    public delegate void LevelUpEffectDelegate ();

    private EntityController.HeroResurrect heroResurrect;

    public HeroUnitController ( EntityController.Select entityControllerSelect,
        HeroViewPresentor unitViewPresenter,
        BaseUnit.UnitCharacteristics unitCharacteristics,
        EntityController.GetTarget getTarget,
        EntityController.Faction faction,
        DeathDestroy updateDeath, 
        EntityController.HeroResurrect heroResurrect,
        BaraksModel.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate ) :base ( 
            entityControllerSelect, unitViewPresenter, unitCharacteristics, getTarget, faction, updateDeath, setUpdeteCharacteristicsDelegate ) {

        this.updateDeath = updateDeath;
        EffectsController effectsController = new EffectsController();
        this.heroResurrect = heroResurrect;
        unitBehaviour.CallDeathFSMEvent();
        unitBehaviour = new HeroBehaviour( getTarget, faction, unitViewPresenter, animationController );
        unitModel = new HeroUnit( "Unit", unitCharacteristics, SpellInit( effectsController ), faction, effectsController, _UpdateCharacteristics, UpdateDeath, LevelUpEffect, setUpdeteCharacteristicsDelegate, DeleteVisualEffect );
        unitView = new HeroView( unitViewPresenter, Selected, GetDamage, ((HeroUnit)unitModel).GetXp );

    }

    private Spell[] SpellInit ( EffectsController effectsController ) {

        GameObject freezeEffect = (GameObject)Resources.Load( "Prefabs/Particles/Freeze" );
        GameObject meteorShawer = (GameObject)Resources.Load( "Prefabs/Particles/MeteorShower" );

        Spell[] spells = new Spell[2];
        spells[0] = new Spell();

        TimeEffect effect = new TimeEffect( effectsController );
        effect.name = "Freeze";
        effect.characteristicsModifiers.attackSpeed = 0.1f;
        effect.characteristicsModifiers.speed = 0.1f;
        effect.visualPrefab = freezeEffect;
        effect.duration = 10;

        spells[0].aoeRadius = 0;
        spells[0].attackRange = 10;
        spells[0].damage = 15;
        spells[0].healing = 0;
        spells[0].needTarget = true;
        spells[0].effect = effect;
        spells[0].cdTime = 5;
        spells[0].cd = false;

        TimeEffect effect2 = new TimeEffect( effectsController );
        effect2.name = "MeteorShawer";
        effect2.visualPrefab = meteorShawer;
        effect2.duration = 0;

        spells[1] = new Spell();
        spells[1].aoeRadius = 10;
        spells[1].attackRange = 10;
        spells[1].damage = 50;
        spells[1].healing = 0;
        spells[1].needTarget = false;
        spells[1].effect = effect2;
        spells[1].cdTime = 5;
        spells[1].cd = false;

        return spells;
    }

    public void LevelUpEffect () {
        ((HeroView)unitView ).ShowLevelUp();
    }

    public int GetLevel () {
        return ((HeroUnit)unitModel).GetLevel();
    }

    public override void UpdateDeath () {
        unitBehaviour.CallDeathFSMEvent();
        Debug.Log( "UpdateDeath" );
        updateDeath( this );
        unitView.GetUnitViewPresenter().gameObject.SetActive( false );
        resurrectTimer = ( ((HeroUnit)unitModel ).GetLevel() +1) * 60;
        resurrectCounter = 0;
        SceneManager.Instance.CoroutineManager.InvokeRepeatingBool( UpdateResuract, 1);
    }

    private int resurrectTimer = 0;

    private int resurrectCounter = 0;

    public void GetSpells () {

    }

    public HeroUnit.ActionSpell AbilityDelegate () {

        HeroUnit.ActionSpell resActionSpell = ( (HeroUnit)unitModel ).GetActionSpells();

        resActionSpell.positionSpell = ( (HeroBehaviour)unitBehaviour ).PlayerUseAbility;
        resActionSpell.targetSpell = ( (HeroBehaviour)unitBehaviour ).PlayerUseAbility;

        return resActionSpell;
    }

    public bool UpdateResuract () {

        if ( resurrectCounter >= resurrectTimer ) {
            Debug.Log( "UpdateResuract" );
            heroResurrect(this);
            unitView.GetUnitViewPresenter().gameObject.SetActive( true );
            ((HeroBehaviour)unitBehaviour).Resurrect();
            return false;
        }

        ++resurrectCounter;

        return true;
    }

}
