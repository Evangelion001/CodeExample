using System;
using UnityEngine;
using UnityEngine.UI;

public class CenterUIView {

    private CenterUIViewPresenter uiViewPresenter;
    private UnitUIViewPresentor unitUIViewPresentor;

    public void UpdateGold (int gold) {
        uiViewPresenter.goldValue.text = gold.ToString();
    }

    private int selectUnitCounter = 0;
    private int actionButtonCounter = 0;

    private GameObject[] actionButtonArray = new GameObject[6];
    private GameObject[] unitIconArray = new GameObject[14];
    private Action<InputController.CursorsType> cursorAction;
    public CenterUIView ( CenterUIViewPresenter uiViewPresenter, UnitUIViewPresentor unitUIViewPresentor, Action<Action<UnitViewPresenter>> currentTargetSpell, 
        Action<Action<Vector3>> currentPositionSpell, Action<InputController.CursorsType> cursorAction ) {
        this.cursorAction = cursorAction;
        this.currentTargetSpell = currentTargetSpell;
        this.currentPositionSpell = currentPositionSpell;
        this.uiViewPresenter = uiViewPresenter;
        this.unitUIViewPresentor = unitUIViewPresentor;
        HideUnitDescription();

        for ( int i = 0; i < unitIconArray.Length; ++i ) {
            unitIconArray[i] = GameObject.Instantiate( uiViewPresenter.unitIconPrefab );
            unitIconArray[i].name = "UnitIcon " + i;
            unitIconArray[i].transform.SetParent( uiViewPresenter.gridUnitIcons.transform, false );
            unitIconArray[i].SetActive( false );
        }

        for ( int i = 0; i < actionButtonArray.Length; ++i ) {
            actionButtonArray[i] = GameObject.Instantiate( uiViewPresenter.actionButtonPrefab );
            actionButtonArray[i].name = "ActionButton " + i;
            actionButtonArray[i].transform.SetParent( uiViewPresenter.actionButtonGrid.transform, false );
            actionButtonArray[i].SetActive( false );
        }
    }

    public void ShowUnitDescription ( BaseUnit.UnitType unitType, float armor, int attack, float attackSpeed, float attackRange, float speed, int level ) {
        unitUIViewPresentor.UnitPanel.SetActive( true );
        unitUIViewPresentor.LevelPanel.SetActive( false );

        unitUIViewPresentor.Type = unitType.ToString();
        unitUIViewPresentor.Armor = armor;
        unitUIViewPresentor.Attack = attack;
        unitUIViewPresentor.AttackSpeed = attackSpeed;
        unitUIViewPresentor.AttackRange = attackRange;
        unitUIViewPresentor.Speed = speed;

        if ( level > -1 ) {
            unitUIViewPresentor.LevelPanel.SetActive( true );
            unitUIViewPresentor.Level = level;
        }

        switch ( unitType ) {
            case BaseUnit.UnitType.swordman:
                unitUIViewPresentor.Icon = uiViewPresenter.swordIcon;
                break;
            case BaseUnit.UnitType.archer:
                unitUIViewPresentor.Icon = uiViewPresenter.archerIcon;
                break;
            case BaseUnit.UnitType.hero:
                unitUIViewPresentor.Icon = uiViewPresenter.heroIcon;
                break;
        }
        
    }

    public void HideUnitDescription () {
        unitUIViewPresentor.UnitPanel.SetActive( false );
    }

    public void ShowBuildDescription ( int level, BaseUnit.UnitType unitType, int timeRecruitment, int updateCost ) {
        uiViewPresenter.BaraksPanel.SetActive( true );

        uiViewPresenter.buildTitle.text = "Baraks level " + level;
        uiViewPresenter.unitType.text = "Unit type " + unitType.ToString();
        uiViewPresenter.timeRecruitment.text = "Time Recruitment " + timeRecruitment;
        uiViewPresenter.updateCost.text = "UpdateCost: " + updateCost;

        switch ( unitType ) {
            case BaseUnit.UnitType.swordman:
                uiViewPresenter.unitIcon.sprite = uiViewPresenter.swordIcon;
                break;
            case BaseUnit.UnitType.archer:
                uiViewPresenter.unitIcon.sprite = uiViewPresenter.archerIcon;
                break;
        }

    }

    public void HideBuildDescription () {
        uiViewPresenter.BaraksPanel.SetActive( false );
    }

    public void AddBuildActionButtons ( BuildView.UpgradeBuildingDelegate upgradeBuildingDelegate ) {
        actionButtonArray[actionButtonCounter].GetComponent<Button>().onClick.RemoveAllListeners();
        actionButtonArray[actionButtonCounter].SetActive( true );
        actionButtonArray[actionButtonCounter].GetComponent<Image>().sprite = uiViewPresenter.upgradeIcon;
        actionButtonArray[actionButtonCounter].GetComponent<Button>().onClick.AddListener( () => {
            upgradeBuildingDelegate();
        } );
        ++actionButtonCounter;
    }

    private  Action<Action<UnitViewPresenter>> currentTargetSpell;
    private  Action<Action<Vector3>> currentPositionSpell;

    public void AddHeroActionButton ( HeroUnit.ActionSpell actionSpell ) {
        Debug.Log( "init: " + actionSpell.spells[0].attackRange);
        //FIXME add buttons for each spells
        actionButtonCounter = 0;
        actionButtonArray[0].SetActive( true );
        actionButtonArray[0].GetComponent<Image>().sprite = uiViewPresenter.iceBoltIcon;
        actionButtonArray[0].GetComponent<Button>().onClick.RemoveAllListeners();
        actionButtonArray[0].GetComponent<Button>().onClick.AddListener( () => {
            int idx = 0;
            if ( actionSpell.spells[0].needTarget ) {
                Debug.Log( "Ok3" );
                cursorAction( InputController.CursorsType.TargetSpell );
                //currentTargetSpell -= currentTargetSpell;
                Action<UnitViewPresenter> currentTargetSpell = x => {
                    actionSpell.targetSpell( actionSpell.spells[idx], x );
                };

                this.currentTargetSpell( currentTargetSpell );
            } else {
                cursorAction( InputController.CursorsType.PositionSpell );
                //currentPositionSpell -= currentPositionSpell;
                Action<Vector3> currentPositionSpell = x => {
                    actionSpell.positionSpell( actionSpell.spells[idx], x );
                };
            }
        }); 

        ++actionButtonCounter;
        actionButtonArray[actionButtonCounter].SetActive( true );
        actionButtonArray[actionButtonCounter].GetComponent<Image>().sprite = uiViewPresenter.meteorShawerIcon;
        actionButtonArray[actionButtonCounter].GetComponent<Button>().onClick.RemoveAllListeners();
        actionButtonArray[actionButtonCounter].GetComponent<Button>().onClick.AddListener( () => {
            int idx = 1;
            //if ( actionSpell.spells[idx].needTarget ) {
            //    cursorAction( InputController.CursorsType.TargetSpell );
            //    currentTargetSpell -= currentTargetSpell;
            //    currentTargetSpell += x => {
            //        actionSpell.targetSpell( actionSpell.spells[idx], x );
            //    };
            //} else {
            //    cursorAction( InputController.CursorsType.PositionSpell );
            //    currentPositionSpell -= currentPositionSpell;
            //    currentPositionSpell += x => {
            //        actionSpell.positionSpell( actionSpell.spells[idx], x );
            //    };
            //}
        } );
        ++actionButtonCounter;
        actionButtonArray[actionButtonCounter].SetActive( true );
        actionButtonArray[actionButtonCounter].GetComponent<Image>().sprite = uiViewPresenter.levelUpIcon;
        ++actionButtonCounter;
    }

    public void HideActionButton () {

        for ( int i = 0; i < actionButtonArray.Length; ++i ) {
            actionButtonArray[i].SetActive( false );
        }
        actionButtonCounter = 0;
    }

    public void AddSwordIcon () {
        unitIconArray[selectUnitCounter].SetActive( true );
        unitIconArray[selectUnitCounter].GetComponent<Image>().sprite = uiViewPresenter.swordIcon;
        ++selectUnitCounter;
    }

    public void AddHeroIcon () {
        unitIconArray[selectUnitCounter].SetActive( true );
        unitIconArray[selectUnitCounter].GetComponent<Image>().sprite = uiViewPresenter.heroIcon;
        ++selectUnitCounter;
    }

    public void AddArcherIcon () {
        unitIconArray[selectUnitCounter].SetActive( true );
        unitIconArray[selectUnitCounter].GetComponent<Image>().sprite = uiViewPresenter.archerIcon;
        ++selectUnitCounter;
    }

    public void UnselectUnits () {
        for ( int i = 0; i < selectUnitCounter; ++i ) {
            unitIconArray[i].SetActive( false );
        }
        selectUnitCounter = 0;
    }

}
