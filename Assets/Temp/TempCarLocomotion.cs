using UnityEngine;
using System.Collections;

public class TempCarLocomotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    private const float MAX_SPEED = 60;

   
    public float acceleration;
    public float turnSpeed = 10;

  



     Vector3 curNormal = Vector3.up;
    float facing=-90;
    // Update is called once per frame
    void Update () {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -curNormal, out hit))
        {

                if (Input.GetKey(KeyCode.W))
            {
                if (GetComponent<Rigidbody>().velocity.magnitude < MAX_SPEED)
                    GetComponent<Rigidbody>().AddForce(transform.forward * acceleration);

            }
            if (Input.GetKey(KeyCode.D))
            {
                if (GetComponent<Rigidbody>().velocity.magnitude > 2)
                    facing += turnSpeed;



            }
            if (Input.GetKey(KeyCode.A))
            {

                if (Mathf.Abs(GetComponent<Rigidbody>().velocity.magnitude) > 2)
                    facing -= turnSpeed;



            }
            if (Input.GetKey(KeyCode.S))
            {

       
                if(GetComponent<Rigidbody>().velocity.magnitude > -MAX_SPEED)
                    GetComponent<Rigidbody>().AddForce(-transform.forward * acceleration);

            }

      

            // after raycast, or however you get normal:
            // Compute angle to tilt with ground:
            Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, hit.normal);
            
            transform.rotation = Quaternion.Euler(0, facing, 0);
            // tilt to align with ground:   
            transform.rotation = grndTilt * transform.rotation;
        }



        float dot =Vector3.Dot(new Vector3(transform.forward.x, 0, transform.forward.z), new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z).normalized );
        if (Mathf.Abs(dot)<.7f)
        {
            GameObject.Find("sparksLeft").GetComponent<ParticleSystem>().emissionRate = (int)(700 *( Mathf.Abs(GetComponent<Rigidbody>().velocity.magnitude) / MAX_SPEED));
            GameObject.Find("sparksRight").GetComponent<ParticleSystem>().emissionRate = (int)(700 *( Mathf.Abs(GetComponent<Rigidbody>().velocity.magnitude) / MAX_SPEED));
            GameObject.Find("sparksLeft").GetComponent<ParticleSystem>().Play();
            GameObject.Find("sparksRight").GetComponent<ParticleSystem>().Play();
      
        }
        else
        {
            GameObject.Find("sparksLeft").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("sparksRight").GetComponent<ParticleSystem>().Stop();
        }


    }
}
 