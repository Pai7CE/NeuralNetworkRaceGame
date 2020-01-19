using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNCarController: MonoBehaviour
{

    [Header("Wheelcollider")]
    public WheelCollider frontDriverW;
    public WheelCollider frontPassengerW;
    public WheelCollider rearDriverW;
    public WheelCollider rearPassengerW;


    [Header("Transform")]

    public Transform frontDriverT;
    public Transform frontPassengerT;
    public Transform rearDriverT;
    public Transform rearPassengerT;

    [Header("Brakes")]

    public MeshRenderer leftBrakeLight;
    public MeshRenderer rightBrakeLight;

    [Header("Sensors")]
    public float sensorLength = 15f;
    public Transform sensorForward;
    public Transform sensorLeft;
    public Transform sensorRight;
    public float startPositionSide;
    public float sideSensorAngle;
    
    [Header("General")]

    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float brakeForce = 50;
    //public float naturalDecelaration;
    //public float wheelTurnDegree = 1;

    public bool rearWheelDrive = false;
    public bool frontWheelDrive = false;

    private float newSteerangle = 0f;

    private float newRPMotor = 0;
    private float newRDMotor = 0;
    private float newFPMotor = 0;
    private float newFDMotor = 0;

    private float newRPBrake = 0;
    private float newRDBrake = 0;
    private float newFPBrake = 0;
    private float newFDBrake = 0;

    //sensor variables

    private RaycastHit hitForward;
    private RaycastHit hitLeft;
    private RaycastHit hitRight;
    private List<RaycastHit> sensors = new List<RaycastHit>();
    //private Vector3 leftSensorPos;
    //private Vector3 rightSensorPos;

    private Checkpoint checkpoint;
    private float lookingDir;



    private void Start()
    {
        sensors.Add(hitForward);
        sensors.Add(hitLeft);
        sensors.Add(hitRight);

        checkpoint = gameObject.GetComponent<Checkpoint>();
    }

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

    public List<RaycastHit> getSensors()
    {
        return sensors;
    }

    public float getLookingDir()
    {
        return lookingDir;
    }

    private void updateLookingDirection()
    {
        GameObject nextCheckpoint = checkpoint.nextCheckpoint();
        lookingDir = Vector3.Dot(gameObject.transform.forward, (nextCheckpoint.transform.position - gameObject.transform.position).normalized);
        //Debug.Log(lookingDir);
    }

    private void updateSensors()
    {

        RaycastHit tmpForward;
        RaycastHit tmpLeft;
        RaycastHit tmpRight;

        //front sensor
        if (Physics.Raycast(sensorForward.position, transform.forward, out tmpForward, sensorLength))
        {
            //Debug.Log("Front sensor: " + hitForward.collider.gameObject.name);
            //Debug.Log("Front sensor: " + tmpForward.distance);

        }
        sensors[0] = tmpForward;
        Debug.DrawLine(sensorForward.position, sensors[0].point);
        //Debug.Log("Front sensor: " + sensors[0].distance);

        //left sensor
        if (Physics.Raycast(sensorLeft.position, Quaternion.AngleAxis(-sideSensorAngle, gameObject.transform.up) * transform.forward, out tmpLeft, sensorLength))
        {
            //Debug.Log("Left sensor: " + tmpLeft.distance);
        }
        sensors[1] = tmpLeft;
        Debug.DrawLine(sensorLeft.position, sensors[1].point);

        //right sensor
        if (Physics.Raycast(sensorRight.position, Quaternion.AngleAxis(sideSensorAngle, gameObject.transform.up) * transform.forward, out tmpRight, sensorLength))
        {
            //Debug.Log("Right sensor: " + tmpRight.distance);
        }
        sensors[2] = tmpRight;
        Debug.DrawLine(sensorRight.position, sensors[2].point);
    }

private void FixedUpdate()
    {
        //GetInput();
        //Steer();
        //Accelerate();
        //Brake();
        ApplyChanges();
        UpdateWheelPoses();
        updateSensors();
        updateLookingDirection();
    }
}
