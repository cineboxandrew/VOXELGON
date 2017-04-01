﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

	private Camera _camera;

	// Use this for initialization
	void Start () {
		_camera = Camera.main;
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = _camera.transform.position;
		
	}
}
