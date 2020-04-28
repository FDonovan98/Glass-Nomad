using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class LoadoutManager : MonoBehaviour
{
    [SerializeField] private PlayersInLobby playersInLobby;
    [SerializeField] private TMP_Dropdown primaryDropdown = null;
    [SerializeField] private TMP_Dropdown secondaryDropdown = null;
    [SerializeField] private TMP_Dropdown armourDropdown = null;
    [SerializeField] private bool debug = false;

    private List<string> primaryItemsNames = new List<string>();
    private List<string> secondaryItemsNames = new List<string>();
    private List<string> armourItemsNames = new List<string>();

    private List<Weapon> primaryItems = new List<Weapon>();
    private List<Weapon> secondaryItems = new List<Weapon>();
    private List<Armour> armourItems = new List<Armour>();

    private void Start()
    {
        LoadAllItems();

        RemoveAlienSpecific();
        
        InitialiseDropdown(primaryDropdown, primaryItemsNames, "Primary");
        InitialiseDropdown(secondaryDropdown, secondaryItemsNames, "Secondary");
        InitialiseDropdown(armourDropdown, armourItemsNames, "Armour");

        // if (!PlayerPrefsExist())
        // {
            UpdatePlayerPrefs();
        // }
    }

    private void InitialiseDropdown(TMP_Dropdown dropdown, List<string> items, string prefString)
    {
        dropdown.AddOptions(items);
        if (debug) Debug.Log(items.Count + " items added to " + dropdown.name);

        if (PlayerPrefs.HasKey(prefString))
        {
            int index = items.IndexOf(PlayerPrefs.GetString(prefString));
            dropdown.SetValueWithoutNotify(index);
            if (debug) Debug.Log(prefString + " PlayerPref found: " + dropdown.captionText.text);
        }
    }

    private void LoadAllItems()
    {
        BaseObject[] baseObjects = Resources.LoadAll("Items", typeof(BaseObject)).Cast<BaseObject>().ToArray();
        AddItemsToLists(baseObjects);

        if (debug) Debug.Log(baseObjects.Length + " items found in the Resources/Items folder.");
    }

    private void AddItemsToLists(BaseObject[] items)
    {
        foreach (BaseObject obj in items)
        {
            switch (obj.itemType)
            {
                case BaseObject.ItemType.Primary:
                    primaryItemsNames.Add(obj.name);
                    primaryItems.Add((Weapon)obj);
                    break;

                case BaseObject.ItemType.Secondary:
                    secondaryItemsNames.Add(obj.name);
                    secondaryItems.Add((Weapon)obj);
                    break;

                case BaseObject.ItemType.Armour:
                    armourItemsNames.Add(obj.name);
                    armourItems.Add((Armour)obj);
                    break;

                default:
                    break;
            }
        }
    }

    public void UpdatePlayerPrefs()
    {
        SetPlayerPrefs(primaryDropdown, "Primary");
        SetPlayerPrefs(secondaryDropdown, "Secondary");
        SetPlayerPrefs(armourDropdown, "Armour");

        playersInLobby.localPlayer.primaryWeapon = primaryItems[primaryDropdown.value];
        playersInLobby.localPlayer.selectedArmour = armourItems[armourDropdown.value];

        if (debug) Debug.LogFormat("New PlayerPref values: {0}, {1}, {2}", primaryDropdown.captionText.text,
            secondaryDropdown.captionText.text, armourDropdown.captionText.text);
    }

    private void SetPlayerPrefs(TMP_Dropdown dropdown, string prefString)
    {
        PlayerPrefs.SetString(prefString, dropdown.captionText.text);
    }

    private bool PlayerPrefsExist()
    {
        string primary = PlayerPrefs.GetString("Primary", null);
        if (string.IsNullOrEmpty(primary))
        {
            return false;
        }
        return true;
    }

    private void RemoveAlienSpecific()
    {
        for (int i = 0; i < primaryItemsNames.Count; i++)
        {
            if (primaryItemsNames[i] == "Claws")
            {
                primaryItemsNames.RemoveAt(i);
                primaryItems.RemoveAt(i);
                break;
            }
        }
    }
}
