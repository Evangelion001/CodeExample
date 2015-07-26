using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour {

    public GameObject tagret;

    private CenterUIView cuiv;

    private EntityController entityController;

    private GameStateController gameStateController;

    public delegate void UpdateWaveTimer ( int value ); 

    public Text timerValue;

    void Start () {
        cuiv = new CenterUIView();
        cuiv.centerUIViewPresenter = FindObjectOfType<CenterUIViewPresenter>();

        entityController = new EntityController();
        gameStateController = new GameStateController( entityController, _UpdateWaveTimer );

    }

    private void _UpdateWaveTimer (int value) {
        timerValue.text = value.ToString();
    }

    void Update () {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            MoveAtClickedPosition();
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

                    //FIXME move to anather place for cuiv methods;
                    cuiv.RemoveIcons();
                    cuiv.AddHeroIcon();
                    break;
            }

        }

    }

}
