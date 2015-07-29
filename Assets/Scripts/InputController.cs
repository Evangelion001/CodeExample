using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour {

    public GameObject tagret;

    private EntityController entityController;

    private GameStateController gameStateController;

    public delegate void UpdateWaveTimer ( int value ); 

    public Text timerValue;

    public Player player;

    private bool hadSelected = false;

    void Start () {

        player = new Player();
        entityController = new EntityController( player );
        gameStateController = new GameStateController( entityController, _UpdateWaveTimer );

    }

    private void _UpdateWaveTimer (int value) {
        timerValue.text = value.ToString();
    }

    void Update () {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            SelectClick();
        }
        if ( Input.GetMouseButtonDown( 1 ) ) {
            SetTargetByClick();
        }
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
                    break;
                case "Unit":
                    entityController.UnselectUints();
                    player.HideActionButtons();
                    player.HideUnitIcon();
                    //FIXME add input array targets;
                    player.ShowUnitsIcon( hit.transform.gameObject.GetComponent<UnitViewPresenter>().unityType);
                    player.ShowActionButtons( hit.transform.gameObject.GetComponent<UnitViewPresenter>().unityType );
                    hit.transform.gameObject.GetComponent<UnitViewPresenter>().Select();
                    break;
                case "Build":
                    player.HideActionButtons();
                    player.HideUnitIcon();
                    BuildView temp = hit.transform.gameObject.GetComponent<BuildView>();
                    player.ShowBuildDescription( temp.buildLevel, temp.spawnUnitType, temp.baraksUnitConstructor[temp.buildLevel].traingTime, int updateCost )
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
                    break;
                case "Unit":
                    if ( entityController.isSelected() ) {
                        entityController.SetTarget( hit.transform.gameObject.GetComponent<UnitViewPresenter>() );
                    }
                    break;
            }

        }
    }

}
