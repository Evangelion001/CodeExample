﻿using UnityEngine;

public class UnitViewPresenter : MonoBehaviour {

    public GameObject selectCircle;
    public NavMeshAgent navMeshAgent;
    public BaseUnitBehaviour baseUnitBehaviour;
    public BaseUnit.UnitType unityType;
    public Animation _animation;

    private BaseUnitController.SelectUnit selectUnit;
    private BaseUnit.DamageDelegate damageDelegate;
    private Player player;

    public void AddSelectDelegate ( BaseUnitController.SelectUnit selectUnit ) {
        this.selectUnit = selectUnit;
    }

    public void AddDamageDelegate ( BaseUnit.DamageDelegate damageDelegate ) {
        this.damageDelegate = damageDelegate;
    }

    public void Select () {
        selectUnit();
    }

    public void ShowCircle () {
        selectCircle.SetActive( true );
    }

    public void HideCircle () {
        selectCircle.SetActive( false );
    }

    public void GetDamage (Influence influence ) {
        damageDelegate( influence );
    }

    public void GetPlayer (Player player) {
        this.player = player;
    }

    public void GetGold (int gold) {
        //FIXME rename method
        player.Gold = gold;
    }

}
