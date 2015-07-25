using System;
using UnityEngine;
using System.Collections.Generic;

public class FiniteStateMachine {

    public enum States {
        Empty,
        Idle,
        FollowTarget,
        Action,
        Path,
        Dead
    };

    public States CurrentState {
        get;

        private set;
    }

    public enum Events {
        Start,
        TargetFound,
        TargetApproached,
        TargetLost,
        GoToPosition,
        Dead
    };

    public struct StateActionsUpdates {
        public CoroutineManager.CoroutineMethodToCall actions;
        public float timers;
    }

    private CoroutineManager.CoroutineMethodToCall[] entryStateActions;
    private CoroutineManager.CoroutineMethodToCall[] exitStateActions;
    private List<StateActionsUpdates>[] stateActionsUpdatesList;
    private List<int> currentCoroutinesId;

    private States[,] statesPermitArray;

    public FiniteStateMachine () {

        int statesNum = Enum.GetNames( typeof( States ) ).Length;
        int eventsNum = Enum.GetNames( typeof( Events ) ).Length;

        stateActionsUpdatesList = new List<StateActionsUpdates>[statesNum];

        for ( int i = 0; i < stateActionsUpdatesList.Length; ++i ) {
            stateActionsUpdatesList[i] = new List<StateActionsUpdates>();
        }

        exitStateActions = new CoroutineManager.CoroutineMethodToCall[statesNum];
        entryStateActions = new CoroutineManager.CoroutineMethodToCall[statesNum];
        statesPermitArray = new States[statesNum, eventsNum];
        currentCoroutinesId = new List<int>();

    }

    private void StartState ( States newState ) {

        for ( int i = 0; i < currentCoroutinesId.Count; i++ ) {
            SceneManager.Instance.CoroutineManager.StopInvoke( currentCoroutinesId[i] );
        }
        currentCoroutinesId = new List<int>();

        if ( exitStateActions[(int)CurrentState] != null ) {
            exitStateActions[(int)CurrentState]();
        }

        CurrentState = newState;

        if ( entryStateActions[(int)CurrentState] != null ) {
            entryStateActions[(int)CurrentState]();
        }

    }

    public void CallEvent ( Events stateEvent ) {
        if (// statesPermitArray[(int)CurrentState, (int)stateEvent] != CurrentState  && 
            statesPermitArray[(int)CurrentState, (int)stateEvent] != States.Empty) {
            StartState( statesPermitArray[(int)CurrentState, (int)stateEvent] );

            for ( int i = 0; i < stateActionsUpdatesList[(int)CurrentState].Count; ++i ) {
                if ( stateActionsUpdatesList[(int)CurrentState] != null &&
                    stateActionsUpdatesList[(int)CurrentState][i].actions != null ) {
                    currentCoroutinesId.Add( SceneManager.Instance.CoroutineManager.InvokeRepeating(
                        stateActionsUpdatesList[(int)CurrentState][i].actions,
                        0.1f,
                        stateActionsUpdatesList[(int)CurrentState][i].timers
                    ) );
                }
            }
        }
    }

    public void AddStateUpdate ( States state, CoroutineManager.CoroutineMethodToCall stateDelegate, float updateTimer ) {
        StateActionsUpdates tempStateActionsUpdates = new StateActionsUpdates();

        tempStateActionsUpdates.actions = stateDelegate;
        tempStateActionsUpdates.timers = updateTimer;

        stateActionsUpdatesList[(int)state].Add( tempStateActionsUpdates );
    }

    public void SetStateEntery ( States state, CoroutineManager.CoroutineMethodToCall stateDelegate ) {
        entryStateActions[(int)state] = stateDelegate;
    }

    public void SetStateExit ( States state, CoroutineManager.CoroutineMethodToCall stateDelegate ) {
        exitStateActions[(int)state] = stateDelegate;
    }

    public void SetStatePermit ( States currentState, Events stateEvent,  States newState ) {
        statesPermitArray[(int)currentState, (int)stateEvent] = newState;
    }

}
