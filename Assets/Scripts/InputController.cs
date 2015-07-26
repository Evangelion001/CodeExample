using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public GameObject tagret;

    private CenterUIView cuiv;

    private Spawn spawn;

    private BuildView[] buildView;

    private EntityController entityController;

    void Start () {
        cuiv = new CenterUIView();
        cuiv.centerUIViewPresenter = FindObjectOfType<CenterUIViewPresenter>();

        entityController = new EntityController();
        spawn = new Spawn( entityController );

        buildView = FindObjectsOfType<BuildView>();

        foreach ( var key in buildView ) {
            spawn.CreateUnitByType( key.spawnUnitType, key.spawnPosition.transform.position, key.GetUnit(), key.faction );
        }

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
