using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour {

    public float cameraHeight;
    public float cameraSpeed;
    private RaycastHit Hit;
    private float indentFromScreenEdgeX = 20;
    private float indentFromScreenEdgeY = 20;
    private float smooth = 2f;
    public GameObject target;
    private float currentCameraHight;

    private void Start () {
        QualitySettings.vSyncCount = 1;
        indentFromScreenEdgeX = Screen.width * 0.05f;
        indentFromScreenEdgeY = Screen.height * 0.05f;
        tempCameraPosition = transform.position;
        tempCameraPosition2 = transform.position;
    }

    private void Update () {
        if ( target == null ) {
            CameraWidthPosition();
        } else {
            MoveToTarget();
        }
        CameraHeightPosition();
    }

    private void LateUpdate () {
        transform.position = tempCameraPosition;
    }

    Vector3 tempCameraPosition = Vector3.zero;

    Vector3 tempCameraPosition2 = Vector3.zero;

    void CameraHeightPosition () {

        Vector3 DirectionRay = transform.TransformDirection( Vector3.down );

        if ( Physics.Raycast( transform.position, DirectionRay, out Hit ) ) {

            if ( Hit.distance < cameraHeight ) {
                tempCameraPosition2 = transform.position + new Vector3( 0, ( cameraHeight - Hit.distance), 0 );
            }
            if ( Hit.distance > cameraHeight ) {
                tempCameraPosition2 = transform.position - new Vector3( 0, (Hit.distance - cameraHeight ), 0 );
            }
        }
        transform.position = Vector3.Lerp( transform.position, tempCameraPosition2, smooth * Time.deltaTime );
    }

    private void CameraWidthPosition () {

        tempCameraPosition = transform.position;

        if ( indentFromScreenEdgeX > Input.mousePosition.x ) {
            tempCameraPosition -= new Vector3( cameraSpeed * Time.deltaTime, 0, 0 );
        }

        if ( ( Screen.width - indentFromScreenEdgeX ) < Input.mousePosition.x ) {
            tempCameraPosition += new Vector3( cameraSpeed * Time.deltaTime, 0, 0 );
        }

        if ( indentFromScreenEdgeY > Input.mousePosition.y ) {
            tempCameraPosition -= new Vector3( 0, 0, cameraSpeed * Time.deltaTime );
        }
        if ( ( Screen.height - indentFromScreenEdgeY ) < Input.mousePosition.y ) {
            tempCameraPosition += new Vector3( 0, 0, cameraSpeed * Time.deltaTime );
        }

        //transform.position = tempCameraPosition;
    }

    public void SetTarget () {
        currentCameraHight = 7;
    }

    public void FreeView () {
        currentCameraHight = cameraHeight;
        target = null;
    }

    private void MoveToTarget () {
        Vector3 destination = new Vector3( target.transform.position.x,transform.position.y, target.transform.position.z -10 );
        transform.position = Vector3.Slerp( transform.position, destination, cameraSpeed * Time.deltaTime );
    }

}
