using UnityEngine;

public class CenterUIView {

    public CenterUIViewPresenter centerUIViewPresenter;

    public GameObject[] iconArray = new GameObject[14];

    private void AddIconOnGrid (GameObject iconPrefab) {
        GameObject temp = (GameObject)GameObject.Instantiate( iconPrefab );

        temp.transform.SetParent( centerUIViewPresenter.gridUnitIcons.transform, false );

        iconArray[0] = temp;
    }

    public void AddSwordIcon () {
        AddIconOnGrid( centerUIViewPresenter.prefabSwordIconPrefab );
    }

    public void AddHeroIcon () {
        AddIconOnGrid( centerUIViewPresenter.prefabHeroIconPrefab );
    }

    public void RemoveIcons () {
        foreach ( var key in iconArray ) {
            GameObject.Destroy(key);
        }
    }

}
