using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters;

public class ZE_DemoGUI : MonoBehaviour
{

	public GameObject[] Prefabs;
    public Texture HUETexture;
 
  //  public bool UsePCVersion = false;

	private int currentNomber;
	private GameObject currentInstance;
	private GUIStyle guiStyleHeader = new GUIStyle();
    private float oldIntensity;
    private Color oldAmbientColor;
    float dpiScale;
    private bool isDay;
    private float colorHUE;

	void Start () {
        if (Screen.dpi < 1) dpiScale = 1;
        if (Screen.dpi < 200) dpiScale = 1;
        else dpiScale = Screen.dpi / 200f;
        guiStyleHeader.fontSize = (int)(15f * dpiScale);
		guiStyleHeader.normal.textColor = new Color(0.15f,0.15f,0.15f);
	    ChangeCurrent(0);
    }

	private void OnGUI()
	{
        if (GUI.Button(new Rect(10 * dpiScale, 15 * dpiScale, 135 * dpiScale, 37 * dpiScale), "PREVIOUS EFFECT"))
        {
			ChangeCurrent(-1);
		}
        if (GUI.Button(new Rect(160 * dpiScale, 15 * dpiScale, 135 * dpiScale, 37 * dpiScale), "NEXT EFFECT"))
        {
			ChangeCurrent(+1);
		}

        GUI.DrawTexture(new Rect(12 * dpiScale, 60 * dpiScale, 285 * dpiScale, 15 * dpiScale), HUETexture, ScaleMode.StretchToFill, false, 0);
        float oldColorHUE = colorHUE;
        colorHUE = GUI.HorizontalSlider(new Rect(12 * dpiScale, 80 * dpiScale, 285 * dpiScale, 15 * dpiScale), colorHUE, 0, 360);
        if (Mathf.Abs(oldColorHUE - colorHUE) > 0.001)
            ChangeColor();
    }
	

    void ChangeCurrent(int delta) {
		currentNomber+=delta;
		if (currentNomber> Prefabs.Length - 1)
			currentNomber = 0;
		else if (currentNomber < 0)
			currentNomber = Prefabs.Length - 1;

		if(currentInstance!=null) Destroy(currentInstance);
        currentInstance = Instantiate(Prefabs[currentNomber], transform.position, transform.rotation) as GameObject;
#if UNITY_5_3
        var reactivationTime = currentInstance.GetComponentInChildren<ParticleSystem>().startLifetime;
        if (currentInstance.GetComponentInChildren<ParticleSystem>().loop) reactivationTime = 0;
#else
        var reactivationTime = currentInstance.GetComponentInChildren<ParticleSystem>().main.startLifetime.constantMax;
        if (currentInstance.GetComponentInChildren<ParticleSystem>().main.loop) reactivationTime = 0;
#endif
        CancelInvoke();
        if (reactivationTime > 0.1f)
        {
           
            InvokeRepeating("Reactivate", reactivationTime + 1.5f, reactivationTime + 1.5f);
        }
        
    }

    void Reactivate()
    {
        currentInstance.SetActive(false);
        currentInstance.SetActive(true);
    }

    private Color Hue(float H)
    {
        Color col = new Color(1, 0, 0);
        if (H >= 0 && H < 1)
            col = new Color(1, 0, H);
        if (H >= 1 && H < 2)
            col = new Color(2 - H, 0, 1);
        if (H >= 2 && H < 3)
            col = new Color(0, H - 2, 1);
        if (H >= 3 && H < 4)
            col = new Color(0, 1, 4 - H);
        if (H >= 4 && H < 5)
            col = new Color(H - 4, 1, 0);
        if (H >= 5 && H < 6)
            col = new Color(1, 6 - H, 0);
        return col;
    }

    public struct HSBColor
    {
        public float h;
        public float s;
        public float b;
        public float a;

        public HSBColor(float h, float s, float b, float a)
        {
            this.h = h;
            this.s = s;
            this.b = b;
            this.a = a;
        }
    }

    public HSBColor ColorToHSV(Color color)
    {
        HSBColor ret = new HSBColor(0f, 0f, 0f, color.a);

        float r = color.r;
        float g = color.g;
        float b = color.b;

        float max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
            return ret;

        float min = Mathf.Min(r, Mathf.Min(g, b));
        float dif = max - min;

        if (max > min)
        {
            if (g == max)
                ret.h = (b - r) / dif * 60f + 120f;
            else if (b == max)
                ret.h = (r - g) / dif * 60f + 240f;
            else if (b > g)
                ret.h = (g - b) / dif * 60f + 360f;
            else
                ret.h = (g - b) / dif * 60f;
            if (ret.h < 0)
                ret.h = ret.h + 360f;
        }
        else
            ret.h = 0;

        ret.h *= 1f / 360f;
        ret.s = (dif / max) * 1f;
        ret.b = max;

        return ret;
    }

    public Color HSVToColor(HSBColor hsbColor)
    {
        float r = hsbColor.b;
        float g = hsbColor.b;
        float b = hsbColor.b;
        if (hsbColor.s != 0)
        {
            float max = hsbColor.b;
            float dif = hsbColor.b * hsbColor.s;
            float min = hsbColor.b - dif;

            float h = hsbColor.h * 360f;

            if (h < 60f)
            {
                r = max;
                g = h * dif / 60f + min;
                b = min;
            }
            else if (h < 120f)
            {
                r = -(h - 120f) * dif / 60f + min;
                g = max;
                b = min;
            }
            else if (h < 180f)
            {
                r = min;
                g = max;
                b = (h - 120f) * dif / 60f + min;
            }
            else if (h < 240f)
            {
                r = min;
                g = -(h - 240f) * dif / 60f + min;
                b = max;
            }
            else if (h < 300f)
            {
                r = (h - 240f) * dif / 60f + min;
                g = min;
                b = max;
            }
            else if (h <= 360f)
            {
                r = max;
                g = min;
                b = -(h - 360f) * dif / 60 + min;
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }
        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsbColor.a);
    }

    Material SetMatHUEColor(Material mat, String name, float hueColor)
    {
        var oldColor = mat.GetColor(name);
        var brightness = oldColor.maxColorComponent;
        if (brightness < 0.0001f)
            brightness = 0.0001f;
        var hsv = ColorToHSV(oldColor / brightness);
        hsv.h = hueColor / 360f;
        var color = HSVToColor(hsv) * brightness;
        color.a = oldColor.a;

        mat.SetColor(name, color);
        return mat;
    }

    private void ChangeColor()
    {
        //var color = Hue(colorHUE / 255f);
        var rend = currentInstance.GetComponentsInChildren<Renderer>();
        foreach (var r in rend)
        {
            var mat = r.material;
            if (mat == null)
                continue;
            if (mat.HasProperty("_TintColor"))
            {
                SetMatHUEColor(mat, "_TintColor", colorHUE);
            }
            if (mat.HasProperty("_CoreColor"))
            {
                SetMatHUEColor(mat, "_CoreColor", colorHUE);
            }
            if (mat.HasProperty("_MainColor"))
            {
                SetMatHUEColor(mat, "_MainColor", colorHUE);
            }
            if (mat.HasProperty("_RimColor"))
            {
                SetMatHUEColor(mat, "_RimColor", colorHUE);
            }
        }

        var projectors = currentInstance.GetComponentsInChildren<Projector>();
        foreach (var proj in projectors)
        {
            var mat = proj.material;
            if (mat == null || !mat.HasProperty("_TintColor"))
                continue;

            proj.material = SetMatHUEColor(mat, "_TintColor", colorHUE);
            
        }
        var light = currentInstance.GetComponentInChildren<Light>(true);

        if (light!=null) {
            var hsv = ColorToHSV(light.color);
            hsv.h = colorHUE / 360f;
            light.color = HSVToColor(hsv);
        }
    }
}
