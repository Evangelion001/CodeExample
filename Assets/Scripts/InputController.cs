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
            MoveAtClickedPosition();
        }
        if ( Input.GetMouseButtonDown( 1 ) ) {
            SetTargetByClick();
        }
    }

    private void MoveAtClickedPosition () {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit ) ) {

            switch ( hit.transform.gameObject.tag ) {
                case "Ground":
                    tagret.transform.position = hit.point;
                    entityController.MoveToPosition( hit.point );
                    break;
                case "Unit":
                    entityController.UnselectUints();
                    hit.transform.gameObject.GetComponent<UnitViewPresenter>().Select();
                    break;
            }

        }

    }

    private void SetTargetByClick () {

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit ) ) {

            switch ( hit.transform.gameObject.tag ) {
                case "Unit":
                    if ( entityController.isSelected() ) {
                        entityController.SetTarget( hit.transform.gameObject.GetComponent<UnitViewPresenter>() );
                    }
                    break;
            }

        }
    }

}
