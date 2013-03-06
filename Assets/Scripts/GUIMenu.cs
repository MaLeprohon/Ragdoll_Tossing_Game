using UnityEngine;
using System.Collections;

public class GUIMenu : MonoBehaviour
{

    public enum Variable
    {
        Gravity,
        Velocity,
        Angle,
        //X,
        Y
    }

    public Variable inputVariable;
    public Variable inputVariable2;
    public float minValue = -15.0f, maxValue = 15.0f; // Min and Max values for the input variable
    public bool hasWon;
    private int maxLevel = 5;
    public int totalScore;

    private float hSliderValue;
    private float hSliderValue2;
    private bool canShoot;
    private string menuType;
    private Cannon cannonScript;
    private bool musicOn;
    public Texture2D texturePointLabel;

    public GUIStyle myStyle;
    public GUIStyle myStyle2;
    public GUIStyle myStylePlusMinus;
    public GUIStyle myStyleText;
    public GUIStyle myStyleText2;
    public GUIStyle myStylelvl4;
    public GUIStyle myStyleSolution;
    public GUIStyle myStyleHelp;
    public GUIStyle myStyleHelpPanel;
    public GUIStyle myStyleHelpPanelClose;

    private float fontFactor = 0.015f;

    public bool helpPanel = false;
    public bool tutClose = false;
    public bool blinking = false;

    public int pressedPlus, pressedMinus, releasedPlus, releasedMinus;
    
    public int nTries = 0; // Number of shots
    public int fetchedPoints;
    public int triesPoints;

    public Texture shoot, pause, restart;
    public GUIStyle customButton;
    public GUIStyle customButton2;

    void Start()
    {
        Application.CaptureScreenshot("Screenshot.png");

        //finding and initializing a cannon
        cannonScript = GameObject.FindGameObjectWithTag("cannon").GetComponent<Cannon>();

        menuType = "inGame";
        canShoot = true;
        hasWon = false;
        switch (inputVariable)
        {
            case Variable.Gravity:
                minValue = cannonScript.minGravity;
                maxValue = cannonScript.maxGravity;
                break;
            case Variable.Velocity:
                minValue = cannonScript.minVelocity;
                maxValue = cannonScript.maxVelocity;
                break;
            case Variable.Y:
                minValue = cannonScript.minY;
                maxValue = cannonScript.maxY;
                break;
        }
        if (PlayerPrefs.HasKey(inputVariable.ToString()))
        {
            hSliderValue = PlayerPrefs.GetFloat(inputVariable.ToString());
        }
        else
        {
            hSliderValue = (minValue + maxValue) / 2f;
        }

        //music
        musicOn = false;
        if (PlayerPrefs.GetInt("musicOn") == 1)
        {
            musicOn = true;
        }
        pressedPlus = pressedMinus = releasedPlus = releasedMinus = 0;
    }

    void OnGUI()
    {
        GUI.skin.label.normal.textColor = Color.black;
        switch (menuType)
        {
            case "inGame":


                //help button to open tutorial
                Rect rectHelp = new Rect(Screen.width * 0.01f, Screen.height * .45f, Screen.width * 0.1f, Screen.width * 0.1f);
                GUILayout.BeginArea(rectHelp);

                myStyleHelp.fontSize = Mathf.RoundToInt(Screen.width * fontFactor);
                if (GUILayout.Button("Click here to get help", myStyleHelp, GUILayout.Height(rectHelp.width), GUILayout.Width(rectHelp.height)))
                {
                    helpPanel = true;
                    tutClose = true;
                }

                GUILayout.EndArea();

                //solution/answer popup
                Rect rectSolution = new Rect(Screen.width * 0.01f, Screen.height * .75f, Screen.width * 0.18f, Screen.height * 0.25f);
                myStyleSolution.fontSize = Mathf.RoundToInt(Screen.width * 0.01f);
                if (Application.loadedLevel != 5)
                {
                    switch (nTries)
                    {
                        //after 3 tries
                        case 3:
                            GUI.Button(rectSolution, "Mouseover here for the right formula", myStyleSolution);

                            if (rectSolution.Contains(Event.current.mousePosition))
                            {
                                GUI.Label(rectSolution, "Try deriving the answer from this formula:\n" + "y = tan(A)*x-g*x^2/(2*v^2*cos(A)^2)", myStyleSolution);
                            }
                            break;

                        //after 4 tries
                        case 4:
                            GUI.Label(rectSolution, "Mouseover to get the answer", myStyleSolution);

                            if (rectSolution.Contains(Event.current.mousePosition))
                            {
                                GUI.Label(rectSolution, (GameObject.FindGameObjectWithTag("cannon").GetComponent<Cannon>().answer).ToString(), myStyleSolution);
                            }

                            break;

                    }
                }

                float areaY = Screen.height * .05f;
                float btnSize = Screen.width * 0.06f;
                myStylePlusMinus.fontSize = Mathf.RoundToInt(Screen.width * 0.08f);
                myStyleText.fontSize = Mathf.RoundToInt(Screen.width * 0.018f);
                float areaHeight = btnSize * 2 + myStyleText.CalcHeight(new GUIContent("A"), 100);
                GUILayout.BeginArea(new Rect(Screen.width * .80f, areaY, Screen.width * .20f, areaHeight));
                GUILayout.BeginHorizontal();
                //some flexible space for the button to be exactly in the middle
                GUILayout.FlexibleSpace();
                if (GUILayout.RepeatButton("+", myStylePlusMinus, GUILayout.Height(btnSize), GUILayout.Width(btnSize)))
                {
                    releasedPlus = 0;
                    pressedPlus++;
                    //speed modes for the controls
                    if (pressedPlus < 100) hSliderValue += 0.01f * Time.fixedDeltaTime * 5;
                    if (pressedPlus >= 100 && pressedPlus < 200) hSliderValue += 0.1f * Time.fixedDeltaTime;
                    if (pressedPlus >= 200) hSliderValue += 1f * Time.fixedDeltaTime;
                    if (pressedPlus >= 400) hSliderValue += 2f * Time.fixedDeltaTime;
                    //if (hSliderValue > maxValue) hSliderValue = minValue;
                    if (hSliderValue > maxValue) hSliderValue = maxValue;
                }
                else if (pressedPlus > 0)
                {
                    releasedPlus++;
                }
                //resetting the counters after 10 OnGUI method calls
                if (releasedPlus > 10) pressedPlus = releasedPlus = 0;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                string label = "";
                switch (inputVariable)
                {
                    case Variable.Gravity:
                        label = "Gravity: " + hSliderValue.ToString("#0.00") + " m:s2";
                        break;
                    case Variable.Velocity:
                        label = "Velocity: " + hSliderValue.ToString("#0.00") + " m:s";
                        break;
                    case Variable.Y:
                        label = "Y: " + hSliderValue.ToString("#0.00") + " m";
                        break;
                }
                GUILayout.Label(label, myStyleText);


                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                //minus button similar to plus button
                if (GUILayout.RepeatButton("-", myStylePlusMinus, GUILayout.Height(btnSize), GUILayout.Width(btnSize)))
                {
                    releasedMinus = 0;
                    pressedMinus++;
                    if (pressedMinus < 100) hSliderValue -= 0.01f * Time.fixedDeltaTime * 5;
                    if (pressedMinus >= 100 && pressedMinus < 200) hSliderValue -= 0.1f * Time.fixedDeltaTime;
                    if (pressedMinus >= 200) hSliderValue -= 1f * Time.fixedDeltaTime;
                    if (pressedMinus >= 400) hSliderValue -= 2f * Time.fixedDeltaTime;
                    //if (hSliderValue < minValue) hSliderValue = maxValue;
                    if (hSliderValue < minValue) hSliderValue = minValue;
                }
                else if (pressedMinus > 0)
                {
                    releasedMinus++;
                }
                if (releasedMinus > 10) pressedMinus = releasedMinus = 0;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                //text field for easy input
                //hSliderValue = float.Parse(GUI.TextField(new Rect(Screen.width/2, 115, 125, 20), hSliderValue.ToString()));


                //second input for level4
                if (Application.loadedLevel == 5)
                {
                    //FFA tag

                    if (!blinking)
                    {
                        float blinkingSize = Screen.width * 0.08f;
                        myStylelvl4.fontSize = Mathf.RoundToInt(Screen.width * 0.012f);
                        GUI.Label(new Rect(Screen.width * 0.01f, Screen.height * .20f, blinkingSize, blinkingSize), "Fire\nat\nWill!", myStylelvl4);
                        StartCoroutine(StartBlinking());
                    }
                    else
                    {
                        StartCoroutine(StartBlinking2());
                    }

                    GUILayout.BeginArea(new Rect(Screen.width * .80f, areaY * 1.5f + areaHeight, Screen.width * .20f, areaHeight));
                    GUILayout.BeginHorizontal();
                    //some flexible space for the button to be exactly in the middle
                    GUILayout.FlexibleSpace();
                    if (GUILayout.RepeatButton("+", myStylePlusMinus, GUILayout.Height(btnSize), GUILayout.Width(btnSize)))
                    {
                        releasedPlus = 0;
                        pressedPlus++;
                        //speed modes for the controls
                        if (pressedPlus < 100) hSliderValue2 += 0.01f * Time.fixedDeltaTime * 5;
                        if (pressedPlus >= 100 && pressedPlus < 200) hSliderValue2 += 0.1f * Time.fixedDeltaTime;
                        if (pressedPlus >= 200) hSliderValue2 += 1f * Time.fixedDeltaTime;
                        if (pressedPlus >= 400) hSliderValue2 += 2f * Time.fixedDeltaTime;
                        //if (hSliderValue > maxValue) hSliderValue = minValue;
                        if (hSliderValue > maxValue) hSliderValue2 = maxValue;
                    }
                    else if (pressedPlus > 0)
                    {
                        releasedPlus++;
                    }
                    //resetting the counters after 10 OnGUI method calls
                    if (releasedPlus > 10) pressedPlus = releasedPlus = 0;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    label = "";
                    switch (inputVariable2)
                    {
                        case Variable.Gravity:
                            label = "Gravity: " + hSliderValue2.ToString("#0.00") + " m:s2";
                            break;
                        case Variable.Velocity:
                            label = "Velocity: " + hSliderValue2.ToString("#0.00") + " m:s";
                            break;
                        case Variable.Y:
                            label = "Y: " + hSliderValue2.ToString("#0.00") + " m";
                            break;
                        case Variable.Angle:
                            label = "Angle: " + hSliderValue2.ToString("#0.00") + " deg";
                            break;
                    }
                    GUILayout.Label(label, myStyleText);


                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    //minus button similar to plus button
                    if (GUILayout.RepeatButton("-", myStylePlusMinus, GUILayout.Height(btnSize), GUILayout.Width(btnSize)))
                    {
                        releasedMinus = 0;
                        pressedMinus++;
                        if (pressedMinus < 100) hSliderValue2 -= 0.01f * Time.fixedDeltaTime * 5;
                        if (pressedMinus >= 100 && pressedMinus < 200) hSliderValue2 -= 0.1f * Time.fixedDeltaTime;
                        if (pressedMinus >= 200) hSliderValue2 -= 1f * Time.fixedDeltaTime;
                        if (pressedMinus >= 400) hSliderValue2 -= 2f * Time.fixedDeltaTime;
                        //if (hSliderValue < minValue) hSliderValue = maxValue;
                        if (hSliderValue < minValue) hSliderValue2 = minValue;
                    }
                    else if (pressedMinus > 0)
                    {
                        releasedMinus++;
                    }
                    if (releasedMinus > 10) pressedMinus = releasedMinus = 0;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();

                    //text field for easy input
                    //hSliderValue2 = float.Parse(GUI.TextField(new Rect(Screen.width - 135, 300, 125, 20), hSliderValue2.ToString()));
                }

                //pause button
                //GUILayout.BeginArea(new Rect(150, 20, 100, 100));
                float size = absFromPercent(7, Screen.width);
                GUILayout.BeginArea(new Rect(absFromPercent(10, Screen.width), absFromPercent(2, Screen.height), size, size));
                if (GUILayout.Button(new GUIContent(pause), customButton))
                {
                    Time.timeScale = 0;
                    menuType = "pause";
                }
                GUILayout.EndArea();

                //restart button
                //GUILayout.BeginArea(new Rect(20, 20, 100, 100));
                GUILayout.BeginArea(new Rect(absFromPercent(2, Screen.width), absFromPercent(2, Screen.height), size, size));
                if (GUILayout.Button(new GUIContent(restart), customButton))
                {
                    Application.LoadLevel(Application.loadedLevel);
                    setPrefs();
                }
                GUILayout.EndArea();

                //area for GUI texture shoot button
                float shootSize = Screen.width * 0.15f;
                customButton2.fontSize = Mathf.RoundToInt(Screen.width * 0.023f);
                GUILayout.BeginArea(new Rect(Screen.width * .87f, Screen.height * .72f, shootSize, shootSize));
                // Make a background box
                //GUILayout.Box("Menu",GUILayout.Width(120),GUILayout.Height(150));
                //some menu buttons
                if (GUILayout.Button("Shoot!", customButton2, GUILayout.Height(shootSize), GUILayout.Width(shootSize)) && canShoot)
                {
                    cannonScript.easyShoot();
                    setPrefs();
                    nTries++;
                    if (Application.loadedLevel != 5)
                    {
                        StopCoroutine("enableShooting");
                        StartCoroutine("enableShooting");
                        canShoot = false;
                    }
                }
                GUILayout.EndArea();

                float aLabelY = Screen.height * 0.01f;
                myStyleText2.fontSize = Mathf.RoundToInt(Screen.width * 0.025f);
                GUIContent content = new GUIContent("Angle: " + cannonScript.getAngle() + " Deg");
                Vector2 labelSize = myStyleText2.CalcSize(content);
                GUI.Label(new Rect((Screen.width - labelSize.x) / 2, aLabelY, labelSize.x, labelSize.y), content, myStyleText2);

                float yLabelY = Screen.height * 0.90f;
                content.text = "X distance: " + cannonScript.getX().ToString("#0.00") + " M\n";
                labelSize = myStyleText2.CalcSize(content);
                GUI.Label(new Rect((Screen.width - labelSize.x) / 2, yLabelY - labelSize.y * 0.5f, labelSize.x, labelSize.y), content, myStyleText2);
                content.text = "Y distance: " + cannonScript.getY().ToString("#0.00") + " M\n";
                labelSize = myStyleText2.CalcSize(content);
                GUI.Label(new Rect((Screen.width - labelSize.x) / 2, yLabelY, labelSize.x, labelSize.y), content, myStyleText2);

                switch (inputVariable)
                {
                    case Variable.Gravity:
                        content.text = "Velocity: " + (cannonScript.getForce() * Time.fixedDeltaTime / cannonScript.getMass()).ToString("#0.00") + " m:s";
                        break;
                    case Variable.Velocity:
                        content.text = "Gravity: " + cannonScript.getGravity().ToString("#0.00") + " M:S2";
                        break;
                    case Variable.Y:
                        content.text = "Gravity: " + cannonScript.getGravity().ToString("#0.00") + " m:s2";
                        labelSize = myStyleText2.CalcSize(content);
                        GUI.Label(new Rect((Screen.width - labelSize.x) / 2, aLabelY + labelSize.y * 2f, labelSize.x, labelSize.y), content, myStyleText2);
                        content.text = "Velocity: " + (cannonScript.getForce() * Time.fixedDeltaTime / cannonScript.getMass()).ToString("#0.00") + " m:s";
                        break;
                }
                labelSize = myStyleText2.CalcSize(content);
                GUI.Label(new Rect((Screen.width - labelSize.x) / 2, aLabelY + labelSize.y, labelSize.x, labelSize.y), content, myStyleText2);

                if (helpPanel)
                {
                    myStyleHelpPanel.fontSize = Mathf.RoundToInt(Screen.width * fontFactor);
                    if (Application.loadedLevel == 2)
                    {
                        content.text = "The goal of the game is to fire the ragdoll onto the barrel.\n\n To achieve this " +
                            "you have to calculate the missing variable\n\n" +
                            "Formulas and definitions to help you getting started:\n\n" +
                            "x = v * cos(A) * t\n\n" +
                            "y = v * sin(A) * t - (1/2) * g  * t^2\n\n" +

                                "v: initial velocity\n" +
                                "A: angle of velocity\n" +
                                "g: gravitational constant\n" +
                                "t: time\n" +
                                "x: horizontal distance\n" +
                                "y: vertical distance";
                    }
                    else if (Application.loadedLevel == 3)
                    {
                        content.text =
                            "x = v * cos(A) * t\n\n" +
                            "y = v * sin(A) * t - (1/2) * g  * t^2\n\n" +

                                "v: initial velocity\n" +
                                "A: angle of velocity\n" +
                                "g: gravitational constant\n" +
                                "t: time\n" +
                                "x: horizontal distance\n" +
                                "y: vertical distance\n\n" +
                                "Level 2 perk: Get the crates for extra points!";
                    }
                    else if (Application.loadedLevel == 4)
                    {
                        content.text =
                            "x = v * cos(A) * t\n\n" +
                            "y = v * sin(A) * t - (1/2) * g  * t^2\n\n" +

                                "v: initial velocity\n" +
                                "A: angle of velocity\n" +
                                "g: gravitational constant\n" +
                                "t: time\n" +
                                "x: horizontal distance\n" +
                                "y: vertical distance\n\n\n" +
                                "Get the crates for extra points!\n\n\n" +
                                "Level 3 perk: avoid the obstacles!";
                    }
                    else if (Application.loadedLevel == 5)
                    {
                        content.text =
                            "x = v * cos(A) * t\n\n" +
                            "y = v * sin(A) * t - (1/2) * g  * t^2\n\n" +

                                "v: initial velocity\n" +
                                "A: angle of velocity\n" +
                                "g: gravitational constant\n" +
                                "t: time\n" +
                                "x: horizontal distance\n" +
                                "y: vertical distance\n\n\n" +
                                "Get the crate for extra points!\n\n" +
                                "Avoid the obstacles!\n\n" +
                                "Level 4 perk: Free for all, shoot as much as you want!\n\n" +
                                "Mind the wind!";
                    }
                    labelSize = myStyleHelpPanel.CalcSize(content);
                    float tutWidth = labelSize.x * 1.2f;
                    float tutHeight = labelSize.y * 1.3f;
                    Rect rectTutorial = new Rect((Screen.width - tutWidth) / 2, (Screen.height - tutHeight) / 2, tutWidth, tutHeight);
                    GUI.Label(rectTutorial, content, myStyleHelpPanel);

                    float closeSize = Screen.width * 0.06f;
                    float closeMargin = Screen.width * 0.013f;
                    Rect rectTutorialCLose = new Rect(rectTutorial.xMax - closeSize / 2 - closeMargin, rectTutorial.y - closeSize / 2 + closeMargin, closeSize, closeSize);
                    if (GUI.Button(rectTutorialCLose, "", myStyleHelpPanelClose))
                    {
                        tutClose = false;
                        helpPanel = false;
                    }

                }
                break;

            case "pause":
                float btnHeight = Screen.height * 0.09f;
                float btnAreaWidth = Screen.width * 0.15f;
                float btnAreaHeight = btnHeight * 6 * 1.2f;
                GUILayout.BeginArea(new Rect((Screen.width - btnAreaWidth) / 2, (Screen.height - btnAreaHeight) / 2, btnAreaWidth, btnAreaHeight));
                if (GUILayout.Button("Continue", GUILayout.Height(btnHeight)))
                {
                    Time.timeScale = 1;
                    menuType = "inGame";
                }
                if (GUILayout.Button("Retry", GUILayout.Height(btnHeight)))
                {
                    Time.timeScale = 1;
                    Application.LoadLevel(Application.loadedLevel);
                }
                if (GUILayout.Button("Level selector", GUILayout.Height(btnHeight)))
                {
                    Application.LoadLevel(1);
                    Time.timeScale = 1;
                }

                if (GUILayout.Button("Options", GUILayout.Height(btnHeight)))
                {
                    menuType = "options";
                }
                if (GUILayout.Button("Exit to Menu", GUILayout.Height(btnHeight)))
                {
                    Time.timeScale = 1;
                    Application.LoadLevel(0);
                }
                if (GUILayout.Button("Exit", GUILayout.Height(btnHeight)))
                {
                    Application.Quit();
                }
                GUILayout.EndArea();
                break;
            case "options":
                btnHeight = Screen.height * 0.09f;
                btnAreaWidth = Screen.width * 0.15f;
                btnAreaHeight = btnHeight * 2 * 1.2f;
                GUILayout.BeginArea(new Rect((Screen.width - btnAreaWidth) / 2, (Screen.height - btnAreaHeight) / 2, btnAreaWidth, btnAreaHeight));
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                musicOn = GUILayout.Toggle(musicOn, "Music", GUILayout.Height(btnHeight), GUILayout.ExpandWidth(true));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Back", GUILayout.Height(btnHeight)) || Input.GetKeyDown(KeyCode.Escape))
                {
                    menuType = "pause";
                    if (musicOn) PlayerPrefs.SetInt("musicOn", 1);
                    else PlayerPrefs.SetInt("musicOn", 0);
                    PlayerPrefs.Save();
                }
                GUILayout.EndArea();
                break;
            case "win":
                btnHeight = Screen.height * 0.09f;
                btnAreaWidth = Screen.width * 0.15f;
                if (Application.loadedLevel < maxLevel)
                {
                    btnAreaHeight = btnHeight * 5 * 1.2f;
                }
                else
                {
                    btnAreaHeight = btnHeight * 4 * 1.2f;
                }
                float areaLeft = (Screen.width - btnAreaWidth) / 2;
                float areaTop = (Screen.height - btnAreaHeight) / 2;
                GUILayout.BeginArea(new Rect(areaLeft, areaTop, btnAreaWidth, btnAreaHeight));

                PlayerPrefs.SetInt("SavedLevel", Mathf.Max(PlayerPrefs.GetInt("SavedLevel"), Application.loadedLevel));
                PlayerPrefs.Save();
                if (Application.loadedLevel < maxLevel)
                {
                    if (GUILayout.Button("Next level", GUILayout.Height(btnHeight)))
                    {
                        Time.timeScale = 1;
                        Application.LoadLevel(Application.loadedLevel + 1);
                    }
                }
                if (GUILayout.Button("Retry", GUILayout.Height(btnHeight)))
                {
                    Time.timeScale = 1;
                    Application.LoadLevel(Application.loadedLevel);
                }
                if (GUILayout.Button("Level selector", GUILayout.Height(btnHeight)))
                {
                    Time.timeScale = 1;
                    Application.LoadLevel(1);
                }
                if (GUILayout.Button("Exit to Menu", GUILayout.Height(btnHeight)))
                {
                    Application.LoadLevel(0);
                    Time.timeScale = 1;
                }
                if (GUILayout.Button("Exit", GUILayout.Height(btnHeight)))
                {
                    Application.Quit();
                }

                GUILayout.EndArea();

                myStyle.fontSize = Mathf.RoundToInt(Screen.width * 0.025f);

                //call function that calculate bonus + prints the amount of point for current level
                returnPoint(nTries);
                content = new GUIContent("Well done!\n" + "# of attempts: " + nTries.ToString() + "\n" + triesPoints.ToString() + " bonus points");
                labelSize = myStyle.CalcSize(content);
                float w = labelSize.x * 1.5f;
                float h = labelSize.y * 1.5f;
                GUI.Label(new Rect(areaLeft - w * 1.05f, Screen.height * .10f, w, h), content, myStyle);

                //Updates playerpref totalscore
                fetchedPoints = PlayerPrefs.GetInt("Player Score");
                GUI.Label(new Rect(areaLeft + btnAreaWidth + w * .05f, Screen.height * 0.10f, w, h), "Total Score: \n" + fetchedPoints.ToString(), myStyle2);

                break;
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && menuType.Equals("inGame"))
        {
            Time.timeScale = 0;
            menuType = "pause";
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuType = "inGame";
            Time.timeScale = 1;
        }
        if (hasWon)
        {
            hasWon = false;
            menuType = "win";
            Time.timeScale = 1;
        }

        if (!canShoot && GameObject.FindGameObjectWithTag("missile") == null)
        {
            canShoot = true;
            StopCoroutine("enableShooting");
        }
    }
    public void setPrefs()
    {
        PlayerPrefs.SetFloat(inputVariable.ToString(), hSliderValue);
        PlayerPrefs.Save();
    }

    public float getInputValue()
    {
        return hSliderValue;
    }

    public float getInputValue2()
    {
        return hSliderValue2;
    }

    private IEnumerator enableShooting()
    {
        yield return new WaitForSeconds(5);
        canShoot = true;
        StopCoroutine("enableShooting");
    }


    private IEnumerator StartBlinking()
    {
        yield return new WaitForSeconds(1);
        blinking = true;

    }

    private IEnumerator StartBlinking2()
    {
        yield return new WaitForSeconds(1);
        blinking = false;

    }

    private float absFromPercent(float percent, float total)
    {
        return percent / 100 * total;
    }

    public int returnPoint(int nTries)
    {
        switch (nTries)
        {
            case 1:
                triesPoints = 5000;
                break;

            case 2:
                triesPoints = 3000;
                break;

            case 3:
                triesPoints = 1000;
                break;

            default:
                triesPoints = 0;
                break;
        }
        return triesPoints;
    }


}
