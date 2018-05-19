using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    public static BulletPool pool;

    public List<GameObject> bullets;
    public GameObject prefab;
    public int poolSize;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Starting");
        if (pool == null)
        {
            pool = this;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(prefab);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
	}

    public GameObject GetBullet()
    {
        Debug.Log("retrieving");
        for(int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
        }

        return null;
    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
