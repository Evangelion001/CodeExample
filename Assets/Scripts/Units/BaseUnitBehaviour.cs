using UnityEngine;
using Stateless;
using System.Collections;

public class BaseUnitBehaviour: MonoBehaviour {

    public enum State {
        Empty,
        Idle,
        FollowTarget,
        Action,
        Path,
        Dead
    }

    public enum Trigger {
        Start,
        TargetFound,
        TargetApproached,
        TargetLost,
        GoToPosition,
        Dead
    }

    private NavMeshAgent navMeshAgent;
    private UnitViewPresenter targetUnit;
    private Vector3 targetPosition;
    private EntityController.GetTarget GetTargetDelegate;
    private EntityController.Faction myFaction;
    private bool playerOrder = false;

    private StateMachine<State, Trigger> fsm = new StateMachine<State, Trigger>( State.Empty );

    public void Init (NavMeshAgent navMeshAgent, EntityController.GetTarget getTarget, EntityController.Faction faction ) {
        myFaction = faction;
        this.navMeshAgent = navMeshAgent;
        GetTargetDelegate = getTarget;
        InitStateMachine();
    }

    private void InitStateMachine () {
        //fsm.Configure( State.Empty ).Permit( Trigger.Start, State.Idle );

        //fsm.Configure( State.Path )
        //    .OnEntry( StartMove )
        //    .OnUpdate( this, "UpdateFindGroundPosition", 0.5f, 0.1f )
        //    .Permit( Trigger.TargetApproached, State.Idle )
        //    .PermitReentry( Trigger.GoToPosition );

        //fsm.Configure( State.Idle )
        //    .OnUpdate( this, "UpdateFindTarget", 0.1f )
        //    .OnEntry( StartIdle )
        //    .Permit( Trigger.GoToPosition, State.Path )
        //    .Permit( Trigger.TargetFound, State.FollowTarget )
        //    .Permit( Trigger.Dead, State.Dead );

        //fsm.Configure( State.FollowTarget )
        //    .OnUpdate( this, "UpdateFindTarget", 0.1f )
        //    .OnUpdate( this, "UpdateFollowTarget", 0.1f )
        //    .OnEntry( StartFollowTarget )
        //    .OnExit( StopMoving )
        //    .Permit( Trigger.GoToPosition, State.Path )
        //    .Permit( Trigger.TargetApproached, State.Action )
        //    .Permit( Trigger.TargetLost, State.Idle )
        //    .Permit( Trigger.Dead, State.Dead )
        //    .PermitReentry( Trigger.TargetFound );

        //fsm.Configure( State.Action )
        //    .OnEntry( StartAttack )
        //    .OnExit( StopAttack )
        //    .OnUpdate( this, "UpdateAttack", 0.5f )
        //    .Permit( Trigger.TargetLost, State.Idle )
        //    .Permit( Trigger.GoToPosition, State.Path )
        //    .Permit( Trigger.Dead, State.Dead );


        //fsm.Configure( State.Dead )
        //    .OnEntry( OnDeadEnter );

        //fsm.Fire( Trigger.Start );

        TestStateMachine tsm = new TestStateMachine();

        tsm.SetStatePermit( TestStateMachine.States.Empty, TestStateMachine.Events.Start, TestStateMachine.States.Idle );

        tsm.SetStateEntery( TestStateMachine.States.Idle, TestMethod );
        tsm.AddStateUpdate( TestStateMachine.States.Idle, TestMethod, 0.5f );

        tsm.CallEvent( TestStateMachine.Events.Start );
    }

    bool testBool = false;

    private void OutMethod () {
        Debug.Log( "OutMethod " + transform.name );
        if ( !testBool ) {
            testBool = true;
            Debug.Log( "OutMethod " + true + "  "+transform.name );
        }
    }


    private void TestMethod () {
        OutMethod();
    }

    public void SetTargetPosition (Vector3 targetPosition ) {
        this.targetPosition = targetPosition;
        fsm.Fire( Trigger.GoToPosition );
    }

    private void StartMove () {
        Debug.Log( "StartMove " + targetPosition );
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination( targetPosition );
    }
 
    private void UpdateFindTarget () {

        UnitViewPresenter tempTarget = GetTargetDelegate( myFaction );

        if ( tempTarget != targetUnit ) {
            targetUnit = tempTarget;
            if ( targetUnit != null ) {

                fsm.Fire( Trigger.TargetFound );
            }
        }

    }

    private void UpdateFindGroundPosition () {

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, transform.position ) <= 0.1f ) {
            Debug.Log( "navMeshAgent.pathEndPosition: " + navMeshAgent.pathEndPosition );
            Debug.Log( "TargetApproached " + transform.name );
            fsm.Fire( Trigger.TargetApproached );
        }

    }

    private void UpdateFollowPath () {

    }

    private void UpdateFollowTarget () {
        Debug.Log( "UpdateFollowTarget " + transform.name );

        if ( Vector3.Distance( navMeshAgent.pathEndPosition, transform.position ) <= 3f ) {
            Debug.Log( "TargetApproached " + transform.name );
            fsm.Fire( Trigger.TargetApproached );
            return;
        }

        if ( navMeshAgent.pathEndPosition != targetUnit.transform.position ) {
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination( targetUnit.transform.position );
        }
    }

    private void StartIdle () {
        Debug.Log( "StartIdle" );
        targetUnit = null;
        navMeshAgent.ResetPath();
    }

    private void StartFollowTarget () {
        Debug.Log( "StartFollowTarget" );
        navMeshAgent.SetDestination( targetUnit.transform.position );
    }

    private void StopMoving () {
        Debug.Log( "StopMoving " + transform.name );
        navMeshAgent.ResetPath();
    }

    private void UpdateAttack () {
        Debug.Log( "UpdateAttack " + transform.name );

        if ( Vector3.Distance( targetUnit.transform.position, transform.position ) > 3f ) {
            fsm.Fire( Trigger.TargetLost );
            return;
        }

    }

    private void StartAttack () {
        transform.LookAt( targetUnit.transform.position );
        Debug.Log( "StartAttack " + transform.name );
    }

    private void StopAttack () {
        Debug.Log( "StopAttack" );
        targetUnit = null;
        navMeshAgent.ResetPath();
    }

    private void OnDeadEnter () {
        Debug.Log( "OnDeadEnter" );
    }

}
