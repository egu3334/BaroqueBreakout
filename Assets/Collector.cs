using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{

    public GameObject gameOverHUD;
    // private GameOverHUDController hudController;
    [HideInInspector]
    public bool win = false;

    // Start is called before the first frame update
    void Start() {
        // this.hudController = gameOverHUD.GetComponent<GameOverHUDController>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Collect() {
        /* this.hudController.SetNextLevelActionAndScene("Next Level",
                    "Chapter 1");
        this.hudController.PresentWithText("You won! :)");
        win = true; */
    }

}
