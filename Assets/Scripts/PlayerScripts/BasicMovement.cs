using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicMovement : MonoBehaviour {

    //basic input variables
    private bool mMoveForward, mMoveBack,
                 mMoveRight, mMoveLeft, mBrake,
                 mCantMoveForward, mAirborne;

    //the variables that are changed for the vehicles;
    public float mAcceleration, //how fast the scooter can accelerate 
        mDeceleration, //how fast you slow down if the gas is not pushed down
        mBraking, //how quickly you can brake, should be faster than accereration speed
        mHandling, //how good the turning is, higher the number the better the turning 
        mTopSpeed, //fasting forward speed  
        mTopReverseSpeed, //fastest reverse speed, should be slower than fastest forward speed
        mFallingGravity, //how hard you fall
        m_OnGroundGravity, //gravity on the ground 
        mGroundBufferLimit, //ray cast to find ground under player 
        mForwardGroundBufferLimit; //ray cast to find a wall infront of the player

    private float mSpeed;
    public LayerMask ground;

    // Use this for initialization
    void Start () {

        mMoveForward = false;
        mMoveBack = false;
        mMoveRight = false;
        mMoveLeft = false;
        mCantMoveForward = false;
        mAirborne = false;

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
        Vector3 firstPos = transform.position + transform.forward * 0.5f;
        bool hit = Physics.Raycast(firstPos, -transform.up, mGroundBufferLimit, ground);
        Debug.DrawRay(firstPos, -transform.up * mGroundBufferLimit, Color.black);

        Vector3 secondPos = transform.position + transform.forward * -1.7f;
        bool hit1 = Physics.Raycast(secondPos, -transform.up, mGroundBufferLimit, ground);
        Debug.DrawRay(secondPos, -transform.up * mGroundBufferLimit, Color.black);

        if (hit || hit1)
        {
            mAirborne = false;
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().AddForce(m_OnGroundGravity * transform.up * GetComponent<Rigidbody>().mass, ForceMode.Force);
        }
        else
        {
            mAirborne = true;
            transform.rotation = new Quaternion(0.0f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().AddForce(mFallingGravity * Vector3.up * GetComponent<Rigidbody>().mass, ForceMode.Force);
        }

        bool hitforward = Physics.Raycast(transform.position, transform.forward, mForwardGroundBufferLimit, ground);
        Debug.DrawRay(transform.position, transform.forward * mForwardGroundBufferLimit, Color.black);

        if (hitforward)
        {
            if (!mCantMoveForward)
            {
                mCantMoveForward = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                mSpeed = 0.0f;
            }
            
        }
        else
        {
            mCantMoveForward = false;
        }
    }

    void ModifyMovement()
    {
        //actual physics based acceleration and decceleration
        if (mMoveForward && !mCantMoveForward && !mAirborne)
        {
            if (mSpeed >= mTopSpeed)
                mSpeed = mTopSpeed;
            else
                mSpeed = mSpeed + mAcceleration * Time.deltaTime;
        }

        //reverse, broken at the moment
        if (mMoveBack && !mAirborne)
        {
            if (mSpeed <= mTopReverseSpeed)
                mSpeed = mTopReverseSpeed;
            else
                mSpeed = mSpeed - mAcceleration * Time.deltaTime;
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
        if (mMoveRight && !mAirborne)
        {
            if (mSpeed != 0.0f)
                transform.Rotate(transform.up * mHandling);
        }

        if (mMoveLeft && !mAirborne)
        {
            if (mSpeed != 0.0f)
                transform.Rotate(transform.up * -mHandling);
        }

        //last step is to add the velovity
        GetComponent<Rigidbody>().velocity = transform.forward * mSpeed;
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
