using UnityEngine;
using System.Collections;

public class MouseHandler : MonoBehaviour {
	
	private GameObject gridManager;
	private GameObject selHighlight;
	
	// Use this for initialization
	void Start () {
		gridManager = GameObject.Find("GridManager");
		selHighlight = GameObject.Find("Select");
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if(gridManager.GetComponent<GridManager>().selectEnable) { 
				if (Input.GetMouseButtonDown (0)) {
					if(hit.collider.gameObject.GetComponent<Gem>() != null) {
						if(gridManager.GetComponent<GridManager>().SelGem1 == null) {
							gridManager.GetComponent<GridManager>().SelGem1 = hit.collider.gameObject.transform;
							iTween.ScaleTo (hit.collider.gameObject, iTween.Hash ("scale", new Vector3 (1.4f, 1.4f, 1.4f), "time", 0.5f, "delay", 0f, "easetype", iTween.EaseType.easeInOutBack));
						} else {
							gridManager.GetComponent<GridManager>().selectEnable = false;
							
							gridManager.GetComponent<GridManager>().SelGem2 = hit.collider.gameObject.transform;
							
							Transform selGem1 = gridManager.GetComponent<GridManager>().SelGem1;
							Transform selGem2 = gridManager.GetComponent<GridManager>().SelGem2;
							
							iTween.ScaleTo (selGem2.gameObject, iTween.Hash ("scale", new Vector3 (1.4f, 1.4f, 1.4f), "time", 0.5f, "delay", 0f, "easetype", iTween.EaseType.easeInOutBack));
							iTween.MoveTo (selGem2.gameObject, iTween.Hash ("position", selGem1.position, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
							iTween.ScaleTo (selGem2.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.5f, "delay", 1f, "easetype", iTween.EaseType.easeInOutBack));
							
							iTween.MoveTo (selGem1.gameObject, iTween.Hash ("position", selGem2.position, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
							iTween.ScaleTo (selGem1.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.5f, "delay", 1f, "easetype", iTween.EaseType.easeInOutBack, "oncomplete", "SwapGems", "oncompletetarget", gridManager));
						}
					}
				} else {
					if(hit.collider.gameObject.GetComponent<Gem>() != null) {
						hit.collider.gameObject.transform.Rotate(new Vector3(0, 1.0f, 0));
						
						selHighlight.transform.position = hit.collider.transform.position;
						selHighlight.GetComponent<Renderer>().enabled = true;
					}
				}
			}
		} else {
			selHighlight.GetComponent<Renderer>().enabled = false;
		}
		
		//TESTE
		/*if (Input.GetMouseButtonDown (0)) {
			gridManager.GetComponent<GridManager>().MoveGem(0,9);
		}*/
	}
}
