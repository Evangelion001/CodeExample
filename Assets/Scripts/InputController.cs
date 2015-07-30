using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputController :MonoBehaviour {

    public GameObject tagret;

    private EntityController entityController;

    private GameStateController gameStateController;

    public delegate void UpdateWaveTimer ( int value );

    public Text timerValue;

    public Player player;

    public Cursor cursor;

    private bool hadSelected = false;

    void Start () {

        player = new Player();
        entityController = new EntityController( player );
        gameStateController = new GameStateController( entityController, _UpdateWaveTimer );

    }

    private void _UpdateWaveTimer ( int value ) {
        timerValue.text = value.ToString();
    }

    public GameObject[] units;

    void Update () {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            if ( UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() ) {
                Debug.Log( "On button" );
                return;
            }
            SelectClick();
        }
        if ( Input.GetMouseButtonDown( 1 ) ) {
            SetTargetByClick();
        }
        //if ( Input.GetMouseButtonUp( 0 ) ) {
        //    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        //    RaycastHit hit;
        //    if ( Physics.Raycast( ray, out hit ) ) {
        //        endSelectPoint = hit.point;

        //        float x = Vector2.Distance( new Vector2( startSelectPoint.x, 0 ), new Vector2( endSelectPoint.x, 0 ) );
        //        float z = Vector2.Distance( new Vector2( startSelectPoint.z, 0 ), new Vector2( endSelectPoint.z, 0 ) );
        //        boxSelectror.transform.position = new Vector3(startSelectPoint.x + x,
        //            0.5f, startSelectPoint.z + z);
        //        boxSelectror.transform.localScale = new Vector3( x * 0.5f, 1, z * 0.5f);

        //        units = boxSelectror.GetSelectedObjects();
        //        boxSelectror.gameObject.SetActive( true );
        //    }
        //}
    }

    private void SelectClick () {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit ) ) {

            switch ( hit.transform.gameObject.tag ) {
                case "Ground":
                    entityController.UnselectUints();
                    player.HideActionButtons();
                    player.HideUnitIcon();
                    player.HideBuildDescription();
                    break;
                //case "Unit":
                    //entityController.UnselectUints();
                    //player.HideActionButtons();
                    //player.HideUnitIcon();
                    //player.HideBuildDescription();
                    ////FIXME add input array targets;
                    //player.ShowUnitsIcon( hit.transform.gameObject.GetComponent<UnitViewPresenter>().unityType );
                    //player.ShowActionButtons( hit.transform.gameObject.GetComponent<UnitViewPresenter>().unityType );
                    //hit.transform.gameObject.GetComponent<UnitViewPresenter>().Select();
                    //break;
                case "Build":
                    player.HideActionButtons();
                    player.HideUnitIcon();
                    player.HideBuildDescription();
                    player.HideUnitDescription();
                    BuildView temp = hit.transform.gameObject.GetComponent<BuildView>();
                    player.ShowBuildActionButtons( temp );
                    player.ShowBuildDescription( temp.buildLevel, temp.spawnUnitType, temp.baraksUnitConstructor[temp.buildLevel].trainingTime, temp.GetUpgradeCost() );
                    cursor.SetMoveCursor();
                    break;
            }

        }

    }

    private void SetTargetByClick () {

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit ) ) {

            switch ( hit.transform.gameObject.tag ) {
                case "Ground":
                    tagret.transform.position = hit.point;
                    entityController.MoveToPosition( hit.point );
                    cursor.SetMoveCursor();
                    break;
                case "Unit":
                    if ( entityController.isSelected() ) {
                        entityController.SetTarget( hit.transform.gameObject.GetComponent<UnitViewPresenter>() );
                        cursor.SetAttackCursor();
                    }
                    break;
            }

        }
    }

    public void GetSelectableUnits ( UnitViewPresenter[] array) {
        if ( array.Length > 0 ) {
            cursor.SetMoveCursor();
            entityController.UnselectUints();
            player.HideActionButtons();
            player.HideUnitIcon();
            player.HideBuildDescription();
            player.HideUnitDescription();
            foreach ( var key in array ) {
                key.Select();
            }
        } 
    }
}
