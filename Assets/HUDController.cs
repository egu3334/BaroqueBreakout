using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
	public GameObject alert;
	public GameObject tip;
	public GameObject security;

	private static TMP_Text alertText;
	private static TMP_Text tipText;
	private static TMP_Text securityText;

	private static int lastCaller;

    // Start is called before the first frame update
    void Start()
    {
        if (alert)
        {
        	alertText = alert.GetComponent<TMP_Text>();
        }
        if (tip)
        {
        	tipText = tip.GetComponent<TMP_Text>();
        }
        if (security)
        {
        	securityText = security.GetComponent<TMP_Text>();
        }
    }

    public static bool WasLastCaller(int compareId)
    {
    	return compareId == lastCaller;
    }

    public static void SetLastCaller(int id)
    {
    	lastCaller = id;
    }

    public static void SetAlert(string msg)
    {
    	if (alertText)
    	{
    		alertText.SetText(msg);
    	}
    }
    public static void SetTip(string msg)
    {
    	if (tipText)
    	{
    		tipText.SetText(msg);
    	}
    }
    public static void SetSecurity(string msg)
    {
    	if (securityText)
    	{
    		securityText.SetText(msg);
    	}
    }

    public static void HideAlert()
    {
    	SetAlert("");
    }

    public static void HideTip()
    {
    	SetTip("");
    }

    public static void HideSecurity()
    {
    	SetSecurity("");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
