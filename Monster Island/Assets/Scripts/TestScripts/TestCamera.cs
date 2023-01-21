using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Transform[] cameraEndLocationTransforms;


    private const float LerpDuration = 0.5f;
    float timeElapsed;
    private Vector3 endMarkerPos;
    private Vector3 startMarkerPos;

    Dictionary<Transform, CameraState> cameraState = new Dictionary<Transform, CameraState>();
    
    void Start()
    {
        foreach (var cT in cameraEndLocationTransforms) {
            cameraState.Add(cT, new CameraState());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(cameraEndLocationTransforms[0], mainCamera.transform, cameraState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(cameraEndLocationTransforms[1], mainCamera.transform, cameraState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(cameraEndLocationTransforms[2], mainCamera.transform, cameraState);
        }

        GameManager.InterpolateActiveCamera(mainCamera.transform, cameraState, ref timeElapsed, LerpDuration, startMarkerPos, endMarkerPos);
    }
}
