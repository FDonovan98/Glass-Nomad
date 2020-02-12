using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadoutManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown primaryDropdown;
    [SerializeField]
    private TMP_Dropdown secondaryDropdown;
    [SerializeField]
    private TMP_Dropdown armourDropdown;

    private List<string> primaryItems = new List<string>();
    private List<string> secondaryItems = new List<string>();
    private List<string> armourItems = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        BaseObject item;
        for (int i = 0; i < ItemDatabase.allItems.GetItemCount(); i++)
        {
            item = ItemDatabase.allItems.GetItem(i);
            switch (item.itemType)
            {
                case BaseObject.ItemType.Primary:
                    primaryItems.Add(item.name);
                    break;
                case BaseObject.ItemType.Secondary:
                    secondaryItems.Add(item.name);
                    break;
                case BaseObject.ItemType.Armour:
                    armourItems.Add(item.name);
                    break;
                default:
                    break;
            }
        }

        primaryDropdown.AddOptions(primaryItems);
        secondaryDropdown.AddOptions(secondaryItems);
        armourDropdown.AddOptions(armourItems);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
