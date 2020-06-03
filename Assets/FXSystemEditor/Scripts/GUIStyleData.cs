using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GUI Style", menuName = "GUIStyle")]
public class GUIStyleData : ScriptableObject {

    public GUIStyle style;
    public GUIStyle selectedStyle;
    public GUIStyle disableStyle;
}
