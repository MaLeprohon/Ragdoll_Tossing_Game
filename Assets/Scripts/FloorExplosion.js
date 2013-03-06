#pragma strict
var explosionFloor:Transform;
var contact : ContactPoint;

function Start () {
	renderer.enabled=false;
}

function Update () {

}

function OnTriggerEnter(collision : Collider)
{
	if(collision.tag=="missile")
	{
		Instantiate(explosionFloor, collision.transform.position, Quaternion.identity);
		audio.pitch = Random.Range(0.9f, 1.1f);
		audio.Play();
		Destroy (collision.gameObject);
		
	}
}
