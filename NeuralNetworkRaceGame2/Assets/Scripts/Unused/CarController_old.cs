using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController_old: MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
    private float negVerticalInput;

    private float newSteeringAngle;

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
    public float wheelTurnDegree = 1;

    public bool rearWheelDrive = false;
    public bool frontWheelDrive = false;


    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        negVerticalInput = verticalInput * -1;
    }
    private void setSteeringAngle(float steerAngle)
    {
        frontDriverW.steerAngle = steerAngle;
        frontPassengerW.steerAngle = steerAngle;
    }

    private void Steer()
    {
        /*if(horizontalInput > 0)
        {
            newSteeringAngle = frontDriverW.steerAngle + wheelTurnDegree;
            if(newSteeringAngle >= maxSteerAngle)
            {
                newSteeringAngle = maxSteerAngle;
            }
            setSteerAngle(newSteeringAngle);
        } */

        steeringAngle = maxSteerAngle * horizontalInput;
        setSteeringAngle(steeringAngle);
    }
    
    private void Accelerate()
    {
        if(verticalInput >= 0)
        {
            frontDriverW.brakeTorque = 0;
            frontPassengerW.brakeTorque = 0;
            rearDriverW.brakeTorque = 0;
            rearPassengerW.brakeTorque = 0;

            if (rearWheelDrive)
            {
                rearDriverW.motorTorque = verticalInput * motorForce;
                rearPassengerW.motorTorque = verticalInput * motorForce;
            }
            if (frontWheelDrive)
            {
                frontDriverW.motorTorque = verticalInput * motorForce;
                frontPassengerW.motorTorque = verticalInput * motorForce;
            }
        }      
    }
    private void Brake()
    {
        if(verticalInput < 0)
        {
            rearDriverW.motorTorque = 0;
            rearPassengerW.motorTorque = 0;
            frontDriverW.motorTorque = 0;
            frontPassengerW.motorTorque = 0;

            //4W braking
            //rearDriverW.brakeTorque = m_negVerticalInput * brakeForce;
            //rearPassengerW.brakeTorque = m_negVerticalInput * brakeForce;
            frontDriverW.brakeTorque = negVerticalInput * brakeForce;
            frontPassengerW.brakeTorque = negVerticalInput * brakeForce;
        }
        //responsible for visual brake lights
        if (Input.GetKey(KeyCode.S))
        {
            enableBrakeLights();
        }
        else
        {
            disableBrakeLights();
        }
    }

    private void Decelarate()
    {
        if(verticalInput == 0)
        {
            rearDriverW.brakeTorque = naturalDecelaration;
            rearPassengerW.brakeTorque = naturalDecelaration;
            frontDriverW.brakeTorque = naturalDecelaration;
            frontPassengerW.brakeTorque = naturalDecelaration;
        }
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
        GetInput();
        Steer();
        Accelerate();
        Brake();
        Decelarate();
        UpdateWheelPoses();
    }
}
