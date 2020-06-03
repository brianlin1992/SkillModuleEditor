using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeInspector : ModuleInspector<EM_Cone>
{
    public override string GetWindowName()
    {
        return "Module: Cone";
    }

    public override void DisplayInspector()
    {
        GUI.Label(new Rect(5, 5, position.width - 10, 25), "Current object: " + Target.gameObject.name);
    }
}
