using UnityEngine;
using System.Collections;

public class HeroBehaviour : BaseUnitBehaviour {

    public HeroBehaviour ( EntityController.GetTarget getTarget, EntityController.Faction faction, UnitViewPresenter myViewPresenter, AnimationController animationController ):base( 
        getTarget, faction, myViewPresenter, animationController ) {

        InitStateMachine();
    }

    private Vector3 spellTargetPosition;

    private UnitViewPresenter spellTarget;

    private bool spellWithTarget = false;

    private Spell spell;

    public override void InitStateMachine () {
        fsm = new FiniteStateMachine();

        fsm.SetStatePermit( FiniteStateMachine.States.Empty, FiniteStateMachine.Events.Start, FiniteStateMachine.States.Idle );

        fsm.SetStateEntery( FiniteStateMachine.States.Path, StartMove );
        fsm.AddStateUpdate( FiniteStateMachine.States.Path, UpdateFindGroundPosition, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.TargetApproached, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.TargetFound, FiniteStateMachine.States.FollowTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.ActivateSpell, FiniteStateMachine.States.FollowSpellTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.Path, StopMoving );

        fsm.SetStateEntery( FiniteStateMachine.States.Idle, StartIdle );
        fsm.AddStateUpdate( FiniteStateMachine.States.Idle, UpdateFindTarget, 0.5f );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.TargetFound, FiniteStateMachine.States.FollowTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.ActivateSpell, FiniteStateMachine.States.FollowSpellTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.Idle, StopMoving );

        fsm.SetStateEntery( FiniteStateMachine.States.FollowTarget, StartFollowTarget );
        fsm.AddStateUpdate( FiniteStateMachine.States.FollowTarget, UpdateFindTarget, 0.5f );
        fsm.AddStateUpdate( FiniteStateMachine.States.FollowTarget, UpdateFollowTarget, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.TargetLost, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.TargetApproached, FiniteStateMachine.States.Action );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.ActivateSpell, FiniteStateMachine.States.FollowSpellTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.FollowTarget, StopMoving );

        fsm.SetStateEntery( FiniteStateMachine.States.Action, StartAttack );
        fsm.AddStateUpdate( FiniteStateMachine.States.Action, UpdateAttack, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.Action, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.Action, FiniteStateMachine.Events.ActivateSpell, FiniteStateMachine.States.FollowSpellTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.Action, FiniteStateMachine.Events.TargetLost, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.Action, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.Action, StopAttack );

        fsm.SetStateEntery( FiniteStateMachine.States.FollowSpellTarget, StartFollowSpellTarget );
        fsm.AddStateUpdate( FiniteStateMachine.States.FollowSpellTarget, UpdateSpellTarget, 0.5f );
        fsm.AddStateUpdate( FiniteStateMachine.States.FollowSpellTarget, UpdateFollowSpellTarget, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowSpellTarget, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowSpellTarget, FiniteStateMachine.Events.TargetApproached, FiniteStateMachine.States.UseSpell );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowSpellTarget, FiniteStateMachine.Events.Cancel, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowSpellTarget, FiniteStateMachine.Events.ActivateSpell, FiniteStateMachine.States.FollowSpellTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowSpellTarget, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.FollowSpellTarget, StopSpellFollow );

        fsm.SetStateEntery( FiniteStateMachine.States.UseSpell, ActivateSpell );
        fsm.SetStatePermit( FiniteStateMachine.States.UseSpell, FiniteStateMachine.Events.TargetApproached, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.UseSpell, FiniteStateMachine.Events.TargetLost, FiniteStateMachine.States.FollowSpellTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.UseSpell, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.UseSpell, StopAttack );

        fsm.SetStateEntery( FiniteStateMachine.States.Dead, OnDeadEnter );
        fsm.SetStatePermit( FiniteStateMachine.States.Dead, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStatePermit( FiniteStateMachine.States.Dead, FiniteStateMachine.Events.Start, FiniteStateMachine.States.Idle );

        fsm.CallEvent( FiniteStateMachine.Events.Start );
    }

    public override void UpdateFindTarget () {

        if ( playerTarget != null && playerTarget != targetViewPresenter ) {
            HideTarget();
            targetViewPresenter = playerTarget;
            if ( targetViewPresenter != null ) {
                ShowTarget();
                fsm.CallEvent( FiniteStateMachine.Events.TargetFound );
            }
        }

    }

    private void UpdateSpellTarget () {

        if ( spellWithTarget && spellTarget == null ) {
            fsm.CallEvent( FiniteStateMachine.Events.Cancel );
        }

    }

    private void StopSpellFollow () {

        navMeshAgent.ResetPath();

    }

    private void UpdateFollowSpellTarget () {

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, myViewPresenter.transform.position ) <= spell.attackRange ) {
            fsm.CallEvent( FiniteStateMachine.Events.TargetApproached );
            return;
        }

        if ( spellWithTarget && spellTarget != null && navMeshAgent.pathEndPosition != spellTarget.transform.position ) {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination( spellTarget.transform.position );
        }

    }

    private void ActivateSpell () {

        if ( spellWithTarget && spellTarget == null || Vector3.Distance( spellTarget.transform.position, myViewPresenter.transform.position ) > spell.attackRange ) {
            fsm.CallEvent( FiniteStateMachine.Events.TargetLost );
            return;
        }

        Vector3 tempPos = spellTargetPosition;

        if ( spellTarget != null ) {
            tempPos = spellTarget.transform.position;
        }

        myViewPresenter.transform.LookAt( tempPos );

        animationController.AttackAnimation();

        Influence temp = new Influence();
        temp.damage = 0;
        temp.healing = 0;
        temp.owner = myViewPresenter;

        temp.timeEffect = spell.effect;

        spellTarget.GetDamage( temp );

        fsm.CallEvent( FiniteStateMachine.Events.TargetApproached );
    }

    private void StopSpellAttack () {
        animationController.IdleAnimation();
        spellTarget = null;
        spellWithTarget = false;
        navMeshAgent.ResetPath();
    }

    public void PlayerUseAbility ( Spell spell, Vector3 spellTargetPosition  ) {
        spellWithTarget = false;
        this.spellTargetPosition = spellTargetPosition;
        this.spell = spell;
        fsm.CallEvent( FiniteStateMachine.Events.ActivateSpell );
    }

    public void PlayerUseAbility ( Spell spell, UnitViewPresenter spellTarget ) {
        spellWithTarget = true;
        this.spellTarget = spellTarget;
        this.spell = spell;
        fsm.CallEvent( FiniteStateMachine.Events.ActivateSpell );
    }

    private void StartFollowSpellTarget () {
        animationController.RunAnimation();

        Vector3 tempPos = spellTargetPosition;

        if ( spellTarget != null ) {
            tempPos = spellTarget.transform.position;
        }

        navMeshAgent.SetDestination( tempPos );
    }

    public override void OnDeadEnter () {
        
    }

    public void Resurrect () {
        fsm.CallEvent( FiniteStateMachine.Events.Start );
    }

}
