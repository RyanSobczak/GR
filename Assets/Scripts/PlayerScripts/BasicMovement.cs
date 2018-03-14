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
        mDeceleration, //how fast you slow down if the gas is not pushed down
        mBraking, //how quickly you can brake, should be faster than accereration speed
        mHandling, //how good the turning is, higher the number the better the turning 
        mTopSpeed, //fasting forward speed  
        mTopReverseSpeed, //fastest reverse speed, should be slower than fastest forward speed
        mFallingGravity; //how hard you fall

    private float mSpeed;
    private const float mc_OnGroundGravity = -20.0f;
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
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().AddForce(mc_OnGroundGravity * transform.up * GetComponent<Rigidbody>().mass, ForceMode.Force);
        }
        else
        {
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().AddForce(mFallingGravity * transform.up * GetComponent<Rigidbody>().mass, ForceMode.Force);
        }
    }

    void ModifyMovement()
    {
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

        //Decel speed if gas not pressed 
        if(mSpeed > 0 && !mMoveForward)
        {
            mSpeed = mSpeed - mDeceleration * Time.deltaTime;
        }
        else if (mSpeed < 0 && !mMoveBack)
        {
            mSpeed = mSpeed + mDeceleration * Time.deltaTime;
        }

        //braking
        if (mBrake)
        {
            if (mSpeed <= 0)
                mSpeed = 0;
            else
                mSpeed = mSpeed + mBraking * Time.deltaTime;
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
