using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public List<Color> directionLightColors;
    public GameObject directionalLightsObject;
    public List<Light> directionalLights;
    public List<Color> pointLightColors;
    public GameObject pointLightObject;
    public List<Light> pointLights;
    bool showDirLights = true;

    void Start() {

    }

    public List<Color> DirectionLightColors() {
        List<Color> copy = new List<Color>();
        foreach(Color c in directionLightColors) {
            copy.Add(c);
        }
        
        List<Color> randomizedColors = new List<Color>();
        for(int i=0; i<directionLightColors.Count; i++) {
            int removalIndex = Random.Range(0, copy.Count);
            randomizedColors.Add(copy[removalIndex]);
            copy.RemoveAt(removalIndex);
        }

        return randomizedColors;
    }

    public List<Color> PointLightColors() {
        List<Color> copy = new List<Color>();
        foreach(Color c in pointLightColors) {
            copy.Add(c);
        }
        
        List<Color> randomizedColors = new List<Color>();
        for(int i=0; i<pointLightColors.Count; i++) {
            int removalIndex = Random.Range(0, copy.Count);
            randomizedColors.Add(copy[removalIndex]);
            copy.RemoveAt(removalIndex);
        }

        return randomizedColors;
    }

    public void SwitchLights() {
        showDirLights = !showDirLights;
        if(showDirLights) {
            directionalLightsObject.SetActive(true);
            pointLightObject.SetActive(false);
        } else {
            directionalLightsObject.SetActive(false);
            pointLightObject.SetActive(true);
        }
    }

    public void NewDirectionLightColors() {
        List<Color> newColors = DirectionLightColors();
        for(int i=0; i<directionalLights.Count; i++) {
            Light dirLight = directionalLights[i];
            dirLight.color = newColors[i];
        }
    }

    public void NewPointLightColors() {
        List<Color> newColors = PointLightColors();
        for(int i=0; i<pointLights.Count; i++) {
            Light pointLight = pointLights[i];
            pointLight.color = newColors[i];
        }
    }
}
