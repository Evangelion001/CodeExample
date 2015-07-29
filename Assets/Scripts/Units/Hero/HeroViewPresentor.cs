using UnityEngine;
using System.Collections;

public class HeroViewPresentor : UnitViewPresenter {

    private HeroUnit.GetXpDelegate GetXpDelegate;

    public ParticleSystem levelUpEffect;

    public void AddXpDelegate ( HeroUnit.GetXpDelegate GetXpDelegate ) {
        this.GetXpDelegate = GetXpDelegate;
    }

    public void GetXP ( int xp ) {
        GetXpDelegate( xp );
    }
}
