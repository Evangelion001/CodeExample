using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InputController :MonoBehaviour {

    private EntityController entityController;

    private GameStateController gameStateController;

    public delegate void UpdateWaveTimer ( int value );

    public Text timerValue;

    public Player player;

    public Cursor cursor;

    public UnitViewPresenter redMainTarget;

    public GameObject blueMainPosition;

    public BuildViewPresenter buildViewPresenter;

    public enum CursorsType {
        Simple,
        TargetSpell,
        PositionSpell
    }

    private CursorsType cursorsType;

    public SelectionBox selelectionBox;

    private Action<UnitViewPresenter> currentTargetSpell;

    private Action<Vector3> currentPositionSpell;

    private void SetCurrentTargetSpell ( Action<UnitViewPresenter> currentTargetSpell ) {
        this.currentTargetSpell = currentTargetSpell;
    }

    private void SetCurrentPositionSpell ( Action<Vector3> currentPositionSpell ) {
        this.currentPositionSpell = currentPositionSpell;
    }

    public void SetCursor ( CursorsType type) {

        cursorsType = type;

        switch ( cursorsType ) {
            case CursorsType.Simple:
                cursor.SetMoveCursor();
                break;
            default:
                cursor.SetMagicCursor();
                break;
        }

    }

    void Start () {
        player = new Player( SetCurrentTargetSpell, SetCurrentPositionSpell, SetCursor );
        entityController = new EntityController( player, redMainTarget, blueMainPosition.gameObject.transform.position, buildViewPresenter );
        gameStateController = new GameStateController( entityController, _UpdateWaveTimer );

    }

    private void _UpdateWaveTimer ( int value ) {
        timerValue.text = value.ToString();
    }

    public GameObject[] units;

    void Update () {

        GetSelectableUnits(selelectionBox.SelectionBoxArea());

        if ( Input.GetMouseButtonDown( 0 ) ) {
            if ( !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() ) {
                return;
            }
            SelectClick();
        }
        if ( Input.GetMouseButtonDown( 1 ) ) {
            cursorsType = CursorsType.Simple;
            SetTargetByClick();
        }

    }

    private void SelectClick () {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit[] hit;

        if ( (hit = Physics.RaycastAll( ray, Mathf.Infinity )).Length > 0 ) {

            foreach ( var key in hit ) {
                switch ( key.transform.gameObject.tag ) {
                    case "Build":
                        if ( cursorsType == CursorsType.Simple ) {
                            Deselect();
                            BaraksModel temp = key.transform.gameObject.GetComponent<BaraksModel>();
                            player.ShowBuildActionButtons( temp );
                            player.ShowBuildDescription( temp.buildLevel, temp.spawnUnitType, temp.baraksUnitConstructor[temp.buildLevel].trainingTime, temp.GetUpgradeCost() );
                            cursor.SetMoveCursor();
                        }
                        return;
                    case "Unit":
                        if ( cursorsType == CursorsType.TargetSpell ) {
                            currentTargetSpell( key.transform.gameObject.GetComponent<UnitViewPresenter>() );
                        } else if( cursorsType == CursorsType.Simple ) {
                            Deselect();
                            UnitViewPresenter[] temp = new UnitViewPresenter[1];
                            temp[0] = key.transform.gameObject.GetComponent<UnitViewPresenter>();
                            GetSelectableUnits( temp );
                            cursor.SetMoveCursor();
                        }
                        return;
                    case "Ground":
                        if ( cursorsType == CursorsType.PositionSpell ) {
                            currentPositionSpell( key.point );
                        } else {
                            Deselect();
                        }
                        return;
                }
            }
        }

    }

    private void SetTargetByClick () {

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit[] hit;

        if ( ( hit = Physics.RaycastAll( ray, Mathf.Infinity ) ).Length > 0 ) {

            foreach ( var key in hit ) {

                switch ( key.transform.gameObject.tag ) {
                    case "Ground":
                        entityController.MoveToPosition( key.point );
                        cursor.SetMoveCursor();
                        return;
                    case "Unit":
                        if ( entityController.isSelected() ) {
                            entityController.SetTarget( key.transform.gameObject.GetComponent<UnitViewPresenter>() );
                            cursor.SetAttackCursor();
                        }
                        return;
                }
            }
            
        }
    }

    public void GetSelectableUnits ( UnitViewPresenter[] array) {
        if ( array.Length > 0 ) {
            Deselect();
            cursor.SetMoveCursor();

            foreach ( var key in array ) {
                key.GetComponent<UnitViewPresenter>().Select();
            }
        } 
    }

    private void Deselect () {
        entityController.UnselectUints();
        player.HideActionButtons();
        player.HideUnitIcon();
        player.HideBuildDescription();
        player.HideUnitDescription();
        cursorsType = CursorsType.Simple;
    }
}
