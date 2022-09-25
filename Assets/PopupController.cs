using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    
    public GameObject ui;
    public float showTime = 5.0f;

    private bool alreadyShown = false;
    private float hideTime = -1.0f;

    private CanvasGroup canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = ui.GetComponent<CanvasGroup>();
    }

    public void OpenPopup()
    {
        if (!alreadyShown) {
            alreadyShown = true;
            canvas.alpha = 1.0f;
            hideTime = Time.fixedTime + showTime;
        }
    }

    public void ClosePopup() {
        canvas.alpha = 0.0f;
    }


    void Update() {
        if (!alreadyShown) {
            return;
        }

        if (hideTime > -1.0f && Time.fixedTime >= hideTime) {
            ClosePopup();
        }
    }


}
