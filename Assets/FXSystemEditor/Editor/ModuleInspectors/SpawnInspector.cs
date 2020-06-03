using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInspector : ModuleInspector<EM_Spawn>
{
    public override string GetWindowName()
    {
        return "Module: Spawn";
    }
    public override void DisplayInspector()
    {
        GUI.Label(new Rect(5, 5, position.width - 10, 25), "Current object: " + Target.gameObject.name);
    }
}


