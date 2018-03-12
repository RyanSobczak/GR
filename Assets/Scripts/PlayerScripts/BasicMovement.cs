using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

    //basic input variables
    private bool mMoveForward, mMoveBack,
                 mMoveRight, mMoveLeft, mBrake;

    //the variables that are changed for the vehicles;
    public float mAcceleration, mDecceleration, mHandling, mTopSpeed, mTopReverseSpeed;

    private float mSpeed;

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
        
    }

    void FixedUpdate()
    {
        ModifyMovement();
    }

    void ModifyMovement()
    {
        //actual physics based acceleration and decceleration
        if(mMoveForward)
        {
            if (mSpeed >= mTopSpeed)
                mSpeed = mTopSpeed;
            else
                mSpeed = mSpeed + mAcceleration * Time.deltaTime;
        }

        //reverse, broken at the moment
        //if (mMoveBack)
        //{
        //    if (mSpeed >= mTopReverseSpeed)
        //        mSpeed = mTopReverseSpeed;
        //    else
        //        mSpeed = mSpeed + mAcceleration * Time.deltaTime;

        //    //GetComponent<Rigidbody>().velocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity) * mSpeed;
        //}

        if (mBrake)
        {
            if (mSpeed <= 0)
                mSpeed = 0;
            else
                mSpeed = mSpeed + mDecceleration * Time.deltaTime;
        }

        //Turning, turning about is dependant on speed, like actual cars and scooters, i guess
        if(mMoveRight)
        {
            if(mSpeed != 0.0f)
            transform.Rotate(transform.up * (mHandling / mSpeed));
        }

        if (mMoveLeft)
        {
            if (mSpeed != 0.0f)
                transform.Rotate(transform.up * (-mHandling / mSpeed));
        }


        

        print(mSpeed);

        //last step is to add the velovity
        GetComponent<Rigidbody>().velocity = transform.forward * mSpeed;
    }

    //Temp code that wwill go into its own cs file 
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            mMoveForward = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            mMoveForward = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            mMoveBack = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            mMoveBack = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            mMoveLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            mMoveLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            mMoveRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            mMoveRight = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mBrake = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            mBrake = false;
        }
    }
}
