using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    
    private bool isPaused;
    private CanvasGroup canvasGroup;

    public GameObject hud;
    private CanvasGroup hudCanvas;

    // Start is called before the first frame update
    void Start() {
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.hudCanvas = hud.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void TogglePause() {
    	if (this.isPaused) {
    		this.UnpauseGame();
    	} else {
    		this.PauseGame();
    	}
    }


    public void PauseGame() {
    	this.isPaused = true;
    	this.canvasGroup.interactable = true;
    	this.canvasGroup.blocksRaycasts = true;
    	this.canvasGroup.alpha = 1.0f;

        this.hudCanvas.alpha = 0.0f;

        Time.timeScale = 0;
        Cursor.visible = true;
    }


    public void UnpauseGame() {
    	this.isPaused = false;
    	this.canvasGroup.interactable = false;
    	this.canvasGroup.blocksRaycasts = false;
    	this.canvasGroup.alpha = 0.0f;

        this.hudCanvas.alpha = 1.0f;

        Time.timeScale = 1;
        Cursor.visible = false;
    }


    public bool IsPaused() {
    	return this.isPaused;
    }

}
