using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public GameObject tagret;

    private CenterUIView cuiv;

    private Spawn spawn;

    private BuildView[] buildView;

    void Start () {
        cuiv = new CenterUIView();
        cuiv.centerUIViewPresenter = FindObjectOfType<CenterUIViewPresenter>();
        spawn = new Spawn( entityController );
        buildView = FindObjectsOfType<BuildView>();
        spawn.CreateUnitByType( buildView[0].spawnUnitType, buildView[0].spawnPosition.transform.position, buildView[0].GetUnit() );
        spawn.CreateUnitByType( buildView[1].spawnUnitType, buildView[1].spawnPosition.transform.position, buildView[1].GetUnit() );
    }

    void Update () {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            MoveAtClickedPosition();
        }
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
