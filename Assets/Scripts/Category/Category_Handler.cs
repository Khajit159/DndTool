using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.CompilerServices;

public class Category_Handler : MonoBehaviour
{
    [SerializeField] GameObject Btn_Container;
    [SerializeField] GameObject Item_Container;

    [SerializeField] GameObject Entity_Prefab;
    public string Name;
    public bool IsIconCustom;
    public Texture Icon;
    public string IconName;
    public Color Color;
    public string Armour;
    private FileInfo[] Entity_DirFiles;
    public DirectoryInfo Entity_DirInfo;
    private string dirCustomIcon;

    private void Start()
    {
        Entity_DirInfo = new DirectoryInfo(Application.persistentDataPath + "/CustomEntity");
        dirCustomIcon = (Application.persistentDataPath + "/CustomIcon");
        Update_Entity_Category();
    }

    public void PressBtn(GameObject btn)
    {
        if (Item_Container.transform.Find(btn.name).gameObject.activeSelf)
        {
            HideAll();
        }
        else
        {
            HideAll();
            Item_Container.transform.Find(btn.name).gameObject.SetActive(true);

        }
    }

    private void HideAll()
    {
        for (int i = 0; i < Item_Container.transform.childCount; i++)
        {
            Item_Container.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Update_Entity_Category()
    {
        for (int i = 1; i < this.gameObject.transform.Find("Item_Container").Find("Entity").childCount; i++)
        {
            Destroy(this.gameObject.transform.Find("Item_Container").Find("Entity").GetChild(i).gameObject);
        }

        Entity_DirFiles = Entity_DirInfo.GetFiles();

        foreach (FileInfo file in Entity_DirFiles)
        {
            string fileRead = File.ReadAllText(file.ToString());
            GameObject CustomEntity = Instantiate(Entity_Prefab, Entity_Prefab.transform.position, Quaternion.identity, this.gameObject.transform.Find("Item_Container").GetChild(0));
            JsonUtility.FromJsonOverwrite(fileRead, this);
            CustomEntity.transform.localScale = Vector3.one;
            CustomEntity.transform.Find("Name").Find("Text").GetComponent<TMP_Text>().text = Name;
            CustomEntity.gameObject.name = Name;
            CustomEntity.GetComponent<Image>().color = Color;
            if (Icon == null && IsIconCustom == false)
            {
                CustomEntity.transform.Find("Icon").gameObject.SetActive(false);
            }
            else
            {
                CustomEntity.transform.Find("Icon").gameObject.SetActive(true);
                if (IsIconCustom == false)
                {
                    CustomEntity.transform.Find("Icon").GetComponent<RawImage>().texture = Resources.Load<Texture2D>("PortraitIcon/" + Icon.name);
                }
                else if (IsIconCustom == true)
                {
                    byte[] bytes = File.ReadAllBytes(dirCustomIcon + "/" + IconName + ".png");
                    Texture2D loadTexture = new Texture2D(1, 1);
                    ImageConversion.LoadImage(loadTexture, bytes);
                    CustomEntity.transform.Find("Icon").GetComponent<RawImage>().texture = loadTexture;
                }

            }

            if (IsIconCustom == true)
            {
                CustomEntity.transform.Find("Icon").gameObject.SetActive(true);
            }

        }

    }
}
