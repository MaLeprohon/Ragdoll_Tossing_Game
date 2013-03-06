#pragma strict
var cannonBallPrefab:Transform;
var cannonBallSpawn:Transform;
var Angle:float=45;
var Force:float=2000;
var Mass:float=1;
var clip:AudioClip;
var lifetime:float=3;


function Start () {
for(var i=0;i<20;i++)
	{
	Shoot();
	yield WaitForSeconds(3);
	}
}

function Update () {


}


function Shoot() 
	{	
	// if (cannonBallPrefab != null && cannonBallSpawn != null)
       // {
        var cannonBall =  Instantiate(cannonBallPrefab, cannonBallSpawn.transform.position, cannonBallSpawn.transform.rotation) as Transform;
       // }
        
        
      	Physics.gravity = new Vector3(0, -9.8, 0);
        cannonBall.rigidbody.angularDrag = 0.05f;
        cannonBall.rigidbody.isKinematic = false;
        cannonBall.rigidbody.mass = Mass;
        cannonBall.rigidbody.useGravity = true;

      	cannonBall.rigidbody.AddForce(Force * Mathf.Cos(Angle * Mathf.Deg2Rad), Force * Mathf.Sin(Angle * Mathf.Deg2Rad), 0);

        //cannonBall.rigidbody.AddForce(100,100,0);
        audio.PlayOneShot(clip);
        Destroy((cannonBall as Transform).gameObject, lifetime);
        //cannonBall.collider.enabled = true;
	}
