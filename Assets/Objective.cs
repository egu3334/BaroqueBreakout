using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective {
    
	public string text;
	public WaypointSet[] waypoints;

	private int currentProgress = 0;
	private bool progressRepresentedInUI = false;

	public int maxProgress {
		get {
			if (waypoints == null) {
				return 1;
			}

			return waypoints.Length;
		}
	}

    public void IncrementProgress() {
    	progressRepresentedInUI = false;
    	currentProgress++;
    }

    public bool IsComplete() {
    	return currentProgress >= maxProgress;
    }

    public bool NeedsTextUpdate() {
    	return !progressRepresentedInUI;
    }

    public string GetText() {
    	progressRepresentedInUI = true;
    	if (this.IsComplete()) {
            if (text == "") {
                return "";
            }

    		return text + " [Objective Complete!]";
    	} else if (maxProgress == 1) {
    		return text;
    	} else {
    		return text + " [" + currentProgress + " / " + maxProgress + "]"; 
    	}
    }

    public void ShowWaypoints(Vector3 position) {
    	if (this.IsComplete() || this.waypoints == null) {
    		return;
    	}

    	foreach (WaypointSet w in waypoints) {
    		w.IlluminatePath(position);
    	}
    }

    public void HideWaypoints() {
    	if (this.waypoints == null) {
    		return;
    	}

    	foreach (WaypointSet w in waypoints) {
    		w.EndIllumination();
    	}
    }

}
