#pragma strict
var explosion: Transform;
var target : Collider;
var testtext:GameObject;
private var bc : BonusCrate;
var pointsFromCrate:int;
var tempCam:GameObject;

function Start () 
{
    bc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent(typeof(BonusCrate));
}

function Update () 
{
}



function OnTriggerEnter(collision : Collider) 
{	
	if(collision.tag == "missile")
	{
		Instantiate(explosion, transform.position, Quaternion.identity);
		var textInstant = Instantiate(testtext, transform.position, Quaternion.identity);
		textInstant.rigidbody.AddForce(0,100,0);
		Destroy(textInstant, 2);
		Destroy(gameObject);
		bc.points += 1000;
		
		tempCam = GameObject.FindGameObjectWithTag("MainCamera");
		pointsFromCrate = tempCam.GetComponent(BonusCrate).points;
		
		var totalpoints = PlayerPrefs.GetInt("Player Score");
		totalpoints+=pointsFromCrate;
		PlayerPrefs.SetInt("Player Score", totalpoints);
	}
}
