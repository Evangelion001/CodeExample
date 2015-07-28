using UnityEngine;
using System.Collections;

public class AnimationController {

    public Animation animation;

    public AnimationController ( Animation animation ) {
        this.animation = animation;
    }

    public void IdleAnimation () {
        if ( animation != null ) {
            animation.Play( "Idle" );
        }
    }

    public void RunAnimation () {
        if ( animation != null ) {
            animation.Play( "Run" );
        }
    }

    public void AttackAnimation () {
        if ( animation != null ) {
            animation.Play( "Attack" );
        }
    }

}
