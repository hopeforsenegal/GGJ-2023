using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Transform transforma;
    public Transform transformb;
    public Transform transformc;

    public class TransformState
    {
        public bool IsAnimating;
    }

    Dictionary<Transform, TransformState> cameraLocations = new Dictionary<Transform, TransformState>();
    // Start is called before the first frame update
    void Start()
    {
        cameraLocations.Add(transforma, new TransformState());
        cameraLocations.Add(transformb, new TransformState());
        cameraLocations.Add(transformc, new TransformState());
    }
    public int interpolationFramesCount = 45; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;
    private Vector3 endMarkerPos;
    private Vector3 startMarkerPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("1");
            UpdateAnimation(transforma);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log("2");
            UpdateAnimation(transformb);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Debug.Log("3");
            UpdateAnimation(transformc);
        }
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        foreach (var kv in cameraLocations) {
            if (kv.Value.IsAnimating) {
                Debug.Log("Hey");

                // Just need to use Time.time to calculate the lerp and move the camera
                mainCamera.transform.position = Vector3.Lerp(startMarkerPos, endMarkerPos, interpolationRatio);
            }
        }
    }

    private void UpdateAnimation(Transform transform)
    {
        foreach (var kv in cameraLocations) {
            kv.Value.IsAnimating = false;
        }
        cameraLocations[transform].IsAnimating = true;
        endMarkerPos = transform.position;
        startMarkerPos = mainCamera.transform.position;
    }
}
