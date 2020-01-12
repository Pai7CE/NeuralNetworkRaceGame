using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNCarController: MonoBehaviour
{
    private float newSteerangle = 0f;

    private float newRPMotor = 0;
    private float newRDMotor = 0;
    private float newFPMotor = 0;
    private float newFDMotor = 0;

    private float newRPBrake = 0;
    private float newRDBrake = 0;
    private float newFPBrake = 0;
    private float newFDBrake = 0;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public MeshRenderer leftBrakeLight;
    public MeshRenderer rightBrakeLight;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float brakeForce = 50;
    public float naturalDecelaration;
    //public float wheelTurnDegree = 1;

    public bool rearWheelDrive = false;
    public bool frontWheelDrive = false;


    //private void setSteeringAngle(float steerAngle)
    //{ 
    //    frontDriverW.steerAngle = steerAngle;
    //    frontPassengerW.steerAngle = steerAngle;
    //}

    public void Steer(float horizontal)
    {
        newSteerangle = maxSteerAngle * horizontal;
    }
    
    public void Drive(float vertical)
    {
        if(vertical >= 0)
        {
            newRDBrake = 0;
            newRPBrake = 0;
            newFDBrake = 0;
            newFPBrake = 0;

            if (rearWheelDrive)
            {
                newRDMotor = vertical * motorForce;
                newRPMotor = vertical * motorForce;
            }
            if (frontWheelDrive)
            {
                newFDMotor = vertical * motorForce;
                newFPMotor = vertical * motorForce;
            }
        }
        //Braking
        if (vertical < 0)
        {
            enableBrakeLights();

            rearDriverW.motorTorque = newRDMotor;
            rearPassengerW.motorTorque = newRPMotor;
            frontDriverW.motorTorque = newFDMotor;
            frontPassengerW.motorTorque = newFPMotor;

            //4W braking
            //rearDriverW.brakeTorque = m_negVerticalInput * brakeForce;
            //rearPassengerW.brakeTorque = m_negVerticalInput * brakeForce;
            newFDBrake = -vertical * brakeForce;
            newFPBrake = -vertical * brakeForce;
        }
        else
        {
            disableBrakeLights();
        }
    }
    //Applies all changes made in FixedUpdate for stable physics
    private void ApplyChanges()
    {
        //Updating steer angle
        frontDriverW.steerAngle = newSteerangle;
        frontPassengerW.steerAngle = newSteerangle;

        //Brake torque
        frontDriverW.brakeTorque = newFDBrake;
        frontPassengerW.brakeTorque = newFPBrake;
        rearDriverW.brakeTorque = newRDBrake;
        rearPassengerW.brakeTorque = newRPBrake;

        //Motor torque
        frontDriverW.motorTorque = newFDMotor;
        frontPassengerW.motorTorque = newFPMotor;
        rearDriverW.motorTorque = newRDMotor;
        rearPassengerW.motorTorque = newRPMotor;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 pos; //= _transform.position
        Quaternion quat; // = _transform.rotation

        _collider.GetWorldPose(out pos, out quat);

        _transform.position = pos;
        _transform.rotation = quat;
    }
    private void enableBrakeLights()
    {
        leftBrakeLight.enabled = true;
        rightBrakeLight.enabled = true;
    }
    private void disableBrakeLights()
    {
        leftBrakeLight.enabled = false;
        rightBrakeLight.enabled = false;
    }

private void FixedUpdate()
    {
        //GetInput();
        //Steer();
        //Accelerate();
        //Brake();
        ApplyChanges();
        UpdateWheelPoses();
    }
}
