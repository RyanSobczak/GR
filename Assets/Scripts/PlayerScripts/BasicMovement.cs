using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMovement : MonoBehaviour {

    //basic input variables
    private bool mMoveForward, mMoveBack,
                 mMoveRight, mMoveLeft, mBrake;

    //the variables that are changed for the vehicles;
    public float mAcceleration, //how fast the scooter can accelerate 
        mDecceleration, //how quickly you can brake, should be faster than accereration speed
        mHandling, //how good the turning is, higher the number the better the turning 
        mTopSpeed, //fasting forward speed  
        mTopReverseSpeed; //fastest reverse speed, should be slower than fastest forward speed

    private float mSpeed;

    private bool gravity;
    public LayerMask ground;

    // Use this for initialization
    void Start () {

        mMoveForward = false;
        mMoveBack = false;
        mMoveRight = false;
        mMoveLeft = false;

        mSpeed = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        CheckGround();
        
    }

    void FixedUpdate()
    {
        ModifyMovement();
    }

    void CheckGround()
    {
        float groundBufferLimit = 0.7f;
        bool hit = Physics.Raycast(transform.position, -Vector3.up, groundBufferLimit, ground);
        Debug.DrawRay(transform.position, -Vector3.up * groundBufferLimit, Color.black);

        if (hit)
        {
            print("hitting Ground");
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().AddForce(-20.0f * transform.up * GetComponent<Rigidbody>().mass, ForceMode.Force);
        }
        else
        {
            print("not hitting ground");
            GetComponent<Rigidbody>().freezeRotation = true;
            //transform.rotation = new Quaternion(transform.rotation.x, 0.0f, transform.rotation.z, transform.rotation.w);
            GetComponent<Rigidbody>().AddForce(-250.0f * transform.up * GetComponent<Rigidbody>().mass, ForceMode.Force);
        }
    }

    void ModifyMovement()
    {
        //add gravity
        //if(gravity)
        //GetComponent<Rigidbody>().AddForce(-50f * transform.up * GetComponent<Rigidbody>().mass, ForceMode.Force);
        //GetComponent<Rigidbody>().AddForce(-9.81f * transform.up * GetComponent<Rigidbody>().mass);

        //actual physics based acceleration and decceleration
        if (mMoveForward)
        {
            if (mSpeed >= mTopSpeed)
                mSpeed = mTopSpeed;
            else
                mSpeed = mSpeed + mAcceleration * Time.deltaTime;
        }

        //reverse, broken at the moment
        if (mMoveBack)
        {
            if (mSpeed <= mTopReverseSpeed)
                mSpeed = mTopReverseSpeed;
            else
                mSpeed = mSpeed - mAcceleration * Time.deltaTime;

            //transform.Translate(transform.InverseTransformDirection(transform.forward) * mSpeed * Time.deltaTime);
            //GetComponent<Rigidbody>().velocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity) * mSpeed;
        }

        if (mBrake)
        {
            if (mSpeed <= 0)
                mSpeed = 0;
            else
                mSpeed = mSpeed + mDecceleration * Time.deltaTime;
        }

        //Turning, turning about is dependant on speed, like actual cars and scooters, i guess
        if (mMoveRight)
        {
            if (mSpeed != 0.0f)
                transform.Rotate(transform.up * mHandling);
        }

        if (mMoveLeft)
        {
            if (mSpeed != 0.0f)
                transform.Rotate(transform.up * -mHandling);
        }




        print(mSpeed);

        //last step is to add the velovity
        //if(mSpeed > 0)
            GetComponent<Rigidbody>().velocity = transform.forward * mSpeed;
        //else
        //{
            //GetComponent<Rigidbody>().velocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity) * -mSpeed;
        //}
    }

    //Temp code that wwill go into its own cs file 
    void CheckInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            mMoveForward = true;
        }
        else 
        {
            mMoveForward = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            mMoveBack = true;
        }
        else 
        {
            mMoveBack = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            mMoveLeft = true;
        }
        else 
        {
            mMoveLeft = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            mMoveRight = true;
        }
        else
        {
            mMoveRight = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            mBrake = true;
        }
        else 
        {
            mBrake = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
