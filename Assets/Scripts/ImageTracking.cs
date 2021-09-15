using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
	[SerializeField]
	private GameObject[] placeablePrefabs;
		
	private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
	private ARTrackedImageManager trackedImageManager;
	
	// Awake is called when the script instance is being loaded.
    private void Awake() {
	    trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
	    
	    foreach(GameObject prefab in placeablePrefabs) {
	    	GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
	    	newPrefab.name = prefab.name;
	    	spawnedPrefabs.Add(prefab.name, newPrefab);
	    }
    }
    
	// This function is called when the object becomes enabled and active.
	private void OnEnable()
	{
		trackedImageManager.trackedImagesChanged += ImagedChanged;
	}
	
	
	// This function is called when the behaviour becomes disabled () or inactive.
	private void OnDisable()
	{
		trackedImageManager.trackedImagesChanged -= ImagedChanged;
	}

	private void ImagedChanged(ARTrackedImagesChangedEventArgs eventArgs) {
		foreach(ARTrackedImage trackedImage in eventArgs.added) {
			UpdateImage(trackedImage);
		}
		foreach(ARTrackedImage trackedImage in eventArgs.updated) {
			UpdateImage(trackedImage);
		}
		//foreach(ARTrackedImage trackedImage in eventArgs.removed) {
		//	spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
		//}
	}

	private void UpdateImage(ARTrackedImage trackedImage) {
		string name = trackedImage.referenceImage.name;
		Vector3 position = trackedImage.transform.position;
		
		GameObject prefab =  spawnedPrefabs[name];
		prefab.transform.position = position; 

		prefab.SetActive(true);
		
		//// Desactivando los otros modelos 
		foreach(GameObject go in spawnedPrefabs.Values) {
			if (go.name != name) {
				go.SetActive(false);
			}
		}

	}
}
