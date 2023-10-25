using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown DropdownFilter;
    [SerializeField]
    private GameObject[] ItemLists;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeItemList()
    {
        foreach (GameObject itemList in ItemLists) 
        {
            if (DropdownFilter.captionText.text == itemList.name)
            {
                itemList.SetActive(true);
            }
            else { itemList.SetActive(false);}
        }
    }
}
