using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldVersion
{
    public class ObjectPoolOld : MonoBehaviour
    {
        List<GameObject>[] pooledObjects;
        //     0            1           2           3
        //projectile    plyhiteff   plyhealeff  enmhiteff
        public GameObject[] objectToPool;
        public int[] amountToPool;
        Transform pooledContainer;

        void Start()
        {
            pooledContainer = GameObject.FindGameObjectWithTag("PooledContainer").transform;
            pooledObjects = new List<GameObject>[objectToPool.Length];
            for (int i = 0; i < pooledObjects.Length; i++)
            {
                //create pool object
                pooledObjects[i] = new List<GameObject>();
                GameObject tmp;
                for (int j = 0; j < amountToPool[i]; j++)
                {
                    //create temp of objectpool
                    tmp = Instantiate(objectToPool[i], pooledContainer);
                    tmp.SetActive(false);
                    pooledObjects[i].Add(tmp);
                }
            }
        }

        ///get pool object method
        public GameObject GetPooledObject(int num)
        {
            for (int i = 0; i < amountToPool[num]; i++)
            {
                //if pooledobject unactive will return
                if (!pooledObjects[num][i].activeInHierarchy)
                {
                    return pooledObjects[num][i];
                }
            }
            return null;
        }
    }
}
