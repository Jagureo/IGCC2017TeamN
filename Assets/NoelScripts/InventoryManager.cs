using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour {

    private int[] inventorySlot;
    private bool triggerRender;
    private int cannotUsePop;
    private int openWardrobe;
    private bool lockDown;
    private GameObject renderButton;

    public Sprite slotEmpty;
    public Sprite slotTools;
    public Sprite slotPillow;
    public Sprite slotCup;

    private GameObject cannotUseBox;
    private GameObject wardrobeSelection;

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
    }
	
	// Update is called once per frame
	void Update () {
	    if(triggerRender == true)
        {
            rerenderButtons();
            triggerRender = false;
        }

        //inventory popup
        if(cannotUsePop == 1)
        {
            if(cannotUseBox.transform.position.y < -60)
            {
                cannotUsePop = 0;
            }
            else
            {
                cannotUseBox.transform.Translate(0, Mathf.Sin((180 - cannotUseBox.transform.position.y + 60) * Mathf.Deg2Rad) - 1, 0);
            }
        }
        else if(cannotUsePop == 2)
        {
            if(cannotUseBox.transform.position.y > 120)
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
                wardrobeSelection.transform.Translate(0, Mathf.Sin(((wardrobeSelection.transform.position.y + 90.0f) * 0.2f) * Mathf.Deg2Rad)  *0.01f - 6, 0);
            }
        }
        else if (openWardrobe == 2)
        {
            if (wardrobeSelection.transform.position.y < 360)
            {
                wardrobeSelection.transform.Translate(0, -Mathf.Sin(((wardrobeSelection.transform.position.y + 90.0f) * 0.2f) * Mathf.Deg2Rad) *0.01f + 4, 0);
            }
        }
        Debug.Log((wardrobeSelection.transform.position.y + 90.0f) * Mathf.Deg2Rad);

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

    }

    // Add item to inventory
    public void addItem(int id)
    {
        for(int i = 0; i < 12; i++)
        {
            if(inventorySlot[i] == 0)
            {
                inventorySlot[i] = id;
                triggerRender = true;
                return;
            }
        }
        return;
    }

    // Use item
    public void useItem(int slot)
    {
        if(inventorySlot[slot] != 0)
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
        for(int i = 0; i < 12; i++)
        {
            if(inventorySlot[i] == 0)
            {
                for(int j = i; j < 11; j++)
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
            switch(inventorySlot[i])
            {
                case 0:
                    renderButton.GetComponent<Image>().sprite = slotEmpty;
                    break;
                case 1:
                    renderButton.GetComponent<Image>().sprite = slotTools;
                    break;
                case 2:
                    renderButton.GetComponent<Image>().sprite = slotPillow;
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
        if(inventorySlot[slot] > 0)
        {
            switch(inventorySlot[slot])
            {
                case 1:
                    // Toolbox
                    return true;
                case 2:
                    // Pillow
                    return true;
                case 3:
                    // Cup
                    return true;
            }
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
            if(clothesOrTools == 2 && EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite != slotEmpty)
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
            if(clothesOrTools == 1 && EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite != slotEmpty)
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
}
