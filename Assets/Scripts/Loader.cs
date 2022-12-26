using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene {
        GameScene,
        MainMenu,
        Loading,
    }

    private static Action loaderCallbackAction;

    public static void Load(Scene scene)
    {
        // Wait for update
        // Set up callback action that will be triggered after the loading scene is loaded
        loaderCallbackAction = () => {
            // load target scene when the loading scene is loaded
            SceneManager.LoadScene(scene.ToString());
        };

        // Load loading scene
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        if (loaderCallbackAction != null) {
            loaderCallbackAction();
            loaderCallbackAction = null;
        }
    }
}
