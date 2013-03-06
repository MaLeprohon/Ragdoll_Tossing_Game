using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	private GameObject cannonBall;
	public float distance=10;
	private Vector3 newpos;
	// Use this for initialization
	void Start () {
		cannonBall = GameObject.FindGameObjectWithTag("cannonBall");
		newpos = cannonBall.transform.position;
		newpos.z -= distance; // why does this work while 'transform.position.x += 5.0f;' doesn't?
		transform.position = newpos;
	}
	
	// Update is called once per frame
	void Update () {
		newpos.x = cannonBall.transform.position.x;
		newpos.y = cannonBall.transform.position.y;
		transform.position = newpos;
	}
}
