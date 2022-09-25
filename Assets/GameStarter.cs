using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{

    public string sceneName;

    public bool forceCheckpoint;
    public GameObject objectiveManager;

    public int checkpointId;    

    public void SetForceCheckpoint(bool force) {
    	this.forceCheckpoint = force;
    }

    public void SetScene(string scene) {
    	this.sceneName = scene;
    }

    public void StartGame() {
    	if (forceCheckpoint) {
    		objectiveManager.GetComponent<ObjectiveController>().ForciblySetCheckpoint(checkpointId);
    	}

    	SceneManager.LoadScene(sceneName);
    	Time.timeScale = 1;
    }

}
