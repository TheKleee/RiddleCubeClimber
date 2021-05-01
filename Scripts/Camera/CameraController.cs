using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public bool moveCamera;     //Set to true to move the camera!
    [HideInInspector] public Transform target;
    private Vector3 offsetPosition = new Vector3(8, 6, -12);

    private void LateUpdate()
    {
        if (moveCamera)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offsetPosition, 4 * Time.deltaTime); //Test...
        }
    }
    public void SetMoveCam()
    {
        Timing.RunCoroutine(_SetMoveCamera().CancelWith(gameObject));
    }

    public IEnumerator<float> _SetMoveCamera()
    {
        moveCamera = true;
        yield return Timing.WaitForSeconds(1.5f);
        transform.position = target.position + offsetPosition;
        moveCamera = false;
    }
}
