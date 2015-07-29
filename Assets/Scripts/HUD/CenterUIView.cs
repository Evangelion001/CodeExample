using UnityEngine;
using UnityEngine.UI;

public class CenterUIView {

    private CenterUIViewPresenter uiViewPresenter;

    public void UpdateGold (int gold) {
        uiViewPresenter.goldValue.text = gold.ToString();
    }

    private int selectUnitCounter = 0;
    private int actionButtonCounter = 0;

    private GameObject[] actionButtonArray = new GameObject[6];
    private GameObject[] unitIconArray = new GameObject[14];

    public CenterUIView ( CenterUIViewPresenter uiViewPresenter ) {
        this.uiViewPresenter = uiViewPresenter;

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

    public void ShowBuildDescription (int level, BaseUnit.UnitType unitType, int timeRecruitment, int updateCost ) {
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

    public void AddBuildActionButtons () {
        actionButtonArray[actionButtonCounter].SetActive( true );
        actionButtonArray[actionButtonCounter].GetComponent<Image>().sprite = uiViewPresenter.upgradeIcon;
        ++actionButtonCounter;
    }

    public void AddHeroActionButton () {
        actionButtonArray[actionButtonCounter].SetActive( true );
        actionButtonArray[actionButtonCounter].GetComponent<Image>().sprite = uiViewPresenter.iceBoltIcon;
        ++actionButtonCounter;
        actionButtonArray[actionButtonCounter].SetActive( true );
        actionButtonArray[actionButtonCounter].GetComponent<Image>().sprite = uiViewPresenter.meteorShawerIcon;
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
