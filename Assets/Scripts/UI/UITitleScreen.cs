using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITitleScreen : MonoBehaviour
{
    /// <summary>
    /// This GameEvent will be invoked by Start Game's onClick
    /// </summary>
    [Tooltip("Triggered on Start Game button press")]
    public Events.GameEvent LoadSceneEvent;

    /// <summary>
    /// Invoke a GameEvent. 
    /// This function is called onClick
    /// </summary>
    public void ButtonPress_Start()
    {
        LoadSceneEvent?.Invoke();
    }

    /// <summary>
    /// Exits the application. On the Web platform, Application.Quit stops the Web Player but doesn't affect the web page front end.
    /// Mainly used for desktop builds.
    /// 
    /// This function is called onClick
    /// </summary>
    public void ButtonPress_Exit()
    {
        Application.Quit();
    }
}
