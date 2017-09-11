using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class WhiteFade : MonoBehaviour {

    public GameObject whiteScreen;
    public GameObject credits;
    public bool whiteorblack = false;

    private bool upFade;
    private bool downFade;

    private float fadeRate = 0.05f;
    public float customFade;

	// Use this for initialization
	void Awake () {
        whiteScreen = GameObject.Find("Image");
        whiteScreen.transform.position = new Vector3(9999, 9999, 9999);
        whiteScreen.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        fadeRate = 0.05f;
        credits = GameObject.Find("Credits");
        if(customFade != 0)
        {
            fadeRate = customFade;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            endGame();
        }

        if (upFade)
        {
            if (whiteScreen.GetComponent<Image>().color.a < 1.0f)
            {
                if (whiteorblack == true)
                {
                    whiteScreen.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, whiteScreen.GetComponent<Image>().color.a + fadeRate);
                    credits.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, whiteScreen.GetComponent<Image>().color.a - fadeRate);
                }
                else
                {
                    whiteScreen.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, whiteScreen.GetComponent<Image>().color.a + fadeRate);
                    credits.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, whiteScreen.GetComponent<Image>().color.a - fadeRate);
                }
            }
            else
            {
                //downFade = true;
                upFade = false;
            }
        }
        else if (downFade)
        {
            if (whiteScreen.GetComponent<Image>().color.a > 0.0f)
            {
                if (whiteorblack == true)
                {
                    whiteScreen.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, whiteScreen.GetComponent<Image>().color.a - fadeRate);
                    credits.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, whiteScreen.GetComponent<Image>().color.a + fadeRate);
                }
                else
                {
                    whiteScreen.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, whiteScreen.GetComponent<Image>().color.a - fadeRate);
                    credits.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, whiteScreen.GetComponent<Image>().color.a + fadeRate);
                }
            }
            else
            {
                whiteScreen.transform.position = new Vector3(9999,9999,9999);
            }
        }
	}

    public void triggerFade()
    {
        upFade = true;
        whiteScreen.transform.localScale = new Vector3(10, 10, 0);
        whiteScreen.transform.position = new Vector3(640, 360, 0);
    }

    public void triggerFade(float rate)
    {
        upFade = true;
        fadeRate = rate;
        whiteScreen.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    public void endGame()
    {
        triggerFade();
        credits.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    }
}
