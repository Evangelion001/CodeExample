using UnityEngine;
using System;
using System.Collections;

public class BuildController : BaseUnitController {

    public BuildController ( EntityController.Select entityControllerSelect,
        BuildViewPresenter unitViewPresenter,
        BaseUnit.UnitCharacteristics unitCharacteristics,
        EntityController.GetTarget getTarget,
        EntityController.Faction faction,
        DeathDestroy updateDeath, 
        BaraksModel.SetUpdeteCharacteristicsDelegate setUpdeteCharacteristicsDelegate ) :base ( 
            entityControllerSelect, 
            unitViewPresenter, 
            unitCharacteristics, 
            getTarget, 
            faction, 
            updateDeath, 
            setUpdeteCharacteristicsDelegate ) {

        this.updateDeath = updateDeath;
        EffectsController effectsController = new EffectsController();
        unitBehaviour.CallDeathFSMEvent();
        unitBehaviour = new BuildBehaviour( getTarget, faction, unitViewPresenter, animationController );
        unitModel = new BuildUnit( unitCharacteristics, faction, effectsController, _UpdateCharacteristics, UpdateDeath, setUpdeteCharacteristicsDelegate, DeleteVisualEffect );
        BuildView unitView = new BuildView( unitViewPresenter, Selected, ((BuildUnit)unitModel).GetDamage );
        this.unitView = unitView;
    }

    public override void UpdateDeath () {
        Debug.Log( "UpdateDeath" );
        GameObject.Destroy( unitView.GetUnitViewPresenter().gameObject );
        unitBehaviour.CallDeathFSMEvent();
    }


}
