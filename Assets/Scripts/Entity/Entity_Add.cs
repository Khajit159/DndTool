using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Entity_Add : MonoBehaviour
{
    [Header("ColorPicker")]
    [SerializeField] FlexibleColorPicker fcp;

    [Header("Entity Data")]
    [SerializeField] TMP_InputField Entity_Name;
    [SerializeField] Image Entity_PortraitColor;
    public RawImage Entity_Icon;
    [SerializeField] bool Entity_IconIsCustom = false;
    [SerializeField] TMP_InputField Entity_Armour;
    [SerializeField] GameObject Entity_Highlight;

    [Header("Menus")]
    [SerializeField] GameObject IconMenu;
    [SerializeField] GameObject CreateButton;
    [SerializeField] Color ButtonGreenColor;
    [SerializeField] Color ButtonGrayColor;

    [Header("Other Variables")]
    public string dirCustomEntity = "";
    public string dirCustomIcon = "";
    public Category_Handler category_handler;
    public MessageBoxInfo messageboxinfo;
    public EntityData Entity_New;
    public CanvasSampleOpenFileImage _openfileexplorer;

    private bool AlreadyCreatedAllow = false;
    private bool BlockCreateEntity = false;



    private void Start()
    {
        dirCustomEntity = (Application.persistentDataPath + "/CustomEntity");
        dirCustomIcon = (Application.persistentDataPath + "/CustomIcon");
    }

    public void PortraitColorChanged()
    {
        Entity_PortraitColor.color = fcp.color;
    }

    public void OpenIconMenu()
    {
        IconMenu.SetActive(!IconMenu.activeSelf);
    }

    public void OpenEntityMenu()
    {
        IconMenu.SetActive(false);
        Entity_Name.text = "";
        Entity_PortraitColor.color = Color.white;
        Entity_Icon.texture = null;
        Entity_Icon.gameObject.SetActive(false);
        Entity_Armour.text = "0";
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void CreateEntity()
    {
        if (BlockCreateEntity) { return; }

        if (Entity_Name.text == "")
        {
            messageboxinfo.CreateMessageBox(new Vector2(0, -10), 5, new string[] { "Entity need some name"});
            return;
        }

        if (!Directory.Exists(dirCustomEntity)) 
        {
            Directory.CreateDirectory(dirCustomEntity);
        }

        if (File.Exists(dirCustomEntity + "/" + Entity_Name.text + ".txt"))
        {
            if (!AlreadyCreatedAllow)
            {
                AlreadyCreated();
                return;
            }
        }

        EntityData NewEntity = ScriptableObject.CreateInstance<EntityData>();
        NewEntity.Name = Entity_Name.text;
        NewEntity.IsIconCustom = Entity_IconIsCustom;
        NewEntity.Icon = Entity_Icon.texture;
        NewEntity.IconName = _openfileexplorer.FileName;
        NewEntity.Color = Entity_PortraitColor.color;
        NewEntity.Armour = Entity_Armour.text;
        Entity_New = NewEntity;
        string json = JsonUtility.ToJson(NewEntity, true);
        Debug.Log(NewEntity.Icon.name);
        File.WriteAllText(dirCustomEntity + "/" + NewEntity.Name + ".txt", json);
        AlreadyCreatedAllow = false;
        category_handler.Update_Entity_Category();
        StartCoroutine(CreateButtonText("Created..."));

    }

    IEnumerator CreateButtonText(string text)
    {
        BlockCreateEntity = true;
        string defaultButtonText = CreateButton.transform.Find("CreateText").GetComponent<TMP_Text>().text;
        CreateButton.transform.Find("CreateText").GetComponent<TMP_Text>().text = text;
        CreateButton.GetComponent<Image>().color = ButtonGrayColor;
        yield return new WaitForSeconds(3);
        CreateButton.transform.Find("CreateText").GetComponent<TMP_Text>().text = defaultButtonText;
        CreateButton.GetComponent<Image>().color = ButtonGreenColor;
        BlockCreateEntity = false;


    }

    private void AlreadyCreated()
    {
        messageboxinfo.CreateMessageBox(new Vector2(0, -10), 5, new string[]{"This entity with this name already exists", "If you wanna recreate it", "Click create again!"});
        AlreadyCreatedAllow = true;
        return;

    }

    public void SetIcon(string IconName)
    {
        if (IconName == "None")
        {
            Entity_Icon.texture = null;
            Entity_IconIsCustom = false;
            Entity_Icon.gameObject.SetActive(false);
        }
        else
        {
            Texture NewIcon = Resources.Load<Texture>("PortraitIcon/" + IconName);
            Entity_Icon.texture = NewIcon;
            Entity_IconIsCustom = false;
            Entity_Icon.gameObject.SetActive(true);
        }
    }

    public void OpenFileExplorer()
    {
        if (!Directory.Exists(dirCustomIcon))
        {
            Directory.CreateDirectory(dirCustomIcon);
        }
        byte[] bytes = ImageConversion.EncodeToPNG((Texture2D)Entity_Icon.texture);
        File.WriteAllBytes(dirCustomIcon + "/" + _openfileexplorer.FileName + ".png", bytes);
        Entity_IconIsCustom = true;
        Entity_Icon.gameObject.SetActive(true);
    }
}
