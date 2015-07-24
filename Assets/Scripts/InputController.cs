using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public GameObject tagret;

    public GameObject testUnit;
    public GameObject testUnit2;

    private CenterUIView cuiv;

    void Start () {
        cuiv = new CenterUIView();
        cuiv.centerUIViewPresenter = FindObjectOfType<CenterUIViewPresenter>();
        //entityController.CreateUnit( testUnit, EntityController.Faction.Blue );
        //entityController.CreateUnit( testUnit2, EntityController.Faction.Red );
    }

    void Update () {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            MoveAtClickedPosition();
        }
    }

    void LateUpdate () {

    }

    public EntityController entityController;

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
