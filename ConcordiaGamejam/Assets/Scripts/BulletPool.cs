using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    public static BulletPool pool;

    public List<GameObject> bullets;
    public GameObject prefab;
    public int poolSize;
    private int curr = 0;

    // Use this for initialization
    void Start()
    {
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

        if (curr == bullets.Count) {
            curr = 0;
        }

        GameObject go = bullets[curr];
        curr++;
        return go;

    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
