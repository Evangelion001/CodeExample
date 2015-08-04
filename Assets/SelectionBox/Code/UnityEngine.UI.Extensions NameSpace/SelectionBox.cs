using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
	
	[RequireComponent(typeof(Canvas))]
	public class SelectionBox : MonoBehaviour
	{
		public Color color;
		public Sprite art;
		private Vector2 fistClick;
		public RectTransform selectionMask;
		private RectTransform boxRect;
		private GameObject[] selectables;
		private GameObject clickedBeforeDrag;
		private GameObject clickedAfterDrag;
        private List<GameObject> selectedList = new List<GameObject>();


    void ValidateCanvas(){
		var canvas = gameObject.GetComponent<Canvas>();
			
		if (canvas.renderMode != RenderMode.ScreenSpaceOverlay) {
			throw new System.Exception("SelectionBox component must be placed on a canvas in Screen Space Overlay mode.");
		}
			
		var canvasScaler = gameObject.GetComponent<CanvasScaler>();
			
		if (canvasScaler && canvasScaler.enabled && (!Mathf.Approximately(canvasScaler.scaleFactor, 1f) || canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize)) {
			Destroy(canvasScaler);
			Debug.LogWarning("SelectionBox component is on a gameObject with a Canvas Scaler component. As of now, Canvas Scalers without the default settings throw off the coordinates of the selection box. Canvas Scaler has been removed.");
		}
	}
		
	void CreateBoxRect(){
		var selectionBoxGO = new GameObject();
			
		selectionBoxGO.name = "Selection Box";
		selectionBoxGO.transform.parent = transform;
		selectionBoxGO.AddComponent<Image>();
			
		boxRect = selectionBoxGO.transform as RectTransform;
			
	}
		

	void ResetBoxRect(){
			
		Image image = boxRect.GetComponent<Image>();
		image.color = color;
		image.sprite = art;
			
		fistClick = Vector2.zero;
			
		boxRect.anchoredPosition = Vector2.zero;
		boxRect.sizeDelta = Vector2.zero;
		boxRect.anchorMax = Vector2.zero;
		boxRect.anchorMin = Vector2.zero;
		boxRect.pivot = Vector2.zero;
		boxRect.gameObject.SetActive(false);
	}
		
		
    void BeginSelection(){

        if (!Input.GetMouseButtonDown(0))
			return;

        boxRect.gameObject.SetActive(true);

		fistClick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			
		if (!PointIsValidAgainstSelectionMask(fistClick)) {
			ResetBoxRect();
			return;
		}
			
		boxRect.anchoredPosition = fistClick;

        UnitViewPresenter[] behavioursToGetSelectionsFrom;

        behavioursToGetSelectionsFrom = GameObject.FindObjectsOfType<UnitViewPresenter>();

        List<GameObject> selectableList = new List<GameObject>();

        foreach ( var key in behavioursToGetSelectionsFrom ) {
            if ( key.GetComponent<BuildViewPresenter>() == null ) {
                selectableList.Add( key.gameObject );
            }
        }

        selectables = selectableList.ToArray();


        clickedBeforeDrag = GetSelectableAtMousePosition();
			
	}
		
	bool PointIsValidAgainstSelectionMask(Vector2 screenPoint){
		if (!selectionMask) {
			return true;
		}
			
		Camera screenPointCamera = GetScreenPointCamera(selectionMask);
			
		return RectTransformUtility.RectangleContainsScreenPoint(selectionMask, screenPoint, screenPointCamera);
	}
		
	GameObject GetSelectableAtMousePosition() {
		
		if (!PointIsValidAgainstSelectionMask(Input.mousePosition)) {
			return null;
		}

		foreach (var selectable in selectables) {
            if ( selectable != null ) {

                var radius = selectable.transform.GetComponent<CapsuleCollider>().bounds.extents.magnitude;
                var selectableScreenPoint = GetScreenPointOfSelectable( selectable );

                if ( Vector2.Distance( selectableScreenPoint, Input.mousePosition ) <= radius*5 ) {
                    return selectable;
                }
            }
        }

		return null;
	}

    void DragSelection() {

		if (!Input.GetMouseButton(0) || !boxRect.gameObject.activeSelf)
			return;

        Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		Vector2 difference = currentMousePosition - fistClick;
			
		Vector2 startPoint = fistClick;

		if (difference.x < 0)
		{
			startPoint.x = currentMousePosition.x;
			difference.x = -difference.x;
		}
		if (difference.y < 0)
		{
			startPoint.y = currentMousePosition.y;
			difference.y = -difference.y;
		}
			
		boxRect.anchoredPosition = startPoint;
		boxRect.sizeDelta = difference;

        List<GameObject> tempSelecteble = new List<GameObject>();

        foreach ( var selectable in selectables ) {
            if ( selectable != null ) {
                Vector3 screenPoint = GetScreenPointOfSelectable( selectable );

                if ( RectTransformUtility.RectangleContainsScreenPoint( boxRect, screenPoint, null ) && PointIsValidAgainstSelectionMask( screenPoint ) ) {
                    tempSelecteble.Add( selectable );
                }
            }
                
        }

        selectedList = tempSelecteble;
    }
		
		
    Vector2 GetScreenPointOfSelectable(GameObject selectable) {

        if ( selectable.transform == null ) {
            return Vector2.zero;
        }

		return Camera.main.WorldToScreenPoint(selectable.transform.position);				                                         
			
	}

	Camera GetScreenPointCamera(RectTransform rectTransform) {
			
		Canvas rootCanvas = null;
		RectTransform rectCheck = rectTransform;
			
		do {
			rootCanvas = rectCheck.GetComponent<Canvas>();
				
			if (rootCanvas && !rootCanvas.isRootCanvas) {
				rootCanvas = null;
			}

			rectCheck = (RectTransform)rectCheck.parent;
				
		} while (rootCanvas == null);
			
			
		switch (rootCanvas.renderMode) {
		    case RenderMode.ScreenSpaceOverlay:
			    return null;				
		    case RenderMode.ScreenSpaceCamera:
			    return (rootCanvas.worldCamera) ? rootCanvas.worldCamera : Camera.main;
	    	default:
		      case RenderMode.WorldSpace:
			    return Camera.main;
		}
			
	}
		
    public UnitViewPresenter[] GetAllSelected(){

		if ( selectedList == null) {
			return new UnitViewPresenter[0];
		}

        List<UnitViewPresenter> tempList = new List<UnitViewPresenter>();

        foreach (var selectable in selectedList) {
			if (selectable) {
                tempList.Add( selectable.transform.GetComponent<UnitViewPresenter>() );
            }
		}

        //if ( clickedAfterDrag != null  ) {
        //    tempList.Add( clickedAfterDrag.transform.GetComponent<UnitViewPresenter>() );
        //}

        //if ( clickedBeforeDrag != null ) {
        //    tempList.Add( clickedBeforeDrag.GetComponent<UnitViewPresenter>() );
        //}

        bool duble = false;
        int idx = 0;

        for ( int i =0; i< tempList.Count; ++i ) {
            for ( int j = 0; j < tempList.Count; ++j ) {
                if ( tempList[i] == tempList[j] && i != j) {
                    duble = true;
                    idx = j;
                }
            }
            if ( duble ) {
                duble = false;
                tempList.Remove( tempList[idx] );
            }
        }

        return tempList.ToArray();

    }
		
    UnitViewPresenter[] EndSelection() {
          
        if ( !Input.GetMouseButtonUp( 0 ) || !boxRect.gameObject.activeSelf ) {
            return new UnitViewPresenter[0];
        }

        clickedAfterDrag = GetSelectableAtMousePosition();

		ResetBoxRect();

        return GetAllSelected();

    }
		
	void Start(){
		ValidateCanvas();
		CreateBoxRect();
		ResetBoxRect();
	}
		
	public UnitViewPresenter[] SelectionBoxArea () {
		BeginSelection ();
		DragSelection ();
		return EndSelection ();
	}
}

