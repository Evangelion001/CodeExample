using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public NavMeshAgent navMeshAgent;

    public GameObject tagret;

    private Vector3 cameraStartDistance;

    private CenterUIView cuiv;

    void Start () {
        cameraStartDistance =  new Vector3(0,10,-15);
        cuiv = new CenterUIView();
        cuiv.centerUIViewPresenter = FindObjectOfType<CenterUIViewPresenter>();
    }

    void Update () {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            MoveAtClickedPosition();
        }
    }

    void LateUpdate () {
        Camera.main.transform.position = navMeshAgent.transform.position + cameraStartDistance;
    }

    private void MoveAtClickedPosition () {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit )) {

            switch ( hit.transform.gameObject.tag ) {
                case "Ground":
                    navMeshAgent.SetDestination( hit.point );
                    tagret.transform.position = hit.point;
                    break;
                case "Unit":
                    Debug.Log( "Unit" );
                    cuiv.AddHeroIcon();
                    break;
            }

        }

    }

}
