using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCamCtrlBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        if (Player.instance.GetPlayerState() == Player.EPlayerState.Prepare)
        {
            Player.instance.ChangeState(Player.EPlayerState.Shooting);
        }
        else
        {
            Player.instance.ChangeState(Player.EPlayerState.Prepare);
        }
    }
}


