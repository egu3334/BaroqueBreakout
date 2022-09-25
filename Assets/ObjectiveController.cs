using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ObjectiveController : MonoBehaviour
{
    
    public GameObject playerModel;
    public GameObject gameStateObject;
    private GamestateManager gameStateManager;

	public Objective[] objectives;

    public Checkpoint[] checkpoints;

	public float waypointIlluminationLength;
	public float waypointIlluminationCooldown;
    public float waypointIlluminationPenalty;

	int currentObjectiveIndex = 1;

    static int checkpoint = -1;
    static float timeRemaining = -1.0f;
    private bool timeSet = true;

	private float objectiveChangeTime = -1.0f;
	private float hideWaypointTime = -1.0f;
	private float nextWaypointUsage = 0.0f;

	public Text objectiveText;


    // Start is called before the first frame update
    void Start() {
        this.gameStateManager = gameStateObject.GetComponent<GamestateManager>();

        LoadLastCheckpoint();
        foreach (Objective o in objectives) {
        	o.HideWaypoints();
        }
    }

    // Update is called once per frame
    void Update() {
        if (!timeSet) {
            gameStateManager.timeLeft = timeRemaining;
            Checkpoint checkpointData = checkpoints[checkpoint];
            float timeLeft = Mathf.Max(checkpointData.minimumTimeAllowed, timeRemaining);
            timeSet = true;
        }

        if (currentObjectiveIndex >= objectives.Length) {
        	objectiveText.text = "";
        	return;
        }

        Objective objective = objectives[currentObjectiveIndex];
        if (objective.NeedsTextUpdate()) {
        	objectiveText.text = objective.GetText();
        }

        if (objectiveChangeTime > 0.0f) {
        	if (Time.fixedTime >= objectiveChangeTime) {
        		currentObjectiveIndex++;
        		objectiveChangeTime = -1.0f;

                for (int i = 0; i < checkpoints.Length; ++i) {
                    if (currentObjectiveIndex > checkpoints[i].activateAfterObjectiveNo) {
                        checkpoint = i;
                        timeRemaining = gameStateManager.timeLeft;
                    }
                }
        	}
        } else if (objective.IsComplete()) {
        	objectiveChangeTime = Time.fixedTime + 3.0f;
        	objective.HideWaypoints();
        	hideWaypointTime = -1.0f;
        }

        if (hideWaypointTime > 0.0f && Time.fixedTime >= hideWaypointTime) {
        	objective.HideWaypoints();
        	hideWaypointTime = -1.0f;
        }
    }

    public Objective ObjectiveAtIndex(int i) {
    	return objectives[i];
    }

    public void IncrementObjectiveIfActive(int i) {
        if (currentObjectiveIndex == i) {
            FindObjectOfType<AudioManager>().PlaySoundEffect("Objective Complete");
            objectives[i].IncrementProgress();
        }
    }

    public int CurrentObjectiveIndex() {
        return currentObjectiveIndex;
    }

    public Objective GetCurrentObjective() {
    	return objectives[currentObjectiveIndex];
    }

    public void ForciblySetCheckpoint(int i) {
        checkpoint = i;
    }

    public void LoadLastCheckpoint() {
        int cpi = checkpoint;

        if (cpi == -1 || cpi >= checkpoints.Length) {
            return;
        }

        timeSet = false;

        Checkpoint checkpointData = checkpoints[cpi];
        float timeLeft = Mathf.Max(checkpointData.minimumTimeAllowed, timeRemaining);
        gameStateManager.timeLeft = timeLeft;
        this.currentObjectiveIndex = (int) Mathf.Max(currentObjectiveIndex, 
            checkpointData.activateAfterObjectiveNo + 1);
        this.playerModel.transform.position = checkpointData.spawnPoint.transform.position;
    }

    public void OnRequestWaypoints(InputAction.CallbackContext context) {
    	if (currentObjectiveIndex >= objectives.Length || Time.fixedTime < nextWaypointUsage
            || gameStateManager.timeLeft < waypointIlluminationPenalty) {
        	return;
        }

        Objective objective = objectives[currentObjectiveIndex];
        Vector3 position = playerModel.transform.position;
        objective.ShowWaypoints(position);
        hideWaypointTime = Time.fixedTime + waypointIlluminationLength;
        nextWaypointUsage = Time.fixedTime + waypointIlluminationCooldown;

        gameStateManager.timeLeft -= waypointIlluminationPenalty;
    }


}
