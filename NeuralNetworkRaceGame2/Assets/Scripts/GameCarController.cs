using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCarController: MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;
    private float m_negVerticalInput;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float brakeForce = 50;
    public float naturalDecelaration;
    public bool rearWheelDrive = false;
    public bool frontWheelDrive = false;

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        m_negVerticalInput = m_verticalInput * -1;
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
    }
    
    private void Accelerate()
    {
        if(m_verticalInput >= 0)
        {
            frontDriverW.brakeTorque = 0;
            frontPassengerW.brakeTorque = 0;
            rearDriverW.brakeTorque = 0;
            rearPassengerW.brakeTorque = 0;

            if (rearWheelDrive)
            {
                rearDriverW.motorTorque = m_verticalInput * motorForce;
                rearPassengerW.motorTorque = m_verticalInput * motorForce;
            }
            if (frontWheelDrive)
            {
                frontDriverW.motorTorque = m_verticalInput * motorForce;
                frontPassengerW.motorTorque = m_verticalInput * motorForce;
            }
        }      
    }
    private void Brake()
    {
        if(m_verticalInput < 0)
        {
            rearDriverW.motorTorque = 0;
            rearPassengerW.motorTorque = 0;
            frontDriverW.motorTorque = 0;
            frontPassengerW.motorTorque = 0;

            //4W braking
            //rearDriverW.brakeTorque = m_negVerticalInput * brakeForce;
            //rearPassengerW.brakeTorque = m_negVerticalInput * brakeForce;
            frontDriverW.brakeTorque = m_negVerticalInput * brakeForce;
            frontPassengerW.brakeTorque = m_negVerticalInput * brakeForce;
        }
    }

    private void Decelarate()
    {
        if(m_verticalInput == 0)
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
