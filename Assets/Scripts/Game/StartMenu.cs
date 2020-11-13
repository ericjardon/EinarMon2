using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameLoader loader;
    public Camera mapCamera;

    public GameManager manager;

    // Start is called before the first frame update

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        manager.state = GameState.Roaming;  // solo se está llamando 1 vez
        MainMenu.SetActive(false);
        Debug.Log("Pressed Play");
        mapCamera.gameObject.SetActive(true);
    }

    public void LoadGameButton(){
        manager.state = GameState.Roaming;
        MainMenu.SetActive(false);
        loader.Load();
        mapCamera.gameObject.SetActive(true);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        //MainMenu.SetActive(true);
        return;
    }
}
