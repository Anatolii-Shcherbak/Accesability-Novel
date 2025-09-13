using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Button muteButton;
    public Text buttonText;     // Reference to the Button Text
    private bool isMuted = false; // Track mute state

    void Start()
    {
        // Load mute state from PlayerPrefs (0 = not muted, 1 = muted)
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        AudioListener.volume = isMuted ? 0 : 1;

        // Update button text on start
        UpdateButtonText();

        // Add click event to button
        muteButton.onClick.AddListener(ToggleMute);
    }

    void ToggleMute()
    {
        isMuted = !isMuted; // Toggle state

        // Set global volume
        AudioListener.volume = isMuted ? 0 : 1;

        // Save mute state
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Update button text
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        buttonText.text = isMuted ? "Unmute" : "Mute";
    }
}
