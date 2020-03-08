using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasSpeed : MonoBehaviour
{

    public Rigidbody target;
    public Text speedUI;
    private string currentSpeed;

    // Update is called once per frame

    private void Start()
    {
        InvokeRepeating("UpdateSpeed", .2f, .2f);
        //StartCoroutine(speedUpdate());
    }
    //private IEnumerator speedUpdate()
    //{
    //    while (Application.IsPlaying(target))
    //    {
    //        Vector3 prevPos = target.position;
    //        yield return new WaitForEndOfFrame();
    //        Vector3 newPos = target.position;
    //        Vector3 deltaPos = prevPos - newPos;
    //        double currVel = (deltaPos).magnitude / Time.deltaTime;
    //        double cutVel = System.Math.Round(currVel, 0);
    //        currentSpeed = cutVel.ToString();            
    //        Debug.Log(currVel);

    //    }
    //}
    private void UpdateSpeed()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb = target;
        double velocity = rb.velocity.magnitude * 3.6;
        double cutFloat = System.Math.Round(velocity, 0);

        speedUI.text =  cutFloat.ToString();



    }
    
}
