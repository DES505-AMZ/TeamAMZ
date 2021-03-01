using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Evidences/New Item")]
public class EvidenceItem : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    [TextArea]
    public string itemInfo;
}
