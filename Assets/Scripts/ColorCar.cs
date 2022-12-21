using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCar : MonoBehaviour
{
    public Renderer rend;
    public Slider redSlider, greenSlider, blueSlider;
    public Text redSliderText, greenSliderText, blueSliderText;
    public Color col;

    public static Color IntToColor(int red, int green, int blue)
    {
        float r = (float)red /255;
        float g = (float)green /255;
        float b = (float)blue /255;

        Color color = new Color(r, g, b);
        return color;
    }

    void SetCarColor(int red, int green, int blue)
    {
        Color color = IntToColor(red, green, blue);
        rend.material.color = color;
        PlayerPrefs.SetInt("Red", red);
        PlayerPrefs.SetInt("Green", green);
        PlayerPrefs.SetInt("Blue", blue);
    }

    private void Start()
    {
        int red = 0, green = 0, blue = 0;

        if (PlayerPrefs.HasKey("Red"))
        {
            red = PlayerPrefs.GetInt("Red");
            green = PlayerPrefs.GetInt("Green");
            blue = PlayerPrefs.GetInt("Blue");
        }
        col = IntToColor(red, green, blue);
        rend.material.color = col;

        redSlider.value = red;
        greenSlider.value = green;
        blueSlider.value = blue;
    }

    private void Update()
    {
        int red = (int)redSlider.value, 
            green = (int)greenSlider.value, 
            blue = (int)blueSlider.value;

        SetCarColor(red, green, blue);

        redSliderText.text = red.ToString();
        greenSliderText.text = green.ToString();
        blueSliderText.text = blue.ToString();
    }
}
