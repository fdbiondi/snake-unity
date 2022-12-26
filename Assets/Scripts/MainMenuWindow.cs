using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour
{
    private enum SubMenu {
        Main,
        HowToPlay
    }

    private const string MAIN_SUB = "mainSubMenu";
    private const string HOWTO_SUB = "howToPlaySubMenu";


    private void Awake() {
        transform.Find(MAIN_SUB).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.Find(HOWTO_SUB).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        transform.Find(MAIN_SUB).Find("playBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.GameScene);
            Time.timeScale = 1f;
        };
        
        transform.Find(MAIN_SUB).Find("exitBtn").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();

        transform.Find(MAIN_SUB).Find("howtoplayBtn").GetComponent<Button_UI>().ClickFunc = () => ShowSubMenu(SubMenu.HowToPlay);
        
        transform.Find(HOWTO_SUB).Find("backBtn").GetComponent<Button_UI>().ClickFunc = () => ShowSubMenu(SubMenu.Main);

        ShowSubMenu(SubMenu.Main);
    }


    private void ShowSubMenu(SubMenu submenu)
    {
        transform.Find(MAIN_SUB).gameObject.SetActive(false);
        transform.Find(HOWTO_SUB).gameObject.SetActive(false);

        switch (submenu) {
            case SubMenu.Main:
            transform.Find(MAIN_SUB).gameObject.SetActive(true);
            break;
            case SubMenu.HowToPlay:
            transform.Find(HOWTO_SUB).gameObject.SetActive(true);
            break;
        
        }
    }
}
