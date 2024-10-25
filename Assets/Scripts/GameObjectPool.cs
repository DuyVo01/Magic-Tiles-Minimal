using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private Queue<GameObject> pool;
    private GameObject prefab;
    private Transform parent;
    private int maxSize;

    public GameObjectPool(GameObject prefab, int maxSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.maxSize = maxSize;
        pool = new Queue<GameObject>(maxSize);
        this.parent = parent;

        for (int i = 0; i < maxSize; i++)
        {
            CreateNewGameObject();
        }
    }

    public GameObject Get()
    {
        GameObject gameObject = pool.Count > 0 ? pool.Dequeue() : CreateNewGameObject();
        gameObject.SetActive(true);
        return gameObject;
    }

    public void Return(GameObject gameObject)
    {
        if (pool.Count > maxSize)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }

    public void ClearPool()
    {
        pool.Clear();
    }

    private GameObject CreateNewGameObject()
    {
        GameObject gameObject = GameObject.Instantiate(prefab, parent);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
        return gameObject;
    }


}
