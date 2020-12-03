using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Random = UnityEngine.Random;
using System;


// Vehicle Controller 2 The electric boogaloo

public class KartController : Bolt.EntityBehaviour<IVehicleState>
{
    private GameObject camera;
    public KartWheel[] wheels;

    private Rigidbody rig;

    public float wheelbase;     // Distance between the wheels
    public float rearTrack;     // Track of the wheels
    public float turnRadius;    // Turn radius of the vehicle

    private float steerInput;

    private float
        steeringAngleLeft,
        steeringAngleRight;



    private float
        minRPM = 0,
        maxRPM = 100;


    // Boosting
    [Header("Boosting")]

    public GameObject boost_effect;

    public GameObject
        boostMeterUI,
        boostYellowMeter;

    public Vector3 boostYellowMeterOriginalPosition;

    // Boost values

    public float
        boostPower,
        slowPower,
        explosionPower;

    public float
        boostTime,
        boostTimeLow,
        boostTimeHigh;

    public float
        cooldownTime,
        cooldownDefault;

    public float
        boostFill,
        boostFillLow,
        boostFillHigh;

    public float boostWindowMax;
    public float boostMeterSpeed;
    public float boostMeterTime;

    public bool boosting;

    // Bool for boost bar
    private bool boostBarOn = false;

    private bool boostFlag = false;
    public bool cooldownFlag = false;

    private void Start()
    {
        rig = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {

        
        HandleInput();
        EngineAudio(minRPM, maxRPM);
    }


    public override void Attached()
    {
        if(entity.IsOwner)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
            transform.gameObject.tag = "Player";
            camera.transform.localPosition = gameObject.transform.position + new Vector3(0,2.5f,10);
            camera.transform.parent = gameObject.transform.parent;
            camera.transform.localRotation = new Quaternion(0,0,0, 0);
        }

        //If you're not the owner
        if (entity.IsOwner == false)
        {
            entity.tag = "Remote-Player";
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        //SetTransforms tells Bolt to replicate the transform over the network
        state.SetTransforms(state.VehicleTransform, transform);

        boostYellowMeterOriginalPosition = new Vector3(boostYellowMeter.transform.position.x, boostYellowMeter.transform.position.y, boostYellowMeter.transform.position.z);
    }

    public override void SimulateOwner()
    {

    }

    void FixedUpdate()
    {
        if (entity.IsOwner)
            SetSteeringAngle();
        

        if(boostFlag && !cooldownFlag && entity.IsOwner)
        {
            //mittari päälle
            boostMeterUI.SetActive(true);
            boostYellowMeter.SetActive(true);

            if (boostBarOn == false)
                StartCoroutine(BoostBar(boostFill, boostWindowMax, boostMeterTime));

            if (boostBarOn == true && boostFill > 0)
                BoostMode(boostFill);

            boostFlag = false;
        }
    }

    private IEnumerator cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldownFlag = false;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boostFlag = true;
        }
    }

    /* Based on ackerman steering
            
        L = wheelbase
        T = track (the distance between the tires)
        R = radius of the turn

        inner wheels angle = iW
        outer wheel angle = oW


        iW = atan( L / ( R + T/2 ))
        oW = atan( L / ( R - T/2 ))

        Source:
        http://datagenetics.com/blog/december12016/index.html


        */
    void SetSteeringAngle()
    {

        steerInput = Input.GetAxis("Horizontal");

        if (steerInput > 0) // Turning Right
        {
            steeringAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius + (rearTrack / 2))) * steerInput;
            steeringAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius - (rearTrack / 2))) * steerInput;
        }

        if (steerInput < 0) // Turning Left
        {
            steeringAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius - (rearTrack / 2))) * steerInput;
            steeringAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (turnRadius + (rearTrack / 2))) * steerInput;
        }

        if (steerInput == 0) // No input
        {
            steeringAngleLeft = 0;
            steeringAngleRight = 0;

        }

        foreach (KartWheel w in wheels)
        {

            if (w.wheel == KartWheel.Wheels.Front_Left)
            {
                w.steerAngle = steeringAngleLeft;
            }

            if (w.wheel == KartWheel.Wheels.Front_Right)
            {
                w.steerAngle = steeringAngleRight;
            }


        }

    }


    // Calculate boost meter level
    public IEnumerator BoostBar(float min, float max, float filltime)
    {
        yield return new WaitForEndOfFrame();
        float fill = 0f;
        float fillRate = (max / filltime) * boostMeterSpeed;
        boostBarOn = true;
        while (boostBarOn)
        {
            // liikuttaa keltaista mittaria
            boostYellowMeter.transform.Translate(new Vector3(250f * Time.deltaTime, 0, 0));

            // boost bar on ehkä tähän 
            fill += Time.deltaTime * fillRate;
            boostFill = Mathf.Lerp(min, max, fill);
            if (boostFill >= boostWindowMax)
            {
                Debug.Log("RESETTED");
                // palauttaa keltaisen mittarin paikoilleen
                boostYellowMeter.transform.position = boostYellowMeterOriginalPosition;
                ResetBoostBar();
            }
            yield return null;
        }
    
    }

    


    //Reset boost difficulty to default
    public void ResetValues()
    {
        cooldownTime = cooldownDefault;
        boostTimeHigh = boostFillHigh;
        boostTimeLow = boostFillLow;
    }

    //Reset boost bar
    public void ResetBoostBar()
    {
        //Debug.Log("RESETTED");
        boostBarOn = false;
        boostFill = 0;

        //mittari pois päältä
        boostMeterUI.SetActive(false);
        //boostYellowMeter.SetActive(false);
    }

    //Handle what happens when boost button is pressed second time
    public void BoostMode(float meterFill)
    {
        //if pressed right on time: boost
        if (meterFill > boostTimeLow && meterFill < boostTimeHigh && !boosting)
        {
            Debug.Log("on time");
            Boost();
            cooldownFlag = true;
            //cooldownTime = 3;
            //boostTimeHigh -= boostTimeHigh*0.2f;
            //boostTimeLow += boostTimeLow*0.2f;
        }

        //if not pressed at all while boosting: reset meter
        if (meterFill >= boostWindowMax)
        {
            Debug.Log("RESET METER");
            ResetBoostBar();
            ResetValues();
        }

        //if pressed too early: slowdown
        if (meterFill < boostTimeLow)
        {
            Debug.Log("too early");
            SlowDown();
            cooldownFlag = true;
            //ResetBoostBar();
            //ResetValues();
        }

        //if pressed too late: slowdown
        if (boostFill > boostTimeHigh)
        {
            Debug.Log("too late");
            SlowDown();
            cooldownFlag = true;
            //ResetValues();
        }
        StartCoroutine(cooldown());

    }

    ////Slowdown used when boosting is failed NOT FINAL FORM!!
    //private void SlowDown()
    //{
    //    //Apply slowdown for set time
    //    for (float i = 0; i < boostTime; i += Time.deltaTime)
    //    {
    //        //Checking if both the front tires are on the ground
    //        if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[2] != new Vector3(0, 0, 0))
    //        {
    //            //The point between the front tires
    //            frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

    //            Vector3 fwd = rig.transform.forward;

    //            //Adding the slowdown to all tires
    //            rig.AddForceAtPosition(fwd * -brakingPower, frontWheels);
    //            rig.AddForceAtPosition(fwd * -brakingPower, backWheels);
    //        }

    //    }
    //}

    // Applying the boost to the vehicle, Edited to work with the new vehicle controller
    private void Boost()
    {
        boosting = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Boost", GetComponent<Transform>().position);
        StartCoroutine(BoostCoolDown());
        boost_effect.SetActive(true);
        StartCoroutine(BoostTrailTime());

        //Apply boost for set time
        //for (float i = 0; i < boostTime; i += Time.deltaTime)
        //{
        //    //Checking if both the front tires are on the ground
        //    if (contactPoints[0] != new Vector3(0, 0, 0) && contactPoints[2] != new Vector3(0, 0, 0))
        //    {
        //        //The point between the front tires
        //        frontWheels = contactPoints[0] + (contactPoints[1] - contactPoints[0]) / 2;

        //        Vector3 fwd = rig.transform.forward;

        //        //Adding the boosted force to the position of the frontwheels
        //        rig.AddForceAtPosition(fwd * enginePower * boostPower, frontWheels);
        //    }
        //}

        foreach (KartWheel w in wheels)
        {

            if (w.wheel == KartWheel.Wheels.Front_Left)
            {
                w.Boost(boostPower);
            }

            if (w.wheel == KartWheel.Wheels.Front_Right)
            {
                w.Boost(boostPower);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > 2000)
        {
            Debug.Log(collision.impulse.magnitude);
            FMODUnity.RuntimeManager.PlayOneShot("event:/ImpactLight", GetComponent<Transform>().position);
        }
        if (collision.impulse.magnitude >9000)
        {
            Debug.Log(collision.impulse.magnitude);
            FMODUnity.RuntimeManager.PlayOneShot("event:/ImpactHard", GetComponent<Transform>().position);
        }
        
    }

    // Slowdown used when boosting fails, Edited to work with the new vehicle controller
    private void SlowDown()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/BoostFail", GetComponent<Transform>().position);
        foreach (KartWheel w in wheels)
        {

            if (w.wheel == KartWheel.Wheels.Front_Left)
            {
                w.SlowDown(slowPower);
            }

            if (w.wheel == KartWheel.Wheels.Front_Right)
            {
                w.SlowDown(slowPower);
            }
        }
    }

    public void OnWeaponHit()
    {
        foreach (KartWheel w in wheels)
        {
            w.OnExplosion(explosionPower);
            //if (w.wheel == KartWheel.Wheels.Front_Left)
            //{
            //    w.OnExplosion(explosionPower);
            //}

            //if (w.wheel == KartWheel.Wheels.Front_Right)
            //{
            //    w.OnExplosion(explosionPower);
            //}
        }
    }

    IEnumerator BoostTrailTime()
    {
        yield return new WaitForSeconds(1.5f);
        boost_effect.SetActive(false);
    }
    //Cooldown for boost
    public IEnumerator BoostCoolDown()
    {
        yield return new WaitForSeconds(cooldownTime);
        boosting = false;
        boost_effect.SetActive(false);
        yield return null;
    }


    // Returns RPM of the vehicle
    public void EngineAudio(float min, float max)
    {
        var speed = Mathf.Log10(rig.velocity.sqrMagnitude);
        float RPM = Mathf.Lerp(min, max, speed/5);
        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.SetParameter("RPM", RPM);

    }



}