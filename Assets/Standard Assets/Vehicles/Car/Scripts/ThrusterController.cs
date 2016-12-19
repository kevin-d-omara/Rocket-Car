using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class ThrusterController : MonoBehaviour
{
    public delegate void FuelUpdate(float percentFuelRemaining);
    public static event FuelUpdate OnFuelUpdate;

    [SerializeField] private float thrustForce = 100f; // Newtons
    [SerializeField] private float fuelDuration = 6f; // second
    private float fuelRemaining;

    private float timeAcc = 0f;

    private void Awake()
    {
        fuelRemaining = fuelDuration;
    }

    // Applies thrust to the parent GameObject's Rigidbody:
    //      //- at the location of this thruster
    //      - in the direction the thruster faces
    public void ApplyThrust()
    {
        if (fuelRemaining > 0f)
        {
            timeAcc += Time.deltaTime;
            fuelRemaining -= Time.deltaTime;
            fuelRemaining = Mathf.Clamp(fuelRemaining, 0, fuelDuration); // ensure fuel doesn't drop below zero
            if (OnFuelUpdate != null)
            {
                float percentFuelRemaining = fuelRemaining / fuelDuration;
                OnFuelUpdate(Mathf.Clamp(percentFuelRemaining, 0, 1));
            }


            Rigidbody rb = GetComponentInParent<Rigidbody>();
            //rb.AddForce(transform.forward * thrustForce); // center of RocketCar GameObject
            rb.AddForceAtPosition(transform.forward * thrustForce, transform.position); // center of Thruster GameObject
        }
    }

    public void ResetFuel()
    {
        fuelRemaining = fuelDuration;

        if (OnFuelUpdate != null)
        {
            float percentFuelRemaining = fuelRemaining / fuelDuration;
            OnFuelUpdate(Mathf.Clamp(percentFuelRemaining, 0, 1));
        }
    }
}
