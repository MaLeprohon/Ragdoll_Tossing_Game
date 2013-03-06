#pragma strict
var crate:GameObject;
private var numCrates:float=50;
var instantiated:GameObject;
var lifeTime:int=20;
public var points : int = 0;
var cameraObj:GameObject;
var tries:int;
var myStyleBonus:GUIStyle;

function Start () {
	for (var i = 0; i < numCrates; i++)
	{
		var randomNum:float;
        randomNum=Random.Range(-25,-5);
        
        var randomNum2:float;
        randomNum2=Random.Range(-75,-200);
        
        var pos = Vector3 (GameObject.FindGameObjectWithTag("target").transform.position.x+randomNum, GameObject.FindGameObjectWithTag("target").transform.position.y+10, GameObject.FindGameObjectWithTag("target").transform.position.z);
        instantiated = Instantiate(crate, pos, Quaternion.identity);
     
        
        instantiated.rigidbody.mass=1;
        instantiated.rigidbody.AddForce(0,randomNum2,0);
        Destroy(instantiated, lifeTime);
    
       
        yield WaitForSeconds(2);
      }

}



function Update () {

	//instantiated.transform.Rotate(0,Time.deltaTime*100,0);
	

}

function OnGUI() 
{
	//GUI.Label(Rect(Screen.width/4-100,Screen.height-50,160,30),"Bonus Score: " + points, "box");
	
    myStyleBonus.fontSize = Mathf.RoundToInt(Screen.width * 0.02f);
    var content : GUIContent = new GUIContent("Crate bonus:\n" + points);
    var size : Vector2 = myStyleBonus.CalcSize(content);
	GUI.Label(new Rect(Screen.width*0.23f, Screen.height*0.83f, size.x*1.3f, size.y*1.6f), content, myStyleBonus);
}