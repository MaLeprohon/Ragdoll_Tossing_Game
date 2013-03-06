using UnityEngine;
using System.Collections;



public class SphereCollision : MonoBehaviour {
	public GameObject explosion;
	public GameObject groundExplosion;
	int levelValue;
	int attempts;
	int triesPoints;
		
	private IEnumerator OnCollisionEnter(Collision collision) {

        if(collision.gameObject.tag == "missile"){
			
			collision.gameObject.audio.pitch = Random.Range (0.9f, 1.1f);
			collision.gameObject.audio.Play ();
			audio.Play ();
			Instantiate(explosion, transform.position, transform.rotation);
			
			yield return new  WaitForSeconds(2);
			
			GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
			GUIMenu menu = camera.GetComponent<GUIMenu>();
			menu.hasWon = true;
			Instantiate(explosion, transform.position, transform.rotation);
						
			levelValue= Application.loadedLevel;
			PlayerPrefs.SetInt("SavedLevel", levelValue);
			
			//Destroy(collision.gameObject); //Not destroying the object on contact makes a nice effect
			//renderer.enabled=false;
			
			//fetch amount of tries and calculate bonus, add it to playerpref total
			attempts = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GUIMenu>().nTries;
			
			
				switch(attempts)
				{
				case 1:
				triesPoints=5000;
				break;
				
				case 2:
				triesPoints=3000;
				break;
				
				case 3:
				triesPoints=1000;
				break;
				
				default:
				triesPoints=0;
				break;
				}
			
			
			var fetchedPoints = PlayerPrefs.GetInt("Player Score");
			fetchedPoints+=triesPoints;
			PlayerPrefs.SetInt("Player Score", fetchedPoints);
		
		}
		

    }
}

