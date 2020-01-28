using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    private const string playerNamePrefKey = "Player Name";

    void Start()
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
