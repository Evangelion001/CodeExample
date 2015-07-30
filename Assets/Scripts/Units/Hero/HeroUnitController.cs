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
        BuildView.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate ) :base ( entityControllerSelect, unitViewPresenter, unitCharacteristics, getTarget, faction, updateDeath, setUpdeteCharacteristicsDelegate ) {

        EffectsController effectsController = new EffectsController();
        this.heroResurrect = heroResurrect;
        unitBehaviour.CallDeathFSMEvent();
        unitBehaviour = new HeroBehaviour( getTarget, faction, unitViewPresenter, animationController );
        unitModel = new HeroUnit( "Unit", unitCharacteristics, SpellInit( effectsController ), faction, effectsController, _UpdateCharacteristics, UpdateDeath, LevelUpEffect, setUpdeteCharacteristicsDelegate );
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

    public void LevelUpEffect () {
        ((HeroView)unitView ).ShowLevelUp();
    }

    public int GetLevel () {
        return ((HeroUnit)unitModel).GetLevel();
    }

    public override void UpdateDeath () {
        unitBehaviour.CallDeathFSMEvent();
        unitView.GetUnitViewPresenter().gameObject.SetActive( false );
        updateDeath( this );
        resurrectTimer = ( ((HeroUnit)unitModel ).GetLevel() +1) * 60;
        resurrectCounter = 0;
        SceneManager.Instance.CoroutineManager.InvokeRepeatingBool( UpdateResuract, 1);
    }

    private int resurrectTimer = 0;

    private int resurrectCounter = 0;

    public bool UpdateResuract () {

        if ( resurrectCounter >= resurrectTimer ) {
            heroResurrect(this);
            unitView.GetUnitViewPresenter().gameObject.SetActive( true );
            ((HeroBehaviour)unitBehaviour).Resurrect();
            return false;
        }

        ++resurrectCounter;

        return true;
    }

}
