using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointSet
{

	public GameObject destination;
	public GameObject waypointsParent;
	public GameObject spotlightOnDestination;

	public void IlluminatePath(Vector3 playerPosition) {
		if (!destination.activeSelf || waypointsParent == null) {
			return;
		}

		foreach (Transform child in waypointsParent.transform) {
			if (Vector3.Distance(playerPosition, destination.transform.position) >
			    Vector3.Distance(child.transform.position, destination.transform.position)) {
				child.gameObject.SetActive(true);
			}
		}

		if (spotlightOnDestination != null) {
			Light lightSource = spotlightOnDestination.GetComponent<Light>();
			lightSource.enabled = true;
		}
	}

	public void EndIllumination() {
		if (spotlightOnDestination != null) {
			Light lightSource = spotlightOnDestination.GetComponent<Light>();
			lightSource.enabled = false;
		}

		if (waypointsParent != null) {
			foreach (Transform child in waypointsParent.transform) {
				child.gameObject.SetActive(false);
			}
		}
	}




}
