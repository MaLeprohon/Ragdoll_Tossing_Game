#pragma strict
var degrees:float;
var levelReached : int = 0;
levelReached=PlayerPrefs.GetInt("SavedLevel");
var level1:Texture2D;
var locked2: Texture2D;
var unlocked2: Texture2D;
var locked3: Texture2D;
var unlocked3: Texture2D;
var locked4:Texture2D;
var unlocked4:Texture2D;
var cube1 : GameObject;
var cube2 : GameObject;
var cube3 : GameObject;
var cube4 : GameObject;


function Start () {
	//PlayerPrefs.DeleteAll();
	switch(levelReached)
		{
		case 0:
		 
			
			cube1.renderer.material.mainTexture=level1;
			
			
			cube2.renderer.material.mainTexture=locked2;
			
			
			cube3.renderer.material.mainTexture=locked3;
			
			
			cube4.renderer.material.mainTexture=locked4;
			
			break;
		
		case 2:
		
			cube1.renderer.material.mainTexture=level1;
			
			
			cube2.renderer.material.mainTexture=unlocked2;
			
		
			cube3.renderer.material.mainTexture=locked3;
			
			
			cube4.renderer.material.mainTexture=locked4;
			
	
			break;
		
		
		case 3:
			
			cube1.renderer.material.mainTexture=level1;
			
	
			cube2.renderer.material.mainTexture=unlocked2;
			
		
			cube3.renderer.material.mainTexture=unlocked3;
			
			
			cube4.renderer.material.mainTexture=locked4;
			
			break;
	
		
		case 4:
		case 5:
			
			cube1.renderer.material.mainTexture=level1;
			
			
			cube2.renderer.material.mainTexture=unlocked2;
			
			
			cube3.renderer.material.mainTexture=unlocked3;
			
			
			cube4.renderer.material.mainTexture=unlocked4;
			
			break;
			
			
	}
}

function Update () {

	//transform.RotateAround(Vector3.zero, Vector3.up,50*Time.deltaTime);
   if(Input.GetMouseButtonDown(0) && !(degrees > 0))
   {
     degrees = 90;
   }
   if(degrees > 0)
   {
	  var deg : float = 60 * Time.deltaTime;
	  if (degrees - deg < 0) deg = degrees;
      transform.RotateAround(Vector3.zero, Vector3.up, deg);
      degrees -= deg;
   }
   
   
   if(Input.GetKeyDown(KeyCode.Escape))
   {
   	Application.LoadLevel(0);
   }
}


function OnMouseDown()
{
		switch(levelReached)
	{
		case 0:
		if(name=="Cube1")
		{
			Application.LoadLevel(2);
		}
		
		break;
	
	case 2:
		if(name=="Cube1")
		{
			Application.LoadLevel(2);
		}
		
		if(name=="Cube2")
		{
			Application.LoadLevel(3);
		}

		break;
	
	
	case 3:
		if(name=="Cube1")
		{
			Application.LoadLevel(2);
		}
		
		if(name=="Cube2")
		{
			Application.LoadLevel(3);
		}
		
		if(name=="Cube3")
		{
			Application.LoadLevel(4);
		}
	
		break;

    case 4:
	case 5:
		if(name=="Cube1")
		{
			Application.LoadLevel(2);
		}
		
		if(name=="Cube2")
		{
			Application.LoadLevel(3);
		}
		
		if(name=="Cube3")
		{
			Application.LoadLevel(4);
		}
		if(name=="Cube4")
		{
			Application.LoadLevel(5);
		}
	
		break;


	}
		
		
}
