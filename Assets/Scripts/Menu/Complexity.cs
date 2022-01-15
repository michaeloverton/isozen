using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Complexity : MonoBehaviour
{
    private float complexity;
    private float copies;
    public Slider complexitySlider;
    public TextMeshProUGUI complexityText;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
	{
		//Adds a listener to the complexity slider and invokes a method when the value changes.
		complexitySlider.onValueChanged.AddListener (delegate {ComplexityValueChangeCheck ();});
        complexity = complexitySlider.value;
        complexityText.SetText(complexitySlider.value.ToString());
	}

    public  void ComplexityValueChangeCheck() {
        complexity = complexitySlider.value;
        complexityText.SetText(complexitySlider.value.ToString());
    }

    public int getComplexity() {
        return (int)complexity;
    }
}
