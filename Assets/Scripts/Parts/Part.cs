using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : ScriptableObject
{
    [Header("Item Information")]
    public string partName;
    public Sprite itemIcon;
    [TextArea] public string itemDescription;
    public int partID;
}
