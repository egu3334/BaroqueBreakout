using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private CanvasGroup transparency;
	void Start()
	{
		transparency = GetComponent<CanvasGroup>();
    	transparency.alpha = 0.5f;
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
    	transparency.alpha = 1;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
    	transparency.alpha = 0.5f;
    }
}
