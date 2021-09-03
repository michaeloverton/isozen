using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetMenu : MonoBehaviour
{
    public GameObject directionalLights;
    public GameObject pointLightGroup;
    bool showDirLights = false;
    public void ReturnToMainMenu() {
        GameObject.Destroy(GameObject.Find("Complexity"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SwitchLights() {
        showDirLights = !showDirLights;
        if(showDirLights) {
            directionalLights.SetActive(true);
            pointLightGroup.SetActive(false);
        } else {
            directionalLights.SetActive(false);
            pointLightGroup.SetActive(true);
        }
    }
}
