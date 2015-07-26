using UnityEngine;
using Stateless;
using System.Collections;

public class BaseUnitBehaviour {

    public NavMeshAgent navMeshAgent;
    private UnitViewPresenter targetViewPresenter;
    private UnitViewPresenter myViewPresenter;
    private Vector3 targetPosition;
    private EntityController.GetTarget GetTargetDelegate;
    private EntityController.Faction myFaction;
    private FiniteStateMachine fsm;

    //private StateMachine<State, Trigger> fsm = new StateMachine<State, Trigger>( State.Empty );

    public BaseUnitBehaviour ( NavMeshAgent navMeshAgent, EntityController.GetTarget getTarget, EntityController.Faction faction, UnitViewPresenter myViewPresenter ) {
        myFaction = faction;
        this.myViewPresenter = myViewPresenter;
        this.navMeshAgent = navMeshAgent;
        GetTargetDelegate = getTarget;
        InitStateMachine();
    }

    private void InitStateMachine () {

        fsm = new FiniteStateMachine();

        fsm.SetStatePermit( FiniteStateMachine.States.Empty, FiniteStateMachine.Events.Start, FiniteStateMachine.States.Idle );

        fsm.SetStateEntery( FiniteStateMachine.States.Path, StartMove );
        fsm.AddStateUpdate( FiniteStateMachine.States.Path, UpdateFindGroundPosition, 0.1f );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.TargetApproached, FiniteStateMachine.States.Idle );
        fsm.SetStatePermit( FiniteStateMachine.States.Path, FiniteStateMachine.Events.GoToPosition, FiniteStateMachine.States.Path );
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

        fsm.CallEvent( FiniteStateMachine.Events.Start );
    }

    public void SetTargetPosition (Vector3 targetPosition ) {
        this.targetPosition = targetPosition;
        //fsm.Fire( Trigger.GoToPosition );
        fsm.CallEvent( FiniteStateMachine.Events.GoToPosition );
    }

    private void StartMove () {
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination( targetPosition );
    }
 
    private void UpdateFindTarget () {
        UnitViewPresenter tempTarget = GetTargetDelegate( myFaction, myViewPresenter );

        if ( tempTarget != targetViewPresenter ) {
            targetViewPresenter = tempTarget;
            if ( targetViewPresenter != null ) {
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

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, myViewPresenter.transform.position ) <= 3f ) {
            fsm.CallEvent( FiniteStateMachine.Events.TargetApproached );
            //fsm.Fire( Trigger.TargetApproached );
            return;
        }

        if ( navMeshAgent.pathEndPosition != targetViewPresenter.transform.position ) {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination( targetViewPresenter.transform.position );
        }
    }

    private void StartIdle () {
        Debug.Log( "StartIdle" );
        targetViewPresenter = null;
        navMeshAgent.ResetPath();
    }

    private void StartFollowTarget () {
        navMeshAgent.SetDestination( targetViewPresenter.transform.position );
    }

    private void StopMoving () {
        navMeshAgent.ResetPath();
    }

    private void UpdateAttack () {
        if ( Vector3.Distance( targetViewPresenter.transform.position, myViewPresenter.transform.position ) > 3f ) {
            fsm.CallEvent( FiniteStateMachine.Events.TargetLost );
            //fsm.Fire( Trigger.TargetLost );
            return;
        }

    }

    private void StartAttack () {
        myViewPresenter.transform.LookAt( targetViewPresenter.transform.position );
    }

    private void StopAttack () {
        Debug.Log( "StopAttack" );
        targetViewPresenter = null;
        navMeshAgent.ResetPath();
    }

    private void OnDeadEnter () {
        Debug.Log( "OnDeadEnter" );
    }

}
