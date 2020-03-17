using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler
{

  private Rigidbody rBody;
  private Vector3 resetPosition;
  private Quaternion resetAngle;

  public CollisionHandler(Rigidbody rBody, Vector3 resetPosition, Quaternion resetAngle)
  {
    this.rBody = rBody;
    this.resetPosition = resetPosition;
    this.resetAngle = resetAngle;
  }

  public void ResetCar()
  {
    rBody.transform.localPosition = resetPosition;
    rBody.angularVelocity = Vector3.zero;
    rBody.velocity = Vector3.zero;
    rBody.transform.rotation = resetAngle;
  }
}
