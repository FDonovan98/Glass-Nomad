using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField]
    private List<BaseObject> itemDatabase = new List<BaseObject>();

    public static ItemDatabase allItems;

    private void Awake()
    {
        if (allItems == null)
        {
            allItems = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public BaseObject GetItem(int id)
    {
        if (id < itemDatabase.Count)
        {
            return itemDatabase[id];
        }
        return null;
    }

    public int GetItemCount()
    {
        return itemDatabase.Count;
    }
}
