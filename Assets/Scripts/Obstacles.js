#pragma strict
var prefab : GameObject;
var prefab2 : GameObject;
var numberOfObjects = 1;
var randomNum : int;
var instantiated : GameObject;
var instantiated2 : GameObject;
var instantiatedMove:boolean=false;
var instantiated2Move:boolean=false;

function Start () {
  for (var i = 0; i < numberOfObjects; i++) {
  		randomNum=Random.Range(-25,-5);
        var pos = Vector3 (GameObject.FindGameObjectWithTag("target").transform.position.x-5, GameObject.FindGameObjectWithTag("target").transform.position.y, GameObject.FindGameObjectWithTag("target").transform.position.z);
        var pos2 = Vector3 (GameObject.FindGameObjectWithTag("target").transform.position.x-20, GameObject.FindGameObjectWithTag("target").transform.position.y, GameObject.FindGameObjectWithTag("target").transform.position.z);
        instantiated = Instantiate(prefab, pos, Quaternion.identity);
        instantiated2 = Instantiate(prefab2, pos2, Quaternion.identity);

  }

}

function Update () {
  
    var speed : float=5;
	
	if(instantiated.transform.position.y>3.75 && instantiatedMove==false)
	{
    	instantiated.transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    else if (instantiatedMove==false){instantiatedMove=true;}
    
 
    if(instantiated.transform.position.y<22 && instantiatedMove==true)
    {
    	instantiated.transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    else if (instantiatedMove==true){instantiatedMove=false;}

 	
    
    if(instantiated2.transform.position.y<22 && instantiated2Move==false)
    {
		instantiated2.transform.Translate(Vector3.up * speed * Time.deltaTime);
	}
	else if (instantiated2Move==false){instantiated2Move=true;}
	
	
	if(instantiated2.transform.position.y>3.75 && instantiated2Move==true)
    {
    	instantiated2.transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
	else if (instantiated2Move==true){instantiated2Move=false;}
        
}