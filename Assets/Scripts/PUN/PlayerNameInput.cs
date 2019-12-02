using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

using System.Collections;

[RequireComponent(typeof(InputField))]
public class PlayerNameInput : MonoBehaviour
{
    private const string playerNamePrefKey = "Player Name";

    void Start()
    {
        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();

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
