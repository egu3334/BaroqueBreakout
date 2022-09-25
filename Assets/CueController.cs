using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueController : MonoBehaviour
{
	public string message = "Add message to be shown";

	private void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.name == "PlayerObj3d")
		{
			int id = GetInstanceID();
			HUDController.SetTip(message);
			HUDController.SetLastCaller(id);
		}
	}

	private void OnTriggerExit(Collider c)
	{
		int id = GetInstanceID();
		if (c.gameObject.name == "PlayerObj3d" && HUDController.WasLastCaller(id))
		{
			HUDController.HideTip();
		}
	}
}
