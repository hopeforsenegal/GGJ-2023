using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Transform transforma;
    public Transform transformb;
    public Transform transformc;


    private const float LerpDuration = 0.5f;
    float timeElapsed;
    private Vector3 endMarkerPos;
    private Vector3 startMarkerPos;

    Dictionary<Transform, CameraState> cameraLocations = new Dictionary<Transform, CameraState>();
    
    void Start()
    {
        cameraLocations.Add(transforma, new CameraState());
        cameraLocations.Add(transformb, new CameraState());
        cameraLocations.Add(transformc, new CameraState());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(transforma, mainCamera.transform, cameraLocations);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(transformb, mainCamera.transform, cameraLocations);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(transformc, mainCamera.transform, cameraLocations);
        }

        GameManager.InterpolateActiveCamera(mainCamera.transform, cameraLocations, ref timeElapsed, LerpDuration, startMarkerPos, endMarkerPos);
    }
}
