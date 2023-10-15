using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{
    public string Name;
    public bool IsIconCustom;
    public Texture Icon;
    public string IconName;
    public Color Color;
    public string Armour;
}
