using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ruler
{
    public Color color;
    public Vector3 vector;
    public Vector3 pos;
    //public float speed;
    public float vx, vy, vz;
    public ruler(Color color, Vector3 vector,Vector3 pos)
    {
        this.color = color;
        this.vector = vector;
        this.pos = pos;
        //this.speed = speed;
    }
}

public class DiskData : MonoBehaviour {

    public Vector3 pos;
    public Vector3 direction;
    public float speed;
    public Color color;

    public void setColor(Color color)
    {
        this.color = color;
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public DiskData(Vector3 pos,Vector3 direction,float speed,Color color)
    {
        this.pos = pos;
        this.direction = direction;
        this.speed = speed;

        this.color = color;
        gameObject.GetComponent<Renderer>().material.color = color;
    }
	// Use this for initialization
	void Start () {


    }

    // Update is called once per frame
    void Update () {
		
	}
}
