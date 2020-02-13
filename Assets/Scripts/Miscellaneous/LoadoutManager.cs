using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class LoadoutManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown primaryDropdown = null;
    [SerializeField] private TMP_Dropdown secondaryDropdown = null;
    [SerializeField] private TMP_Dropdown armourDropdown = null;
    [SerializeField] private bool debug = false;

    private List<string> primaryItems = new List<string>();
    private List<string> secondaryItems = new List<string>();
    private List<string> armourItems = new List<string>();

    private void Start()
    {
        LoadAllItems();
        
        InitialiseDropdown(primaryDropdown, primaryItems, "Primary");
        InitialiseDropdown(secondaryDropdown, secondaryItems, "Secondary");
        InitialiseDropdown(armourDropdown, armourItems, "Armour");
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
                    primaryItems.Add(obj.name);
                    break;

                case BaseObject.ItemType.Secondary:
                    secondaryItems.Add(obj.name);
                    break;

                case BaseObject.ItemType.Armour:
                    armourItems.Add(obj.name);
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

        if (debug) Debug.LogFormat("New PlayerPref values: {0}, {1}, {2}", primaryDropdown.captionText.text,
            secondaryDropdown.captionText.text, armourDropdown.captionText.text);
    }

    private void SetPlayerPrefs(TMP_Dropdown dropdown, string prefString)
    {
        PlayerPrefs.SetString(prefString, dropdown.captionText.text);
    }
}
