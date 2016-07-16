using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Spawner : MonoBehaviour {
    public string PoolName;
    public GameObject SpawnObj;
    public float Delay; //延迟多长时间产生第一个物体
    public float DistanceToPlayer;
    public float Distance; //两个物体产生的距离
    public float MinY; 
    public float MaxY;

    private GameObject LastSpawn;
    private float Timer = 0;
    private SpawnPool _SpawnPool;
    private PrefabPool _PrefabPool;

	// Use this for initialization
	void Start () {
        _SpawnPool = PoolManager.Pools[PoolName];
        _PrefabPool = new PrefabPool(SpawnObj.transform);

        InitPool();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Delay > Timer)
        {
            Timer += Time.deltaTime;
            return;
        }

        if (LastSpawn == null)
            Spawn();
        else
        {
            float dist = SpawnX() - LastSpawn.transform.position.x;
            if (dist > Distance)
                Spawn();
        }
	}

    void Spawn()
    {
        GameObject obj = _SpawnPool.Spawn("Obstacle").gameObject;//GameObject.Instantiate(SpawnObj);
        float y = Random.Range(MinY, MaxY);
        obj.transform.position = new Vector3(SpawnX(), y, 0);
        obj.transform.parent = this.transform;
        LastSpawn = obj;
    }

    float SpawnX()
    {
        GameObject player = GameObject.Find("Player");
        return player.transform.position.x + DistanceToPlayer;
    }

    void InitPool()
    {
        if(!_SpawnPool._perPrefabPoolOptions.Contains(_PrefabPool))
        {
            _PrefabPool = new PrefabPool(SpawnObj.transform);
            _PrefabPool.preloadAmount = 3;
            _PrefabPool.limitInstances = false;
            _PrefabPool.limitFIFO = true;
            _PrefabPool.limitAmount = 7;
            _SpawnPool._perPrefabPoolOptions.Add(_PrefabPool);
            _SpawnPool.CreatePrefabPool(_SpawnPool._perPrefabPoolOptions[_SpawnPool.Count]);
        }
    }
}
