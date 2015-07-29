using UnityEngine;
using System.Collections;

public class HeroView : BaseUnitView {

    public HeroView ( HeroViewPresentor unitViewPresenter, BaseUnitController.SelectUnit selectUnit, BaseUnit.DamageDelegate damageDelegate, HeroUnit.GetXpDelegate GetXpDelegate ) :base(
        unitViewPresenter, selectUnit, damageDelegate
        ) {
        unitViewPresenter.AddXpDelegate( GetXpDelegate );
    }

    public void ShowLevelUp () {
        ( (HeroViewPresentor)unitViewPresenter ).levelUpEffect.Play();
    }

}
