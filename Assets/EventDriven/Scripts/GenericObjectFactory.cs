using UnityEngine;

public abstract class GenericObjectFactory<T> : MonoBehaviour where T : Object
{
    protected T prefab;

    public GenericObjectFactory(T prefabObject)
    {
        prefab = prefabObject;
    }

    public virtual T GetNewInstance()
    {
        return Instantiate(prefab);
    }
}