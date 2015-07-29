using UnityEngine;
using Stateless;
using System.Collections;

public class BaseUnitBehaviour {

    private NavMeshAgent navMeshAgent;
    private UnitViewPresenter targetViewPresenter;
    private UnitViewPresenter myViewPresenter;
    private Vector3 targetPosition;
    private EntityController.GetTarget GetTargetDelegate;
    private EntityController.Faction myFaction;
    private FiniteStateMachine fsm;
    private AnimationController animationController;
    private float attackSpeed = 0;
    private float attackRange = 0;
    private Influence influence;
    private bool canAttack = true;
    private UnitViewPresenter playerTarget = null; 
    public bool isSelected = false;

    //private StateMachine<State, Trigger> fsm = new StateMachine<State, Trigger>( State.Empty );

    public BaseUnitBehaviour ( EntityController.GetTarget getTarget, EntityController.Faction faction, UnitViewPresenter myViewPresenter, AnimationController animationController) {
        this.myViewPresenter = myViewPresenter;
        this.animationController = animationController;
        myFaction = faction;
        this.navMeshAgent = myViewPresenter.navMeshAgent;
        GetTargetDelegate = getTarget;
        InitStateMachine();
    }

    public void ShowTarget () {
        if ( targetViewPresenter != null && isSelected ) {
            Debug.Log( "12" );
            targetViewPresenter.ShowTarget();
        }
    }

    public void HideTarget () {
        if ( targetViewPresenter != null ) {
            targetViewPresenter.HideTarget();
        }
    }

    private void InitStateMachine () {

        fsm = new FiniteStateMachine();

        fsm.SetStatePermit( FiniteStateMachine.States.Empty, FiniteStateMachine.Events.Start, FiniteStateMachine.States.Idle );

        fsm.SetStateEntery( FiniteStateMachine.States.Path, StartMove );
        fsm.AddStateUpdate( FiniteStateMachine.States.Path, UpdateFindGroundPosition, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.TargetApproached, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.TargetFound, FiniteStateMachine.States.FollowTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.Path, StopMoving );

        fsm.SetStateEntery( FiniteStateMachine.States.Idle, StartIdle );
        fsm.AddStateUpdate( FiniteStateMachine.States.Idle, UpdateFindTarget, 0.5f );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.TargetFound, FiniteStateMachine.States.FollowTarget );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.Idle, StopMoving );

        fsm.SetStateEntery( FiniteStateMachine.States.FollowTarget, StartFollowTarget );
        fsm.AddStateUpdate( FiniteStateMachine.States.FollowTarget, UpdateFindTarget, 0.5f );
        fsm.AddStateUpdate( FiniteStateMachine.States.FollowTarget, UpdateFollowTarget, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.TargetApproached, FiniteStateMachine.States.Action );
        fsm.SetStatePermit( FiniteStateMachine.States.FollowTarget, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.FollowTarget, StopMoving );

        fsm.SetStateEntery( FiniteStateMachine.States.Action, StartAttack );
        fsm.AddStateUpdate( FiniteStateMachine.States.Action, UpdateAttack, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.Action, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
        fsm.SetStatePermit( FiniteStateMachine.States.Action, FiniteStateMachine.Events.TargetLost, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.Action, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );
        fsm.SetStateExit( FiniteStateMachine.States.Action, StopAttack );

        fsm.SetStateEntery( FiniteStateMachine.States.Dead, OnDeadEnter );
        fsm.SetStatePermit( FiniteStateMachine.States.Dead, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );

        fsm.CallEvent( FiniteStateMachine.Events.Start );
    }

    public void SetTargetPosition (Vector3 targetPosition ) {
        this.targetPosition = targetPosition;
        //fsm.Fire( Trigger.GoToPosition );
        fsm.CallEvent( FiniteStateMachine.Events.GoToPosition );
    }

    private void StartMove () {
        animationController.RunAnimation();
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination( targetPosition );
    }

    public void SetPlayerTarget ( UnitViewPresenter unitViewPresenter ) {
        playerTarget = unitViewPresenter;
        fsm.CallEvent( FiniteStateMachine.Events.TargetFound );
        if ( playerTarget != targetViewPresenter ) {
            HideTarget();
        }    
    }

    private void UpdateFindTarget () {

        UnitViewPresenter tempTarget = GetTargetDelegate( myFaction, myViewPresenter );

        if ( playerTarget != null ) {
            tempTarget = playerTarget;
        }

        if ( tempTarget != targetViewPresenter ) {
            HideTarget();
            targetViewPresenter = tempTarget;
            if ( targetViewPresenter != null ) {
                ShowTarget();
                fsm.CallEvent( FiniteStateMachine.Events.TargetFound );
                //fsm.Fire( Trigger.TargetFound );
            }
        }

    }

    private void UpdateFindGroundPosition () {

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, myViewPresenter.transform.position ) <= 0.1f ) {
            fsm.CallEvent( FiniteStateMachine.Events.TargetApproached );
            //fsm.Fire( Trigger.TargetApproached );
        }

    }

    private void UpdateFollowPath () {

    }

    private void UpdateFollowTarget () {

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, myViewPresenter.transform.position ) <= attackRange ) {
            fsm.CallEvent( FiniteStateMachine.Events.TargetApproached );
            //fsm.Fire( Trigger.TargetApproached );
            return;
        }

        if ( targetViewPresenter != null && navMeshAgent.pathEndPosition != targetViewPresenter.transform.position ) {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination( targetViewPresenter.transform.position );
        }
    }

    private void StartIdle () {
        animationController.IdleAnimation();
        targetViewPresenter = null;
        navMeshAgent.ResetPath();
    }

    private void StartFollowTarget () {
        animationController.RunAnimation();
        if ( targetViewPresenter != null ) {
            navMeshAgent.SetDestination( targetViewPresenter.transform.position );
        }
    }

    private void StopMoving () {
        animationController.IdleAnimation();
        navMeshAgent.ResetPath();
    }

    private void UpdateAttack () {
        //Debug.Log( "UpdateAttack " + myViewPresenter.name );
        if ( targetViewPresenter == null || Vector3.Distance( targetViewPresenter.transform.position, myViewPresenter.transform.position ) > attackRange ) {
            fsm.CallEvent( FiniteStateMachine.Events.TargetLost );
            //fsm.Fire( Trigger.TargetLost );
            return;
        }

        myViewPresenter.transform.LookAt( targetViewPresenter.transform.position );

        if ( canAttack ) {
            animationController.AttackAnimation();
            influence.owner = myViewPresenter;
            targetViewPresenter.GetDamage( influence );
            canAttack = false;
            SceneManager.Instance.CoroutineManager.InvokeAttack( EndOfDelayAttack, attackSpeed );
        }

    }

    private void EndOfDelayAttack () {
        canAttack = true;
    }

    public void SetAttackParam (float attackSpeed, float attackRange) {
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public void SetInfluence (Influence influence ) {
        this.influence = influence;
    }

    public void CallDeathFSMEvent () {
        fsm.CallEvent( FiniteStateMachine.Events.Dead );
    }

    private void StartAttack () {
        if ( targetViewPresenter != null ) {
            myViewPresenter.transform.LookAt( targetViewPresenter.transform.position );
        }
    }

    private void StopAttack () {
        Debug.Log( "StopAttack" );
        animationController.IdleAnimation();
        targetViewPresenter = null;
        navMeshAgent.ResetPath();
    }

    private void OnDeadEnter () {
        Debug.Log( "OnDeadEnter " + myViewPresenter.transform.name );
    }

}
