using BuildingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMenu : MonoBehaviour
{
    [SerializeField]
    private ConstructionLayer _constructonLayer;
    [Header("Selected GameObject")]
    [SerializeField] Image GameObjectIcon;
    [SerializeField] TMP_Text GameObjectName;
    private bool Is_Entity;
    public GameObject SelectedGameObject { get; private set; }

    [Header("Position")]
    [SerializeField] TMP_InputField X_Pos;
    [SerializeField] TMP_InputField Y_Pos;
    [SerializeField] TMP_InputField Layer_Pos;
    [SerializeField] Toggle FlipX;
    [SerializeField] Toggle FlipY;

    [Header("Rotation")]
    [SerializeField] Slider _sliderRotation;
    [SerializeField] TMP_Text SliderRotation_Text;

    [Header("Scale")]
    [SerializeField] Slider _sliderScale;
    [SerializeField] TMP_Text SliderScale_Text;


    private bool CanContinue = false;
    public void SetSelectedGameObject(GameObject SelectedObject, bool IsEntity = false)
    {
        CanContinue = false;
        Is_Entity = IsEntity;
        SelectedGameObject = SelectedObject;
        GameObjectIcon.sprite = SelectedGameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        GameObjectName.text = SelectedGameObject.name;
        X_Pos.text = SelectedGameObject.transform.position.x.ToString("F2");
        Y_Pos.text = SelectedGameObject.transform.position.y.ToString("F2");
        Layer_Pos.text = SelectedGameObject.transform.position.z.ToString();
        FlipX.isOn = SelectedGameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
        FlipY.isOn = SelectedGameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY;
        if (IsEntity)
        {
            GameObjectIcon.sprite = SelectedGameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
            FlipX.isOn = SelectedGameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().flipX;
            FlipY.isOn = SelectedGameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().flipY;
        }
        _sliderRotation.value = SelectedGameObject.transform.rotation.z;
        _sliderScale.value = SelectedGameObject.transform.localScale.x;
        SliderHandleValueSet();
        CanContinue = true;
    }

    public void ApplyPosition()
    {
        if (SelectedGameObject == null) return;
        if (CanContinue == false) return;

        float Xfloat = float.Parse(X_Pos.text);
        float Yfloat = float.Parse(Y_Pos.text);
        int IntLayer_Pos = Convert.ToInt32(Layer_Pos.text); 
        _constructonLayer.UpdatePos(SelectedGameObject.transform.position, new Vector3(Xfloat, Yfloat, IntLayer_Pos));
        SelectedGameObject.transform.position = new Vector3(Xfloat, Yfloat, IntLayer_Pos);

    }

    public void SliderHandleValueSet()
    {
        if (Input.mouseScrollDelta.y != 0) { return; }
        if (SelectedGameObject == null) return;
        SliderRotation_Text.text = Convert.ToInt32(_sliderRotation.value).ToString();
        SliderScale_Text.text = Convert.ToInt32(_sliderScale.value).ToString();
        if (CanContinue == false) return;
        SetRotation();
        SetScale();
    }

    public void FlipSprite()
    {
        if (SelectedGameObject == null) return;
        if (CanContinue == false) return;
        if (!Is_Entity)
        {
            SelectedGameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = FlipX.isOn;
            SelectedGameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = FlipY.isOn;
        }
        else
        {
            SelectedGameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().flipX = FlipX.isOn;
            SelectedGameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().flipY = FlipY.isOn;
        }
    }

    private void SetRotation()
    {
        SelectedGameObject.transform.rotation = Quaternion.Euler(0f, 0f, _sliderRotation.value);
    }

    private void SetScale()
    {
        SelectedGameObject.transform.localScale = new Vector3(_sliderScale.value, _sliderScale.value, 0);
    }
}
