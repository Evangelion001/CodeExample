using UnityEngine;
using UnityEngine.UI;

public class CenterUIView {

    private CenterUIViewPresenter uiViewPresenter;

    public void UpdateGold (int gold) {
        uiViewPresenter.goldValue.text = gold.ToString();
    }

    private int selectUnitCounter = 0;

    public CenterUIView ( CenterUIViewPresenter uiViewPresenter ) {
        this.uiViewPresenter = uiViewPresenter;
        unitIconArray = new GameObject[14];
        for ( int i = 0; i < unitIconArray.Length; ++i ) {
            unitIconArray[i] = GameObject.Instantiate( uiViewPresenter.unitIconPrefab );
            unitIconArray[i].transform.SetParent( uiViewPresenter.gridUnitIcons.transform, false );
            unitIconArray[i].SetActive( false );
        }
    }

    private GameObject[] unitIconArray;

    public void AddSwordIcon () {
        unitIconArray[selectUnitCounter].SetActive( true );
        unitIconArray[selectUnitCounter].GetComponent<Image>().sprite = uiViewPresenter.swordIcon;
    }

    public void AddHeroIcon () {
        unitIconArray[selectUnitCounter].SetActive( true );
        unitIconArray[selectUnitCounter].GetComponent<Image>().sprite = uiViewPresenter.heroIcon;
    }

    public void AddArcherIcon () {
        unitIconArray[selectUnitCounter].SetActive( true );
        unitIconArray[selectUnitCounter].GetComponent<Image>().sprite = uiViewPresenter.archerIcon;
    }

    public void UnselectUnits () {
        for ( int i = 0; i < selectUnitCounter; ++i ) {
            unitIconArray[selectUnitCounter].SetActive( false );
        }
        selectUnitCounter = 0;
    }

}
