using UnityEngine;
using System.Collections;
using System;
using System.Linq;
//using MoreLinq;
//using Stateless;
using System.Collections.Generic;

public class CreepAI {

//public abstract class CreepAI :BaseAI {

//    public enum State {
//        Empty,
//        Idle,
//        FollowTarget,
//        Attack,
//        Dead
//    }

//    public enum Trigger {
//        Start,
//        TargetFound,
//        TargetApproached,
//        TargetLost,
//        Dead
//    }

//    private StateMachine<State, Trigger> fsm = new StateMachine<State, Trigger>( State.Empty );
//    //	private PlayMakerFSM fsm;

//    public Transform mainTarget;

//    //protected RepeatTrigger targetCheck = new RepeatTrigger(1f);

//    protected MoveController moveController;
//    protected AttackController attackController;

//    private float pathRefindDistanceSqr = 5 * 5; // if target moved 5 meters while we follow - find new way to it

//    [HideInInspector]
//    public Transform target;

//    protected HashSet<GameObject> enemies = new HashSet<GameObject>();

//    protected override void AwakeEntity () {
//        base.AwakeEntity();

//        fsm.Configure( State.Empty ).Permit( Trigger.Start, State.Idle );

//        fsm.Configure( State.Idle )
//            .OnUpdate( this, "UpdateFindTarget", 0.1f )
//            .OnEntry( StartIdle )
//            .Permit( Trigger.TargetFound, State.FollowTarget )
//            .Permit( Trigger.Dead, State.Dead );

//        fsm.Configure( State.FollowTarget )
//            .OnUpdate( this, "UpdateFindTarget", 0.1f )
//            .OnUpdate( this, "UpdateFollowTarget", 0.5f )
//            .OnEntry( StartFollowTarget )
//            .OnExit( StopFollowTarget )
//            .Permit( Trigger.TargetApproached, State.Attack )
//            .Permit( Trigger.TargetLost, State.Idle )
//            .Permit( Trigger.Dead, State.Dead )
//            .PermitReentry( Trigger.TargetFound );

//        fsm.Configure( State.Attack )
//            .OnEntry( StartAttack )
//            .OnExit( StopAttack )
//            .OnUpdate( this, "UpdateAttack", 0.5f )
//            .Permit( Trigger.TargetLost, State.Idle )
//            .Permit( Trigger.Dead, State.Dead );

//        fsm.Configure( State.Dead )
//            .OnEntry( OnDeadEnter );

//        // log state changes
//        //		fsm.OnTransitioned(t => 
//        //		                   Debug.Log(this + " " + t, gameObject)
//        //		                   );
//        fsm.OnUnhandledTrigger( ( s, t ) => Debug.LogError( this + " Can't handle trigger " + t + " in " + s, gameObject ) );

//        fsm.Fire( Trigger.Start );
//    }

//    protected override void StartEntity () {
//        base.StartEntity();

//        moveController = GetComponent<MoveController>();
//        attackController = GetComponent<AttackControllerNew>();
//        //		fsm = GetComponent<PlayMakerFSM>();

//        // subscribe to self death
//        unit.dead.OnChange( this, OnDead );
//    }

//    #region FSM methods
//    // idle methods
//    void StartIdle () {
//        target = null;
//    }

//    void UpdateFindTarget () {
//        Transform newTarget = FindTarget();
//        if ( newTarget != null && newTarget != target ) {
//            target = newTarget;
//            // animate aggr
//            SendMessageRec( "AnimAggr" );

//            fsm.Fire( Trigger.TargetFound );
//        }
//    }

//    // move methods
//    void StartFollowTarget () {
//        moveController.MoveToPointRangeS( target.position, unit.attackRange, OnFollowTargetStop );
//    }
//    void StopFollowTarget () {
//        moveController.Stop();
//    }

//    void OnFollowTargetStop () {
//        //		if (attackController.TargetInRange(target))
//        //			fsm.Fire(Trigger.TargetApproached);
//        //		else {
//        //			target = null;
//        //			fsm.Fire(Trigger.TargetLost);
//        //		}
//    }

//    void UpdateFollowTarget () {
//        if ( target == null || target.Var<bool>( "dead" ) ) {
//            target = null;
//            fsm.Fire( Trigger.TargetLost );

//        } else if ( attackController.TargetInRange( target ) ) {
//            fsm.Fire( Trigger.TargetApproached );

//        } else if ( !moveController.IsMoving() ) {
//            fsm.Fire( Trigger.TargetLost );
//        } else if ( ( target.position - moveController.destination ).sqrMagnitude > pathRefindDistanceSqr ) {
//            fsm.Fire( Trigger.TargetFound );
//        }
//    }

//    // attack
//    void StartAttack () {
//        attackController.SetAttackTarget( target.gameObject );
//    }

//    void StopAttack () {
//        attackController.SetAttackTarget( null );
//        target = null;
//    }

//    void UpdateAttack () {
//        // if target lost
//        if ( !attackController.CanAttackTarget() ) {
//            fsm.Fire( Trigger.TargetLost );
//        }
//    }
//    #endregion

//    void OnDead ( bool dead ) {
//        fsm.Fire( Trigger.Dead );
//    }

//    void OnHit ( Damage damage ) {
//        enemies.Add( damage.attacker );
//        // check if current target is enemy
//        if ( !enemies.Contains( target.gameObject ) ) {
//            target = damage.attacker.transform;
//            fsm.Fire( Trigger.TargetFound );
//        }
//    }

//    void OnDeadEnter () {
//        moveController.Stop();
//        attackController.SetAttackTarget( null );
//    }

//    Transform FindTarget () {
//        Vector3 selfPos = transform.position;
//        var enemy = enemies.MinByNullable( go => go != null ? ( go.transform.position - selfPos ).sqrMagnitude : float.MaxValue );
//        if ( enemy != null )
//            return enemy.transform;

//        return FindNearestEnemy().GetTransform();
//    }
//}
	
}
