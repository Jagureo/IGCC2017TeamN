﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{

    private int[] inventorySlot;
    private bool triggerRender;
    private int cannotUsePop;
    private int openWardrobe;
    private bool lockDown;
    private GameObject renderButton;
    private GameObject invbackground;

    //public Sprite slotEmpty;
    //public Sprite slotTools;
    //public Sprite slotPillow;
    //public Sprite slotCup;

    private Sprite slotEmpty;
    private Sprite slotTools;
    private Sprite slotToolsDragging;
    private Sprite slotPillowGirl;
    private Sprite slotPillowGirlDragging;
    private Sprite slotPillowBoy;
    private Sprite slotPillowBoyDragging;
    private Sprite slotCup;
    private Sprite slotCupDragging;

    private GameObject cannotUseBox;
    private GameObject wardrobeSelection;

    private GameObject playerChar;
    private Image tapper;
    private int tapDelay;

    private GameObject playerResponse;
    private int imageTimer;
    private Vector3 lastClickedPos;

    private bool dragging;
    private int prevSlot;
    private int prevItem;

    public bool gotToolbox = false;
    public bool gotPillow = false;
    public bool gotMug = false;

    private bool plantTriggered = false;
    private bool chestTriggered = false;

    private float distanceTravelled;

    private int awakeTimer;
    private int garbageTimer;

    private bool flyingMug;
    public Sprite brokenMug;
    public Sprite brokenPlant;
    Camera cam;

    [SerializeField]
    private DialogWindow dialogWindow;

    private const string FACEBOOK_APP_ID = "911289349024995";
    private const string FACEBOOK_URL = "http://www.facebook.com/dialog/feed";

    void ShareToFacebook(string linkParameter, string nameParameter, string captionParameter, string descriptionParameter, string pictureParameter, string redirectParameter)
    {
        Application.OpenURL(FACEBOOK_URL + "?app_id=" + FACEBOOK_APP_ID +
        "&link=" + WWW.EscapeURL(linkParameter) +
        "&name=" + WWW.EscapeURL(nameParameter) +
        "&caption=" + WWW.EscapeURL(captionParameter) +
        "&description=" + WWW.EscapeURL(descriptionParameter) +
        "&picture=" + WWW.EscapeURL(pictureParameter) +
        "&redirect_uri=" + WWW.EscapeURL(redirectParameter));
    }

    // Use this for initialization
    void Start()
    {
        inventorySlot = new int[12];
        for (int i = 0; i < 12; i++)
        {
            inventorySlot[i] = 0;
        }

        triggerRender = true;
        lockDown = false;

        cannotUsePop = 0;
        cannotUseBox = GameObject.Find("CannotUsePopupBox");
        cannotUseBox.transform.position = new Vector3(cannotUseBox.transform.position.x, -60, 0);

        openWardrobe = 0;
        wardrobeSelection = GameObject.Find("WardrobePopup");
        wardrobeSelection.transform.position = new Vector3(cannotUseBox.transform.position.x, -90, 0);

        playerChar = GameObject.Find("Player");
        tapper = GameObject.Find("TapReaction").GetComponent<Image>();
        playerResponse = GameObject.Find("PlayerReaction");
        imageTimer = -1;
        tapDelay = 1;

        slotEmpty = GameObject.Find("SlotTemp").GetComponent<Image>().sprite;
        slotTools = GameObject.Find("SlotTools").GetComponent<Image>().sprite;
        slotToolsDragging = GameObject.Find("SlotToolsDragging").GetComponent<Image>().sprite;
        slotPillowGirl = GameObject.Find("SlotPillowLady").GetComponent<Image>().sprite;
        slotPillowGirlDragging = GameObject.Find("SlotPillowLadyDragging").GetComponent<Image>().sprite;
        slotPillowBoy = GameObject.Find("SlotPillowBoy").GetComponent<Image>().sprite;
        slotPillowBoyDragging = GameObject.Find("SlotPillowBoyDragging").GetComponent<Image>().sprite;
        slotCup = GameObject.Find("SlotMug").GetComponent<Image>().sprite;
        slotCupDragging = GameObject.Find("SlotMugDragging").GetComponent<Image>().sprite;
        invbackground = GameObject.Find("area");

        awakeTimer = 30;
        garbageTimer = 60;
        flyingMug = false;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(flyingMug == true)
        {
            //GameObject.Find("SlotMug").transform.Translate(new Vector3(11, -10, 0));
            var mug = GameObject.Find("SlotMug");
            var planterPos = cam.WorldToScreenPoint(GameObject.Find("planter").transform.position + new Vector3(5.0f, 5.0f));
            var direction = (planterPos - mug.transform.position).normalized;
            mug.transform.Translate(direction * 10.0f);
            
            //if(GameObject.Find("SlotMug").transform.position.x > 380 && GameObject.Find("SlotMug").transform.position.y < 50)
            //{
            //    flyingMug = false;
            //}
            if (Vector3.Distance(GameObject.Find("SlotMug").transform.position, cam.WorldToScreenPoint(GameObject.Find("planter").transform.position)) < 30)
            {
                var player = FindObjectOfType<PlayerController>();

                player.planter = true;
                GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("MugPlace");
                ProgressionManager.Instance.ChangeProgression("ThrowsMugAtPlant");
                flyingMug = false;
                GameObject.Find("SlotMug").GetComponent<Image>().sprite = brokenMug;
                GameObject.Find("planter").GetComponent<SpriteRenderer>().sprite = brokenPlant;
            }
        }

        if(garbageTimer < 0)
        {
            garbageTimer = 60;
            System.GC.Collect();
        }
        else
        {
            garbageTimer--;
        }

        if (plantTriggered == false)
        {
            if (Vector3.Distance(GameObject.Find("planter").transform.position, GameObject.Find("Player").transform.position) < 100)
            {
                ProgressionManager.Instance.ChangeProgression("PlayerInteractsWithPlant");
                plantTriggered = true;
            }
        }
        else
        {
            if (Vector3.Distance(GameObject.Find("planter").transform.position, GameObject.Find("Player").transform.position) > 100)
            {
                plantTriggered = false;
            }
        }

        if (chestTriggered == false)
        {
            if (Vector3.Distance(GameObject.Find("chest").transform.position, GameObject.Find("Player").transform.position) < 50)
            {
                ProgressionManager.Instance.ChangeProgression("PlayerWalksOverToCupboard");
                chestTriggered = true;
            }
        }
        else
        {
            if (Vector3.Distance(GameObject.Find("chest").transform.position, GameObject.Find("Player").transform.position) > 50)
            {
                chestTriggered = false;
            }
        }

        if (awakeTimer == 0)
        {
            dialogWindow.AddDialog(ProgressionManager.Instance.CurrentProgression);
            awakeTimer--;
        }
        else
        {
            awakeTimer--;
        }

        if (GameObject.Find("Player").GetComponent<PlayerController>().gender == 1)
        {
            invbackground.GetComponent<Image>().sprite = GameObject.Find("area2").GetComponent<Image>().sprite;
        }
        else
        {
            invbackground.GetComponent<Image>().sprite = GameObject.Find("area3").GetComponent<Image>().sprite;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > 0.84 * Screen.width)
            {
                pickupObject();
            }
            else
            {
                distanceTravelled += Vector3.Distance(GameObject.Find("Player").transform.position, Input.mousePosition);
                if (Vector3.Distance(GameObject.Find("Player").transform.position, Input.mousePosition) > 0.4f * Screen.width)
                {
                    ProgressionManager.Instance.ChangeProgression("PlayerMovesToOtherSide");
                }
                else if(distanceTravelled > 3.0f* Screen.width)
                {
                    ProgressionManager.Instance.ChangeProgression("StartLookingAroundTheRoom");
                    distanceTravelled = 0;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && dragging == true)
        {
            if (checkUsable(prevItem, true))
            {
                useItem(prevSlot - 1);
                rerenderButtons();
            }
            else
            {
                releaseObject();
                rerenderButtons();
            }

            dragging = false;
        }

        if (dragging == true)
        {
            GameObject.Find("SlotTemp").transform.position = Input.mousePosition;
        }
        else
        {
            GameObject.Find("SlotTemp").transform.position = new Vector3(9999, 9999, 9999);
        }

        if (playerChar.transform.position.y < 45 && playerChar.transform.position.x > 350 && ProgressionManager.Instance.CurrentProgression != "PlayerClicksOnScreen" && gotToolbox == false)
        {
            ProgressionManager.Instance.ChangeProgression("PlayerClicksOnScreen");
        }

        if (triggerRender == true)
        {
            rerenderButtons();
            triggerRender = false;
        }

        //inventory popup
        if (cannotUsePop == 1)
        {
            if (cannotUseBox.transform.position.y < -60)
            {
                cannotUsePop = 0;
            }
            else
            {
                cannotUseBox.transform.Translate(0, Mathf.Sin((180 - cannotUseBox.transform.position.y + 60) * Mathf.Deg2Rad) - 1, 0);
            }
        }
        else if (cannotUsePop == 2)
        {
            if (cannotUseBox.transform.position.y > 120)
            {
                cannotUsePop = 1;
            }
            else
            {
                cannotUseBox.transform.Translate(0, -Mathf.Sin((180 - cannotUseBox.transform.position.y + 60) * Mathf.Deg2Rad) + 1, 0);
            }
        }

        // wardrobe popup
        if (openWardrobe == 1)
        {
            if (wardrobeSelection.transform.position.y < -90)
            {
                openWardrobe = 0;
            }
            else
            {
                wardrobeSelection.transform.Translate(0, Mathf.Sin(((wardrobeSelection.transform.position.y + 90.0f) * 0.2f) * Mathf.Deg2Rad) * 0.01f - 6, 0);
            }
        }
        else if (openWardrobe == 2)
        {
            if (wardrobeSelection.transform.position.y < 360)
            {
                wardrobeSelection.transform.Translate(0, -Mathf.Sin(((wardrobeSelection.transform.position.y + 90.0f) * 0.2f) * Mathf.Deg2Rad) * 0.01f + 4, 0);
            }
        }


#if UNITY_ANDROID
        if(Input.touches.Length == 3)
        {
            //addItem(Random.Range(1, 3));
            //openWardrobeMenu();
            ShareToFacebook("https://i.imgur.com/n3tEE8F.png", "Testing Post", "CaptionPost", "Description", "www.google.com", "http://www.facebook.com/");
            triggerPlayerQuestionMark();
        }
#else
        if (Input.GetKeyDown(KeyCode.A))
        {
            addItem(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            addItem(2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            addItem(3);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            openWardrobeMenu();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            triggerPlayerQuestionMark();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(GameObject.Find("Player").transform.position);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("DoorLock");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            testDialog();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ProgressionManager.Instance.ChangeProgression("WebcamIsDisabled");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ShareToFacebook("https://i.imgur.com/n3tEE8F.png", "Testing Post", "CaptionPost", "Description", "www.google.com", "http://www.facebook.com/");
        }
#endif

        if (tapDelay == 1)
        {
            tapper.transform.position = lastClickedPos;
            tapper.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            tapper.transform.localScale.Set(0.1f, 0.1f, 0.1f);
            tapper.GetComponent<RectTransform>().sizeDelta = new Vector2(10.0f, 10.0f);
            tapDelay = 0;
        }
        else if (tapDelay > 1)
        {
            tapDelay--;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastClickedPos = Input.mousePosition;
            tapDelay = 10;
        }
        if (tapper.color.a > 0)
        {
            tapper.color = new Color(1.0f, 1.0f, 1.0f, tapper.color.a - 0.1f);
            tapper.GetComponent<RectTransform>().sizeDelta = new Vector2(tapper.transform.GetComponent<RectTransform>().sizeDelta.x + 10.0f, tapper.transform.GetComponent<RectTransform>().sizeDelta.y + 10.0f);
        }

        if (imageTimer > 0)
        {
            imageTimer--;
            if (playerResponse.GetComponent<SpriteRenderer>().color.a < 1.0f)
            {
                playerResponse.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, playerResponse.GetComponent<SpriteRenderer>().color.a + 0.1f);
            }
        }
        else
        {
            if (playerResponse.GetComponent<SpriteRenderer>().color.a > 0.0f)
            {
                playerResponse.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, playerResponse.GetComponent<SpriteRenderer>().color.a - 0.1f);
            }
        }
    }

    // Add item to inventory
    public void addItem(int id)
    {
        for (int i = 0; i < 12; i++)
        {
            if (inventorySlot[i] == 0)
            {
                inventorySlot[i] = id;
                triggerRender = true;
                switch (id)
                {
                    case 1:
                        gotToolbox = true;
                        break;
                    case 2:
                        gotPillow = true;
                        break;
                    case 3:
                        gotMug = true;
                        break;
                }
                return;
            }
        }
        return;
    }

    // Use item
    public void useItem(int slot)
    {
        if (inventorySlot[slot] != 0)
        {
            if (checkUsable(slot) == true)
            {
                inventorySlot[slot] = 0;
                triggerRender = true;
                cannotUsePop = 1;
                return;
            }
            else
            {
                cannotUsePop = 2;
            }
        }
        return;
    }

    // Shift inventory
    public void shiftInventory()
    {
        for (int i = 0; i < 12; i++)
        {
            if (inventorySlot[i] == 0)
            {
                for (int j = i; j < 11; j++)
                {
                    inventorySlot[j] = inventorySlot[j + 1];
                }
                inventorySlot[11] = 0;
                triggerRender = true;
            }
        }
    }

    // Re-render the buttons
    public void rerenderButtons()
    {
        for (int i = 0; i < 12; i++)
        {
            string myNewString = "Slot" + (i + 1);
            renderButton = GameObject.Find(myNewString);
            switch (inventorySlot[i])
            {
                case 0:
                    renderButton.GetComponent<Image>().sprite = slotEmpty;
                    break;
                case 1:
                    renderButton.GetComponent<Image>().sprite = slotTools;
                    break;
                case 2:
                    if (GameObject.Find("Player").GetComponent<PlayerController>().gender == 0)
                    {
                        renderButton.GetComponent<Image>().sprite = slotPillowBoy;
                    }
                    else
                    {
                        renderButton.GetComponent<Image>().sprite = slotPillowGirl;
                    }
                    break;
                case 3:
                    renderButton.GetComponent<Image>().sprite = slotCup;
                    break;
            }
        }
    }

    // Check if the item can be used
    bool checkUsable(int slot)
    {
        var player = FindObjectOfType<PlayerController>();

        if (inventorySlot[slot] > 0 && playerChar != null)
        {
            switch (inventorySlot[slot])
            {
                case 1:
                    // Toolbox
                    if (playerChar.transform.position.y < 55 && playerChar.transform.position.x > 340)
                    {
                        GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("ErrorSound1");
                        Debug.Log("Can Use toolbox");
                        player.screen = true;
                        ProgressionManager.Instance.ChangeProgression("DoorOpens");
                        return true;
                    }
                    return false;
                case 2:
                    if (playerChar.transform.position.y > 210 && playerChar.transform.position.x > 350)
                    {
                        GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("BushesSound2");
                        Debug.Log("Can Use pillow");
                        player.pc = true;
                        ProgressionManager.Instance.ChangeProgression("WebcamIsDisabled");
                        return true;
                    }
                    // Pillow
                    return false;
                case 3:
                    if (playerChar.transform.position.y > 210 && playerChar.transform.position.x < 260 && gotPillow == true && searchInventory(2) == false)
                    {
                        GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("MugPlace");
                        Debug.Log("Can Use cup");
                        player.planter = true;
                        ProgressionManager.Instance.ChangeProgression("ThrowsMugAtPlant");
                        return true;
                    }
                    // Cup
                    return false;
            }
        }
        return false;
    }

    bool checkUsable(int slot, bool itemInsert)
    {
        var player = FindObjectOfType<PlayerController>();

        switch (slot)
        {
            case 1:
                // Toolbox
                if (playerChar.transform.position.y < 65 && playerChar.transform.position.x > 320)
                {
                    GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("ErrorSound1");
                    Debug.Log("Can Use toolbox");
                    player.screen = true;
                    ProgressionManager.Instance.ChangeProgression("DoorOpens");
                    return true;
                }
                return false;
            case 2:
                if (playerChar.transform.position.y > 190 && playerChar.transform.position.x > 320)
                {
                    GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("BushesSound2");
                    Debug.Log("Can Use pillow");
                    player.pc = true;
                    ProgressionManager.Instance.ChangeProgression("WebcamIsDisabled");
                    return true;
                }
                // Pillow
                return false;
            case 3:
                if (playerChar.transform.position.y > 210 && playerChar.transform.position.x < 230 && gotPillow == true && searchInventory(2) == false)
                {
                    Debug.Log("Can Use cup");
                    GameObject.Find("SlotMug").transform.position = cam.WorldToScreenPoint(GameObject.Find("Player").transform.position);
                    GameObject.Find("SlotMug").transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    flyingMug = true;
                    return true;
                }
                // Cup
                return false;
        }
        return false;
    }

    public void openWardrobeMenu()
    {
        openWardrobe = 2;
    }

    public void selectWardrobeMenu(int clothesOrTools)
    {
        if (lockDown == true)
        {
            if (clothesOrTools == 2 && EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite != slotEmpty)
            {
                addItem(1);
                openWardrobe = 1;
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = slotEmpty;
            }
            else
            {
                openWardrobe = 1;
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = slotEmpty;
            }
        }
        else
        {
            if (clothesOrTools == 1 && EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite != slotEmpty)
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = slotEmpty;
                openWardrobe = 1;
                cannotUsePop = 1;
                // progress through story
            }
            else
            {
                cannotUsePop = 2;
            }
        }
    }

    public void triggerPlayerQuestionMark()
    {
        imageTimer = 180;
    }

    public void pickupObject()
    {
        float shortestDistance = 0.0f;
        int shortestSlot = 0;
        for (int i = 0; i < 12; i++)
        {
            string myNewString = "Slot" + (i + 1);
            renderButton = GameObject.Find(myNewString);
            float distance = Vector3.Distance(renderButton.transform.position, Input.mousePosition);
            if (shortestSlot == 0 || shortestDistance > distance)
            {
                shortestSlot = i + 1;
                shortestDistance = distance;
            }
        }

        if (inventorySlot[shortestSlot - 1] != 0)
        {
            switch (GameObject.Find("Slot" + shortestSlot).GetComponent<Image>().sprite.name)
            {
                case "mug1":
                    GameObject.Find("SlotTemp").GetComponent<Image>().sprite = slotCupDragging;
                    break;
                case "pillow_female1":
                    GameObject.Find("SlotTemp").GetComponent<Image>().sprite = slotPillowGirlDragging;
                    break;
                case "pillow_male1":
                    GameObject.Find("SlotTemp").GetComponent<Image>().sprite = slotPillowBoyDragging;
                    break;
                case "tools1":
                    GameObject.Find("SlotTemp").GetComponent<Image>().sprite = slotToolsDragging;
                    break;
            }
            //GameObject.Find("SlotTemp").GetComponent<Image>().sprite = GameObject.Find("Slot" + shortestSlot).GetComponent<Image>().sprite;
            GameObject.Find("SlotTemp").transform.position = Input.mousePosition;
            dragging = true;

            string myNewString = "Slot" + shortestSlot;
            renderButton = GameObject.Find(myNewString);
            renderButton.GetComponent<Image>().sprite = slotEmpty;
            prevItem = inventorySlot[shortestSlot - 1];
            inventorySlot[shortestSlot - 1] = 0;

            prevSlot = shortestSlot;
        }
    }

    public void releaseObject()
    {
        float shortestDistance = 0.0f;
        int shortestSlot = 0;
        for (int i = 0; i < 12; i++)
        {
            string myNewString = "Slot" + (i + 1);
            renderButton = GameObject.Find(myNewString);
            float distance = Vector3.Distance(renderButton.transform.position, Input.mousePosition);
            if (shortestSlot == 0 || shortestDistance > distance)
            {
                shortestSlot = i + 1;
                shortestDistance = distance;
            }
        }
        cannotUsePop = 2;
        if (inventorySlot[shortestSlot - 1] == 0)
        {
            GameObject.Find("SlotTemp").transform.position = new Vector3(9999, 9999, 9999);
            dragging = false;

            string myNewString = "Slot" + shortestSlot;
            renderButton = GameObject.Find(myNewString);
            renderButton.GetComponent<Image>().sprite = GameObject.Find("SlotTemp").GetComponent<Image>().sprite;

            inventorySlot[shortestSlot - 1] = prevItem;
        }
        else
        {
            inventorySlot[prevSlot - 1] = prevItem;
            GameObject.Find("SlotTemp").transform.position = new Vector3(9999, 9999, 9999);
            triggerRender = true;
        }
    }

    public void checkAdd(int id)
    {
        if (id == 1 && gotToolbox == true)
        {
            return;
        }
        if (id == 2 && gotPillow == true)
        {
            return;
        }
        if (id == 3 && gotMug == true)
        {
            return;
        }

        switch (id)
        {
            case 1:
                GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("MugPickup");
                break;
            case 2:
                GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("BushesSound1");
                break;
            case 3:
                GameObject.Find("PersistentSoundManager").GetComponent<soundPlayer>().PlaySoundEffect("MugDrop");
                break;
        }

        addItem(id);
    }

    public void testDialog()
    {
        dialogWindow.AddDialog(ProgressionManager.Instance.CurrentProgression);
    }

    public bool searchInventory(int id)
    {
        for (int i = 0; i < 12; i++)
        {
            if (inventorySlot[i] == id)
            {
                return true;
            }
        }
        return false;
    }
}