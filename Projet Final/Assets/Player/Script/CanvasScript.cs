﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Networking;

public class CanvasScript : NetworkBehaviour 
{
// Use this for initialization
	public Canvas canvas;
	public Canvas Score;

	private void Start()
	{
		if (!isLocalPlayer)
		{
			canvas.enabled = false;
			Score.enabled = false;
		}
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			canvas.enabled = false;
			Score.enabled = false;
		}
	}
	
}
