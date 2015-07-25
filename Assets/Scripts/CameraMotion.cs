using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour {

    public float cameraHeight;
    public float cameraSpeed;
    private RaycastHit Hit;
    private float indentFromScreenEdgeX = 20;
    private float indentFromScreenEdgeY = 20;
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
        //CameraHeightPosition();
    }

    private void LateUpdate () {
        transform.position = tempCameraPosition;//new Vector3( tempCameraPosition.x, tempCameraPosition2.y, tempCameraPosition.z);
    }

    Vector3 tempCameraPosition = Vector3.zero;

    Vector3 tempCameraPosition2 = Vector3.zero;

    void CameraHeightPosition () {

        Vector3 directionRay = transform.TransformDirection( Vector3.down );

        tempCameraPosition2 = transform.position;

        if ( Physics.Raycast( transform.position, directionRay, out Hit ) ) {

            if ( Hit.distance < currentCameraHight ) {
                tempCameraPosition2 += new Vector3( 0, ( currentCameraHight - Hit.distance), 0 );
            }
            if ( Hit.distance > currentCameraHight ) {
                tempCameraPosition2 -= new Vector3( 0, (Hit.distance - currentCameraHight ), 0 );
            }
        }

        //transform.position = Vector3.Lerp( transform.position, tempCameraPosition2, smooth * Time.deltaTime );
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
