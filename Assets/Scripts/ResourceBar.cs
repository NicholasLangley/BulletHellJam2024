using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{

    public Slider slider;
    public Image barOutline;
    public Color glowing, normalColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMaxValue(float max)
    {
        slider.maxValue = max;
    }

    public void setValue(float value)
    {
        slider.value = value;
    }

    public void notEnoughEnergy()
    {

    }

    public void glow(bool glow)
    {
        if (glow){barOutline.color = glowing;}
        else { barOutline.color = normalColor; }
    }
}
