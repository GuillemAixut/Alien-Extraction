using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Button_Navigation : YmirComponent
{
    public string sceneName = "BASE_FINAL/LVL_BASE_COLLIDERS";

    public bool isNewGame = true;

    // Loading scene
    private GameObject loadSceneImg;
    private bool loadScene = false;

    public void Start()
    {
        loadSceneImg = InternalCalls.GetGameObjectByName("Load Scene Img");

        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(false);
        }

        loadScene = false;

        if (!SaveLoad.GameFileExists(Globals.saveGameDir, Globals.saveGamesInfoFile))
        {
            SaveLoad.CreateSaveGameFile(Globals.saveGameDir, Globals.saveGamesInfoFile);
            SaveLoad.SaveString(Globals.saveGameDir, Globals.saveGamesInfoFile, Globals.saveCurrentGame, "Player_0");
        }
    }

    public void Update()
    {
        if (loadScene)
        {
            InternalCalls.LoadScene("Assets/" + sceneName + ".yscene");
            loadScene = false;

            return;
        }
    }

    public void OnClickButton()
    {
        Debug.Log("Go to scene " + sceneName + ".yscene");
        Audio.PauseAllAudios();

        if (isNewGame)
        {
            SaveNewEmptyGame();
        }
        else
        {

        }

        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(true);
        }

        loadScene = true;
    }

    public void SaveNewEmptyGame()
    {
        int i = 0;
        while (SaveLoad.GameFileExists(Globals.saveGameDir, "Player_" + i.ToString()))
        {
            i++;
        }

        string fileName = "Player_" + i.ToString();

        SaveLoad.SaveString(Globals.saveGameDir, Globals.saveGamesInfoFile, Globals.saveCurrentGame, fileName);

        // Player
        SaveLoad.CreateSaveGameFile(Globals.saveGameDir, fileName);
        SaveLoad.SaveInt(Globals.saveGameDir, fileName, "Last unlocked Lvl", 0);

        SaveLoad.SaveInt(Globals.saveGameDir, fileName, "Current weapon", 0);
        SaveLoad.SaveInt(Globals.saveGameDir, fileName, "Weapon upgrade", 0);

        SaveLoad.SaveFloat(Globals.saveGameDir, fileName, "Health", 1200);

        SaveLoad.SaveInt(Globals.saveGameDir, fileName, "Items num", 0);
    }
}