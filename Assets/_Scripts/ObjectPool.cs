using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance;
        public GameObject prefab;
        public int poolSize;
        public List<GameObject> pool;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            pool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        public GameObject GetObjectFromPool()
        {
            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            // If all objects are active, create a new one
            GameObject newObj = Instantiate(prefab);
            pool.Add(newObj);
            return newObj;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
    }