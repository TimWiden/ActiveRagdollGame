using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/// <summary>
/// Player name input field. Let the user input his name, will appear above the player in the game.
/// </summary>
[RequireComponent(typeof(TMP_InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    const string playerNamePref = "PlayerNme";

    /// <summary>
    /// Sets the text field to whatever existing value exists. 
    /// </summary>
    private void Start()
    {
        string defaultName = string.Empty;

        TMP_InputField inputField = GetComponent<TMP_InputField>();

        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePref))
            {
                defaultName = PlayerPrefs.GetString(playerNamePref);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    /// <summary>
    /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
    /// </summary>
    public void SetPlayerName(string name)
    {
        // If the input is empty still
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        PhotonNetwork.NickName = name;

        // Saves the photon playerpref name key as the new input
        PlayerPrefs.SetString(playerNamePref, name);
    }
}
