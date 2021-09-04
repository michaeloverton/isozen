using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetMenu : MonoBehaviour
{
    public LightController lightController;
    

    public void ReturnToMainMenu() {
        GameObject.Destroy(GameObject.Find("Complexity"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SwitchLights() {
        lightController.SwitchLights();
    }

    public void NewDirectionLightColors() {
        lightController.NewDirectionLightColors();
        lightController.NewPointLightColors();
    }
}
