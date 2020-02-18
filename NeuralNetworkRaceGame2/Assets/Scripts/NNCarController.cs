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
    public float sideSensorAngle = 90;
    public float diaSensorAngle = 45;
    
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
    private RaycastHit hitDiaLeft;
    private RaycastHit hitDiaRight;
    private List<RaycastHit> sensors = new List<RaycastHit>();

    private Checkpoint checkpoint;
    private Rigidbody rBody;

    private float lookingDir;
    private float speed;



    private void Start()
    {
        sensors.Add(hitForward);
        sensors.Add(hitLeft);
        sensors.Add(hitRight);
        sensors.Add(hitDiaLeft);
        sensors.Add(hitDiaRight);

        checkpoint = gameObject.GetComponent<Checkpoint>();
        rBody = gameObject.GetComponent<Rigidbody>();
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

    public float getNormSensors(int i)
    {
        return normalize(sensors[i]);
    }

    public float getSpeed()
    {
        return speed;
    }

    private float normalize(RaycastHit rchit)
    {
        float normLength;
        if (rchit.distance == 0)
        {
            normLength = 1;
        }
        else
        {
            normLength = rchit.distance / sensorLength;
        }
        return normLength;
    }

    public float getLookingDir()
    {
        return lookingDir;
    }

    private void updateLookingDirection()
    {
        GameObject nextCheckpoint = checkpoint.nextCheckpoint();
        lookingDir = Vector3.Dot(gameObject.transform.forward, (nextCheckpoint.transform.position - gameObject.transform.position).normalized);
    }

    private void updateSensors()
    {

        RaycastHit rForward;
        RaycastHit rLeft;
        RaycastHit rRight;
        RaycastHit rDiaLeft;
        RaycastHit rDiaRight;

        //front sensor
        if (Physics.Raycast(sensorForward.position, transform.forward, out rForward, sensorLength))
        {
            //Debug.Log("Front sensor: " + hitForward.collider.gameObject.name);
            //Debug.Log("Front sensor: " + tmpForward.distance);

        }
        sensors[0] = rForward;
        Debug.DrawLine(sensorForward.position, sensors[0].point);
        //Debug.Log("Front sensor: " + sensors[0].distance);

        //left sensor
        if (Physics.Raycast(sensorLeft.position, Quaternion.AngleAxis(-sideSensorAngle, gameObject.transform.up) * transform.forward, out rLeft, sensorLength))
        {
            //Debug.Log("Left sensor: " + tmpLeft.distance);
        }
        sensors[1] = rLeft;
        Debug.DrawLine(sensorLeft.position, sensors[1].point);

        //right sensor
        if (Physics.Raycast(sensorRight.position, Quaternion.AngleAxis(sideSensorAngle, gameObject.transform.up) * transform.forward, out rRight, sensorLength))
        {
            //Debug.Log("Right sensor: " + tmpRight.distance);
        }
        sensors[2] = rRight;
        Debug.DrawLine(sensorRight.position, sensors[2].point);

        //diagonal left sensor
        if (Physics.Raycast(sensorLeft.position, Quaternion.AngleAxis(-diaSensorAngle, gameObject.transform.up) * transform.forward, out rDiaLeft, sensorLength))
        {
            //Debug.Log("Right sensor: " + tmpRight.distance);
        }
        sensors[3] = rDiaLeft;
        Debug.DrawLine(sensorLeft.position, sensors[3].point);

        //diagonal Right sensor
        if (Physics.Raycast(sensorRight.position, Quaternion.AngleAxis(diaSensorAngle, gameObject.transform.up) * transform.forward, out rDiaRight, sensorLength))
        {
            //Debug.Log("Right sensor: " + tmpRight.distance);
        }
        sensors[4] = rDiaRight;
        Debug.DrawLine(sensorRight.position, sensors[4].point);

    }

    private void updateSpeed()
    {
        double velocity = rBody.velocity.magnitude * 3.6;
        speed = (float)System.Math.Round(velocity, 0);
    }

private void FixedUpdate()
    {
        ApplyChanges();
        UpdateWheelPoses();
        updateSensors();
        updateLookingDirection();
        updateSpeed();
    }
}
