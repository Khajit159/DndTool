using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickInfo : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private int PosOf;
    public string buttonName;
    public string CustomEntityDirectory;
    public bool AllowDelete = false;
    public MessageBoxInfo msgbox;
    public Category_Handler ctgrHandler;

    private void Start()
    {
        CustomEntityDirectory = (Application.persistentDataPath + "/CustomEntity");
    }

    private void HideAll()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        this.gameObject.transform.parent.gameObject.transform.position = new Vector3(0, 300, 0);
    }

    public void ShowMenu(string[] MenusYouWant, string EntityName)
    {
        AllowDelete = false;
        this.gameObject.transform.parent.gameObject.transform.position = Input.mousePosition + new Vector3(0, -10, 0);
        foreach (string menu in MenusYouWant)
        {
            this.transform.Find(menu).gameObject.SetActive(true);
        }
        buttonName = EntityName;
    }


    public void UseEdit()
    {
        return;
    }

    public void UseDelete()
    {
        if (!AllowDelete)
        {
            msgbox.CreateMessageBox(new Vector2(0, -10), 6, new string[] { "Do you really want to delete " + buttonName + "?" , "Press delete again" });
            AllowDelete = true;
            return;
        }
        if (File.Exists(CustomEntityDirectory + "/" + buttonName + ".txt"))
        {
            File.Delete(CustomEntityDirectory + "/" + buttonName + ".txt");
            ctgrHandler.Update_Entity_Category();
            AllowDelete = false;
            msgbox.PromptMSGBoxHide();
            HideAll();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideAll();
    }
}
