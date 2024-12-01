using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

/// <summary>
/// Controls the escape menu.
/// 
/// NOTE: mouse cursor does not need to be locked on destroy since everything gets
///       loaded into a fresh state.
/// </summary>
public class UIEscapeMenu : MonoBehaviour
{
    /// <summary>
    /// The panel that will be enabled/disabled
    /// This can be any panel including null.
    /// </summary>
    [Tooltip("The panel that will be enabled/disabled")]
    public GameObject EscapeMenuPanel;

    /// <summary>
    /// This GameEvent will be invoked by Start Game's onClick
    /// </summary>
    [Tooltip("Triggered on Quit button press")]
    public Events.GameEvent LoadSceneEvent;

    /// <summary>
    /// Keep track of the visibility of the escape menu
    /// </summary>
    private bool _showing;

    /// <summary>
    /// Check escape key input in update
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_showing)
                Hide();
            else
                Show();
        }
    }

    private void Awake()
    {
        _showing = false; 
    }

    /// <summary>
    /// Show the escape menu (if exists)
    /// Unlock the mouse cursor and disable player movement
    /// </summary>
    public void Show()
    {
        if(EscapeMenuPanel != null)
        {
            EscapeMenuPanel.SetActive(true);
            PlayerManager.Instance.PlayerController.MovementEnabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _showing = true;
    }

    /// <summary>
    /// Hide the escape menu (if exists)
    /// Lock the mouse cursor and enable player movement
    /// </summary>
    public void Hide()
    {
        if (EscapeMenuPanel != null)
        {
            EscapeMenuPanel.SetActive(false);
            PlayerManager.Instance.PlayerController.MovementEnabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _showing = false;
    }

    /// <summary>
    /// Function assigned to a UI Button component.
    /// Invokes a GameEvent
    /// </summary>
    public void ButtonPress_Quit()
    {
        LoadSceneEvent?.Invoke();
    }
}
