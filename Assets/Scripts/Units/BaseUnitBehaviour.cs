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
    private TestStateMachine fsm;

    //private StateMachine<State, Trigger> fsm = new StateMachine<State, Trigger>( State.Empty );

    public BaseUnitBehaviour ( NavMeshAgent navMeshAgent, EntityController.GetTarget getTarget, EntityController.Faction faction, UnitViewPresenter myViewPresenter ) {
        myFaction = faction;
        this.myViewPresenter = myViewPresenter;
        this.navMeshAgent = navMeshAgent;
        GetTargetDelegate = getTarget;
        InitStateMachine();
    }

    private void InitStateMachine () {

        fsm = new TestStateMachine();

        fsm.SetStatePermit( TestStateMachine.States.Empty, TestStateMachine.Events.Start, TestStateMachine.States.Idle );

        fsm.SetStateEntery( TestStateMachine.States.Path, StartMove );
        fsm.AddStateUpdate( TestStateMachine.States.Path, UpdateFindGroundPosition, 0.1f );
        fsm.SetStatePermit( TestStateMachine.States.Path, TestStateMachine.Events.TargetApproached, TestStateMachine.States.Idle );
        fsm.SetStateExit( TestStateMachine.States.Path, StopMoving );

        fsm.SetStateEntery( TestStateMachine.States.Idle, StartIdle );
        fsm.AddStateUpdate( TestStateMachine.States.Idle, UpdateFindTarget, 0.5f );
        fsm.SetStatePermit( TestStateMachine.States.Idle, TestStateMachine.Events.GoToPosition, TestStateMachine.States.Path );
        fsm.SetStatePermit( TestStateMachine.States.Idle, TestStateMachine.Events.TargetFound, TestStateMachine.States.FollowTarget );
        fsm.SetStatePermit( TestStateMachine.States.Idle, TestStateMachine.Events.Dead, TestStateMachine.States.Dead );
        fsm.SetStateExit( TestStateMachine.States.Idle, StopMoving );

        fsm.SetStateEntery( TestStateMachine.States.FollowTarget, StartFollowTarget );
        fsm.AddStateUpdate( TestStateMachine.States.FollowTarget, UpdateFindTarget, 0.5f );
        fsm.AddStateUpdate( TestStateMachine.States.FollowTarget, UpdateFollowTarget, 0.1f );
        fsm.SetStatePermit( TestStateMachine.States.FollowTarget, TestStateMachine.Events.GoToPosition, TestStateMachine.States.Path );
        fsm.SetStatePermit( TestStateMachine.States.FollowTarget, TestStateMachine.Events.TargetApproached, TestStateMachine.States.Action );
        fsm.SetStatePermit( TestStateMachine.States.FollowTarget, TestStateMachine.Events.Dead, TestStateMachine.States.Dead );
        fsm.SetStateExit( TestStateMachine.States.FollowTarget, StopMoving );

        fsm.SetStateEntery( TestStateMachine.States.Action, StartAttack );
        fsm.AddStateUpdate( TestStateMachine.States.Action, UpdateAttack, 0.1f );
        fsm.SetStatePermit( TestStateMachine.States.Action, TestStateMachine.Events.GoToPosition, TestStateMachine.States.Path );
        fsm.SetStatePermit( TestStateMachine.States.Action, TestStateMachine.Events.TargetLost, TestStateMachine.States.Idle );
        fsm.SetStatePermit( TestStateMachine.States.Action, TestStateMachine.Events.Dead, TestStateMachine.States.Dead );
        fsm.SetStateExit( TestStateMachine.States.Action, StopMoving );

        fsm.SetStateEntery( TestStateMachine.States.Dead, OnDeadEnter );

        fsm.CallEvent( TestStateMachine.Events.Start );
    }

    public void SetTargetPosition (Vector3 targetPosition ) {
        this.targetPosition = targetPosition;
        //fsm.Fire( Trigger.GoToPosition );
        fsm.CallEvent( TestStateMachine.Events.GoToPosition );
    }

    private void StartMove () {
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination( targetPosition );
    }
 
    private void UpdateFindTarget () {
        UnitViewPresenter tempTarget = GetTargetDelegate( myFaction );

        if ( tempTarget != targetViewPresenter ) {
            targetViewPresenter = tempTarget;
            if ( targetViewPresenter != null ) {
                fsm.CallEvent( TestStateMachine.Events.TargetFound );
                //fsm.Fire( Trigger.TargetFound );
            }
        }

    }

    private void UpdateFindGroundPosition () {

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, myViewPresenter.transform.position ) <= 0.1f ) {
            fsm.CallEvent( TestStateMachine.Events.TargetApproached );
            //fsm.Fire( Trigger.TargetApproached );
        }

    }

    private void UpdateFollowPath () {

    }

    private void UpdateFollowTarget () {

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, myViewPresenter.transform.position ) <= 2f ) {
            fsm.CallEvent( TestStateMachine.Events.TargetApproached );
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

        if ( Vector3.Distance( targetViewPresenter.transform.position, myViewPresenter.transform.position ) > 2f ) {
            fsm.CallEvent( TestStateMachine.Events.TargetLost );
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
