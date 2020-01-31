using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    // The string used to set the player's nickname.
    private const string playerNamePrefKey = "Player Name";

    /// <summary>
    /// Initialises the input field with the player's current player
    /// pref nickname, if they have one, otherwise it is initialised
    /// as empty.
    /// </summary>
    private void Start()
    {
        TMP_InputField inputField = GetComponent<TMP_InputField>();
        string defaultName = string.Empty;

        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    /// <summary>
    /// Used by the Lobby manager to set a player's nickname.
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Player name is null or empty");
            return;
        }

        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString(playerNamePrefKey, name);
    }
}
