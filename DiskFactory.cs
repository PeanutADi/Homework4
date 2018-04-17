using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    public DiskData prefab;
    List<DiskData> used = new List<DiskData>();
    List<DiskData> wait = new List<DiskData>();

    public DiskData GetDisk(Vector3 pos, Vector3 direction, float speed,Color color)
    {
        DiskData instance;
        if (wait.Count > 0)
        {
            instance = wait[0];
            wait.RemoveAt(0);
        }
        else instance = Instantiate(prefab);
        instance.pos = pos;
        instance.direction = direction;
        instance.speed = speed;
        instance.setColor(color);
        used.Add(instance);
        return instance;
    }

    public void DiskWait(DiskData disk)
    {
        wait.Add(disk);
        if (!used.Remove(disk))
        {
            throw new System.Exception();
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
