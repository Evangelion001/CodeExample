using UnityEngine;
using System.Collections;

public class Player {

    public delegate void GetGold(int gold);

    private CenterUIView cuiv;

    public Player () {
        cuiv = new CenterUIView();
        cuiv.centerUIViewPresenter = GameObject.FindObjectOfType<CenterUIViewPresenter>();
    }

    public void ShowUnitsIcon () {
        cuiv.AddHeroIcon();
    }

    public void HideUnitIcon () {
        cuiv.RemoveIcons();
    }

    private int gold = 0;

    public int Gold {
        get {
            return gold;
        }
        set {
            gold += value;
            cuiv.UpdateGold( gold );
        }
    }


}
