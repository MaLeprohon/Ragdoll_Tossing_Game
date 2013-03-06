using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public Transform cannonBallPrefab;
    public Transform cannonBallSpawn;

    public int minAngle = 0, maxAngle = 60;
    public float minForce = 5000, maxForce = 10000;
    public float minMass = 1, maxMass = 3;
    public float minGravity = -15, maxGravity = 15;
    public float minVelocity = 10, maxVelocity = 50;
    public float minY = 0, maxY = 15;

    public float targetX, targetY;
    public int Angle;
    public float Force;
    public float Mass = 6;
    private float Gravity;
    private float X, Y; // x and y distances form the cannonBall to the target

    public float answer;
	float lifetime = 30;
	
    GameObject muzzle;
    Quaternion rotation;
    private Transform target;

    private GUIMenu gui;

    // Use this for initialization
    void Start()
    {
        gui = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GUIMenu>();
		//PlayerPrefs.DeleteAll();

        rotation = Quaternion.identity;
        muzzle = GameObject.FindGameObjectWithTag("muzzle");

        // Finding the x, y distances from the cannonBall to the target
        target = GameObject.FindGameObjectWithTag("target").transform;
        targetX = target.position.x;
        targetY = target.position.y;
        X = targetX - cannonBallSpawn.position.x;
        Y = targetY - cannonBallSpawn.position.y;
		
	
        Angle = Random.Range(minAngle, maxAngle);
		
        Force = Random.Range(minForce, maxForce);
        //Mass = Random.Range(minMass, maxMass);
        generateRandomValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevel == 5)
        {
            Angle = (int)gui.getInputValue2();
        }

        float c = calculateC();
        switch (gui.inputVariable)
        {
            case GUIMenu.Variable.Gravity:
                answer = c * Mathf.Pow(Force, 2);
                break;
            case GUIMenu.Variable.Velocity:
                answer = Mathf.Sqrt(Gravity * c);
                break;
            case GUIMenu.Variable.Y:
                float tan = Mathf.Tan(Angle * Mathf.Deg2Rad);
                float cos = Mathf.Cos(Angle * Mathf.Deg2Rad);
                float cos2 = cos * cos;
                answer = tan * X - Gravity * X * X / Mathf.Pow(Force * Time.fixedDeltaTime / Mass, 2) / cos2 / 2;
                targetY = gui.getInputValue() + cannonBallSpawn.position.y;
                break;
        }

        //only needed for level4 -- answer removed to avoid conflict
        switch (gui.inputVariable2)
        {
            case GUIMenu.Variable.Y:
                targetY = gui.getInputValue2() + cannonBallSpawn.position.y;
                break;
        }

        target.position = new Vector3(targetX, targetY);
        X = targetX - cannonBallSpawn.position.x;
        Y = targetY - cannonBallSpawn.position.y;

        rotation.eulerAngles = new Vector3(0, 0, Angle - 180);
        muzzle.transform.rotation = rotation;
        cannonBallSpawn.eulerAngles = new Vector3(-Angle, 90, 0);
    }

    private void Shoot()
    {
		float maxPitch = 1.2f;
		float minPitch = 0.8f;
		float maxVolume = 1.0f;
		float minVolume = 0.5f;
		
        Transform cannonBall = null;
		//audio.pitch = (v - minVelocity) / (maxVelocity - minVelocity) * (maxPitch- minPitch) + minPitch;
		//audio.volume = (v - minVelocity) / (maxVelocity - minVelocity) * (maxVolume- minVolume) + minVolume;
        if (cannonBallPrefab != null && cannonBallSpawn != null)
        {
            cannonBall = Instantiate(cannonBallPrefab, cannonBallSpawn.position, cannonBallSpawn.rotation) as Transform;
			//audio.pitch = (v - minVelocity) / (maxVelocity - minVelocity) * (maxPitch- minPitch) + minPitch;
			//audio.volume = (v - minVelocity) / (maxVelocity - minVelocity) * (maxVolume- minVolume) + minVolume;
			//audio.Play();
        }
		
		
		//destroy the ragdoll after its lifetime (set to 20 seconds)
		Destroy((cannonBall as Transform).gameObject, lifetime); 

		

        if (Application.loadedLevel != 5)
        {
            switch (gui.inputVariable)
            {
                case GUIMenu.Variable.Gravity:
                    Gravity = gui.getInputValue();
                    break;
                case GUIMenu.Variable.Velocity:
                    //Mass = Random.Range(minMass, maxMass);
                    Force = gui.getInputValue() * Mass / Time.fixedDeltaTime;
                    break;
            }
            Physics.gravity = new Vector3(0, -Gravity, 0);
        }
        else //if level4
        {
            Force = gui.getInputValue() * Mass / Time.fixedDeltaTime;
            Angle = (int)gui.getInputValue2();
            Physics.gravity = new Vector3(-30, -10, 0);
        }
		
		float v = Force * Time.fixedDeltaTime / Mass;
		audio.pitch = (v - minVelocity) / (maxVelocity - minVelocity) * (maxPitch- minPitch) + minPitch;
		audio.volume = (v - minVelocity) / (maxVelocity - minVelocity) * (maxVolume- minVolume) + minVolume;
	    cannonBall.rigidbody.AddForce(cannonBall.transform.forward * Force);
		audio.Play();
		

        generateRandomValues();
    }

    // Calculate the constant part of the equation
    private float calculateC()
    {
        float cos, cos2, tan;
        float c = 0;
        switch (gui.inputVariable)
        {
            case GUIMenu.Variable.Gravity:
                tan = Mathf.Tan(Angle * Mathf.Deg2Rad);
                cos = Mathf.Cos(Angle * Mathf.Deg2Rad);
                cos2 = cos * cos;
                c = 2 * Time.fixedDeltaTime * Time.fixedDeltaTime * cos2 * (tan * X - Y) / (X * X) / (Mass * Mass);
                break;
            case GUIMenu.Variable.Velocity:
                tan = Mathf.Tan(Angle * Mathf.Deg2Rad);
                cos = Mathf.Cos(Angle * Mathf.Deg2Rad);
                cos2 = cos * cos;
                c = (X * X) / (2 * cos2 * (tan * X - Y));
                break;
        }
        return c;
    }

    private void generateRandomValues()
    {
        float c = 0;
        float min, max;
        float tan, cos, cos2;
        switch (gui.inputVariable)
        {
            case GUIMenu.Variable.Gravity:
                c = calculateC();
                max = Mathf.Sqrt(maxGravity / Mathf.Abs(c));
                Force = Random.Range(minForce, max);
                break;
            case GUIMenu.Variable.Velocity:
				if(Application.loadedLevel!=5)
				{
                    c = calculateC();
                    if (c > 0)
                    {
                        min = (minVelocity * minVelocity) / c;
                        max = (maxVelocity * maxVelocity) / c;
                    }
                    else
                    {
                        max = (minVelocity * minVelocity) / c;
                        min = (maxVelocity * maxVelocity) / c;
                    }
					Gravity = Random.Range(min, max);
				}
				else
				{
					Gravity = 9.8f;
				}
				break;
            case GUIMenu.Variable.Y:
                tan = Mathf.Tan(Angle * Mathf.Deg2Rad);
                cos = Mathf.Cos(Angle * Mathf.Deg2Rad);
                cos2 = cos * cos;
                min = 2 * (tan * X - maxY) * Mathf.Pow(Force * Time.fixedDeltaTime / Mass, 2) * cos2 / (X * X);
                max = 2 * (tan * X - minY) * Mathf.Pow(Force * Time.fixedDeltaTime / Mass, 2) * cos2 / (X * X);
                Gravity = Random.Range(min, max);
                break;
        }
    }

    public void easyShoot()
    {
        Shoot();
    }

    public float getGravity()
    {
        return Gravity;
    }

    public float getAngle()
    {
        return Angle;
    }

    public float getForce()
    {
        return Force;
    }

    public float getMass()
    {
        return Mass;
    }

    public float getX()
    {
        return X;
    }

    public float getY()
    {
        return Y;
    }
} 
