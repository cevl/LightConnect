using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public void ChangeScene(int sceneIndex)
//string scene is the index of the scene that we want to load.
    {
        print("Changing Scene");
        SceneManager.LoadScene(sceneIndex);
        ResetStaticValues();
    }

    public void NextScene()
    {
        print("Going to the next Scene.");
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        } else
        {
            print("Not next scene, sceneIndex > sceneCount - 1: buildIndex - sceneCount = " + (SceneManager.GetActiveScene().buildIndex - SceneManager.sceneCountInBuildSettings).ToString());
        }
        ResetStaticValues();
    }

    public void PreviousScene()
    {
        print("Going to the previous Scene.");
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex > 0)
        {
            SceneManager.LoadScene(sceneIndex - 1);
        } else
        {
            print("Not previous scene, sceneIndex < 0: " + SceneManager.GetActiveScene().buildIndex.ToString());
        }
        ResetStaticValues();
    }

    public void ReloadScene()
    {
        print("Reloading the Scene.");
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
        ResetStaticValues();
    }

    public void MenuScene()
    {
        print("Going to the menu Scene.");
        int sceneIndex = SceneManager.GetSceneByName("Menu2").buildIndex;
        //SceneManager.LoadScene(sceneIndex);
        SceneManager.LoadScene(0);
        ResetStaticValues();
    }

    private void ResetStaticValues()
    {
        //Restarting static values to avoid problems after a scene change.
        Block.fonts = new List<GameObject>();
        Block.receivers = new List<GameObject>();
    }
}
