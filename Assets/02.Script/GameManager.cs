using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static public GameManager instance;

    private Vector3 WindDir= new Vector3(0, 0, 0);

    void Awake()
    {
        instance = this;
    }

	void Start () {
        SetWindDirection();
	}

	void Update () {
		
	}

    private void SetWindDirection()
    {
        float x = Random.Range(1f, 1.3f);
        //float y = Random.Range(1f, 1.3f);
        float z = Random.Range(1f, 1.3f);

        WindDir = new Vector3(x, 0, z);

        Debug.Log("Wind Direction : " + WindDir);
    }

    public Vector3 GetWindDirection()
    {
        return WindDir;
    }
}
