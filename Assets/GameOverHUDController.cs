using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHUDController : MonoBehaviour
{
    
    private CanvasGroup canvasGroup;
    private GameStarter gameStarter;
    public Text endText;
    public Button button;

    public GameObject hud;
    private CanvasGroup hudCanvas;

    // Start is called before the first frame update
    void Start() {
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.gameStarter = button.GetComponent<GameStarter>();

        this.hudCanvas = hud.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextLevelActionAndScene(string label, string sceneToLoad) {
        this.SetNextLevelActionAndScene(label, sceneToLoad, true);
    }

    public void SetNextLevelActionAndScene(string label, string sceneToLoad, bool forceCheckpoint) {
        this.button.GetComponentInChildren<Text>().text = label;
        this.gameStarter.SetScene(sceneToLoad);
        this.gameStarter.SetForceCheckpoint(forceCheckpoint);
    }

    public void PresentWithText(string text) {
    	Time.timeScale = 0;
    	endText.text = text;
    	canvasGroup.alpha = 1.0f;
    	canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        this.hudCanvas.alpha = 0.0f;
        Cursor.visible = true;
    }

    public void Hide() {
        Time.timeScale = 1;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        this.hudCanvas.alpha = 1.0f;
        Cursor.visible = false;
    }

}
