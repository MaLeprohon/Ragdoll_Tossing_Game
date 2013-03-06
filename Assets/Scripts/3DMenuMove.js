#pragma strict
var clip2:AudioClip;
var mouseOverTexture:Texture2D;
var mouseOverTextureExit:Texture2D;
var mouseOverTextureMaterial:Material;
var mouseOverTextureMaterialExit:Material;

function Start () {

}

function Update () {
//transform.position=startPos;

}

function OnMouseDown()
{
		if(name=="menuItem1")
		{
			PlayerPrefs.DeleteAll();
			Application.LoadLevel(2);
		}
		if(name=="menuItem2")
		{
            var level  = PlayerPrefs.GetInt("SavedLevel");
            if (level == 0) level=2;
            else if (level < 5) level++;
			Application.LoadLevel(level);
		}
		if(name=="menuItem3")
		{
			Application.LoadLevel(1);
		}
		if(name=="menuItem4")
		{
			
		}
		if(name=="menuItem5")
		{
			Application.Quit();
		}
}

function OnMouseEnter()
{
	audio.PlayOneShot(clip2);

}


function OnMouseOver()
{
	
	transform.position.z=-2;
	renderer.material=mouseOverTextureMaterial;
	
}

function OnMouseExit()
{
	transform.position.z=-1;
	//renderer.material.mainTexture=mouseOverTextureExit;
	renderer.material=mouseOverTextureMaterialExit;
}

