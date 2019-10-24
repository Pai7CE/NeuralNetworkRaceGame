using UnityEngine;
using UnityEngine.UI;

public class GUISpeed : MonoBehaviour
{

    public Rigidbody target;
    public Text speedText;

    // Update is called once per frame

    private void Start()
    {
        InvokeRepeating("UpdateSpeed", .2f, .2f);
    }
    private void UpdateSpeed()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb = target;
        double velocity = rb.velocity.magnitude * 3.6;
        double cutFloat = System.Math.Round(velocity, 2);
        speedText.text = "Speed: " + cutFloat;
    }
}
