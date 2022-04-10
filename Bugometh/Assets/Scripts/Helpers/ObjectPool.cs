using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helps storing disabled game objects not to create/destory them every time
public class ObjectPool : Singleton<ObjectPool>
{
    // list og prefabs to be collected
    public List<GameObject> prefabsForPool;

    // list of collected objects
    private List<GameObject> ObjectsPool = new List<GameObject>();

    public GameObject GetObjectFromPool(string objectName)
    {
        var instance = ObjectsPool.Find(obj => obj.name == objectName);

        if (instance != null)
        {
            ObjectsPool.Remove(instance);
            instance.SetActive(true);
            return instance;
        }
        var prefab = prefabsForPool.Find(obj => obj.name == objectName);
        if (prefab == null)
        {
            Debug.LogWarning($"Trying to get inexistent object from collection. ObjectId: {objectName}");
            return null;
        }
        var newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        newObject.name = objectName;
        return newObject;
    }

    public void AddObjectToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        ObjectsPool.Add(gameObject);

    }
}
