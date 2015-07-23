using UnityEngine;
using Stateless;
using System.Collections;

public class BaseUnitBehaviour: MonoBehaviour {

    public enum State {
        Empty,
        Idle,
        FollowPath,
        Action,
        Dead
    }

    public enum Trigger {
        Start,
        TargetFound,
        TargetApproached,
        TargetLost,
        Dead
    }

    private NavMeshAgent navMeshAgent;
    private UnitViewPresenter targetUnit;
    private Vector3 targetPosition;
    private EntityController.GetTarget GetTargetDelegate;
    private EntityController.Faction myFaction;

    private StateMachine<State, Trigger> fsm = new StateMachine<State, Trigger>( State.Empty );

    public void Init (NavMeshAgent navMeshAgent, EntityController.GetTarget getTarget, EntityController.Faction faction ) {
        this.navMeshAgent = navMeshAgent;
        GetTargetDelegate = getTarget;
        myFaction = faction;
        InitStateMachine();
    }

    private void InitStateMachine () {
        fsm.Configure( State.Empty ).Permit( Trigger.Start, State.Idle );
        fsm.Configure( State.Idle )
            .OnUpdate( this, "UpdateFindTarget", 0.1f )
            .OnEntry( StartIdle )
            .Permit( Trigger.TargetFound, State.FollowPath )
            .Permit( Trigger.Dead, State.Dead );

        fsm.Configure( State.FollowPath )
            .OnUpdate( this, "UpdateFindTarget", 0.1f )
            .OnUpdate( this, "UpdateFollowTarget", 0.5f )
            .OnEntry( StartFollowTarget )
            .OnExit( StopMoving )
            .Permit( Trigger.TargetApproached, State.Action )
            .Permit( Trigger.TargetLost, State.Idle )
            .Permit( Trigger.Dead, State.Dead )
            .PermitReentry( Trigger.TargetFound );

        fsm.Configure( State.Action )
            .OnEntry( StartAttack )
            .OnExit( StopAttack )
            .OnUpdate( this, "UpdateAttack", 0.5f )
            .Permit( Trigger.TargetLost, State.Idle )
            .Permit( Trigger.Dead, State.Dead );

        fsm.Configure( State.Dead )
            .OnEntry( OnDeadEnter );

        fsm.Fire( Trigger.Start );
    }

    public void SetTargetPosition (Vector3 targetPosition ) {
        this.targetPosition = targetPosition;
        //fsm.Fire( Trigger.PlayerInputPosition );
    }

    private void UpdateFindTarget () {
        
        Debug.Log( "UpdateFindTarget" );

        UnitViewPresenter tempTarget = GetTargetDelegate( myFaction );

        if ( targetUnit != null ) {
            fsm.Fire( Trigger.TargetFound );
        }

    }

    private void UpdateMovingToTarget () {
        Debug.Log( "UpdateMovingToTarget" );

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, transform.position ) < 0.1f ) {
            Debug.Log( "TargetApproached" );
            fsm.Fire( Trigger.TargetApproached );
        }
    }


    private void UpdateFollowTarget () {
        Debug.Log( "UpdateFollowTarget" );
        if ( Vector3.Distance( navMeshAgent.pathEndPosition, transform.position ) < 0.1f ) {
            Debug.Log( "TargetApproached" );
            fsm.Fire( Trigger.TargetApproached );
        }
    }

    private void StartIdle () {
        Debug.Log( "StartIdle" );
    }

    private void StartGoToPosition () {
        Debug.Log( "StartGoToPosition" );
        navMeshAgent.SetDestination( targetPosition );
    }

    private void StartFollowTarget () {
        Debug.Log( "StartFollowTarget" );
        navMeshAgent.SetDestination( targetUnit.transform.position );
    }

    private void StopMoving () {
        Debug.Log( "StopMoving" );
        navMeshAgent.Stop();
    }

    private void UpdateAttack () {
        Debug.Log( "UpdateAttack" );
    }

    private void StartAttack () {
        Debug.Log( "StartAttack" );
    }

    private void StopAttack () {
        Debug.Log( "StopAttack" );
    }

    private void OnDeadEnter () {
        Debug.Log( "OnDeadEnter" );
    }

}
