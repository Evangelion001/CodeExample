using UnityEngine;
using System.Collections;

public class BuildBehaviour : BaseUnitBehaviour {

    public BuildBehaviour ( EntityController.GetTarget getTarget, EntityController.Faction faction, UnitViewPresenter myViewPresenter, AnimationController animationController ):base( 
        getTarget, faction, myViewPresenter, animationController ) {

        InitStateMachine();
    }

    public override void InitStateMachine () {
        fsm = new FiniteStateMachine();

        fsm.SetStatePermit( FiniteStateMachine.States.Empty, FiniteStateMachine.Events.Start, FiniteStateMachine.States.Idle );

        fsm.SetStateEntery( FiniteStateMachine.States.Idle, StartIdle );
        fsm.SetStatePermit( FiniteStateMachine.States.Idle, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );

        fsm.SetStateEntery( FiniteStateMachine.States.Dead, OnDeadEnter );
        fsm.SetStatePermit( FiniteStateMachine.States.Dead, FiniteStateMachine.Events.Dead, FiniteStateMachine.States.Dead );

        fsm.CallEvent( FiniteStateMachine.Events.Start );
    }

    public override void OnDeadEnter () {
        Debug.Log( "Build Death" );
    }

}
