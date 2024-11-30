using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneManagement;
using UI;
using World;

public class UITitleScreen : MonoBehaviour
{
    /// <summary>
    /// This GameEvent will be invoked by Start Game's onClick
    /// </summary>
    [Tooltip("Triggered on Start Game button press")]
    public Events.GameEvent LoadSceneEvent;

    private void Start()
    {
        UIManager.Instance.OnNodeTransitionEnd += EnableCursor;
    }

    private void OnDestroy()
    {
        UIManager.Instance.OnNodeTransitionEnd -= EnableCursor;
    }

    void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Invoke a GameEvent. 
    /// This function is called onClick
    /// </summary>
    public void ButtonPress_Start()
    {
        WorldManager.Instance?.ResetGameState();
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
