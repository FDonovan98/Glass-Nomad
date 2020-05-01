using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class LoadoutManager : MonoBehaviour
{
    [SerializeField] private PlayersInLobby playersInLobby;
    [SerializeField] private TMP_Dropdown primaryDropdown = null;
    [SerializeField] private TMP_Dropdown armourDropdown = null;
    [SerializeField] private TMP_Dropdown materialDropdown = null;
    [SerializeField] private bool debug = false;

    private List<string> primaryItemsNames = new List<string>();
    private List<string> armourItemsNames = new List<string>();
    
    private List<string> materialItemsNames = new List<string>();
    private List<Material> materialItems = new List<Material>();

    private List<Weapon> primaryItems = new List<Weapon>();
    private List<Armour> armourItems = new List<Armour>();

    private void Start()
    {
        LoadAllItems();

        RemoveAlienSpecific();
        
        InitialiseDropdown(primaryDropdown, primaryItemsNames, "Primary");
        InitialiseDropdown(armourDropdown, armourItemsNames, "Armour");
        InitialiseDropdown(materialDropdown, materialItemsNames, "Colour");

        UpdatePlayerPrefs();
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

        Material[] materials = Resources.LoadAll("Items", typeof(Material)).Cast<Material>().ToArray();
        AddMaterialsToLists(materials);

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

                case BaseObject.ItemType.Armour:
                    armourItemsNames.Add(obj.name);
                    armourItems.Add((Armour)obj);
                    break;

                default:
                    break;
            }
        }
    }

    void AddMaterialsToLists(Material[] materials)
    {
        foreach (Material element in materials)
        {
            materialItemsNames.Add(element.name);
            materialItems.Add(element);
        }
    }

    public void UpdatePlayerPrefs()
    {
        SetPlayerPrefs(primaryDropdown, "Primary");
        SetPlayerPrefs(armourDropdown, "Armour");
        
        SetPlayerPrefs(materialDropdown, "Material");

        playersInLobby.localPlayer.primaryWeapon = primaryItems[primaryDropdown.value];
        playersInLobby.localPlayer.selectedArmour = armourItems[armourDropdown.value];

        playersInLobby.localPlayer.selectedMaterial = materialItems[materialDropdown.value];
        playersInLobby.localPlayer.selectedMaterialIndex = materialDropdown.value;

        if (debug) Debug.LogFormat("New PlayerPref values: {0}, {1}", primaryDropdown.captionText.text, armourDropdown.captionText.text);
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
