using System.Collections.Generic;
using UnityEngine;

public class PooledObjects : MonoBehaviour
{
    public static PooledObjects Instance; 

    [Header("Pengaturan Pool")]
    public GameObject objectToPool;
    
    private List<GameObject> pooledObjects = new List<GameObject>();

    void Awake() 
    {
        if (Instance == null) {
            Instance = this;
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // ✅ Cek null dulu sebelum akses activeInHierarchy
            if (pooledObjects[i] == null)
            {
                pooledObjects.RemoveAt(i);
                i--;
                continue;
            }

            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i]; 
            }
        }

        GameObject obj = Instantiate(objectToPool);
        obj.SetActive(false); 
        pooledObjects.Add(obj); 
        
        return obj; 
    }
}