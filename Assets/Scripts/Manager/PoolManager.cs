using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance = null;

    [SerializeField] private Pool[] poolArray = null;

    private Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
    }

    public static PoolManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < poolArray.Length; i++)
        {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        GameObject parentGameObject = new GameObject(prefab.name + "Anchor");

        parentGameObject.transform.SetParent(transform);

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform);

                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObject);
            }
        }
    }

    public GameObject GetGameObejct(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(prefab.GetInstanceID()))
        {
            GameObject obj = GetGameobjectFromPool(prefab.GetInstanceID(), prefab);

            ResetObject(position, rotation, obj, prefab);

            return obj;
        }
        else
        {
            return null;
        }
    }

    private GameObject GetGameobjectFromPool(int poolKey, GameObject prefab)
    {
        GameObject obj = poolDictionary[poolKey].Dequeue();

        poolDictionary[poolKey].Enqueue(obj);

        if (obj.gameObject.activeSelf == true)
        {
            GameObject newObject = Instantiate(prefab, obj.transform.parent.transform);
            newObject.SetActive(false);
            obj = newObject;
            poolDictionary[poolKey].Enqueue(obj);
        }

        return obj;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, GameObject obj, GameObject prefab)
    {
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.transform.localScale = prefab.transform.localScale;
    }
}