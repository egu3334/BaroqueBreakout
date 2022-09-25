using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointData {
    
	private static int checkpoint = -1;
	private static float timeRemaining = -1.0f;

	public static void SaveCheckpoint(int checkpoint, float timeLeft) {
		checkpoint = checkpoint;
		timeRemaining = timeLeft;
	}

	public static int GetCheckpoint() {
		return checkpoint;
	}

	public static float GetSavedTimeRemaining() {
		return timeRemaining;
	}

}
