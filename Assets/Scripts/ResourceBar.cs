using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{

    public Slider slider;
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
}
