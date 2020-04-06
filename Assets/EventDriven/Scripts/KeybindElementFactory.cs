using UnityEngine;

public class KeybindElementFactory : GenericObjectFactory<GameObject>
{
    private Transform parent;
    public KeybindElementFactory(GameObject prefabObject, Transform menu) : base(prefabObject) 
    {
        parent = menu;
    }

    public override GameObject GetNewInstance()
    {
        return Instantiate(prefab, parent);
    }
}