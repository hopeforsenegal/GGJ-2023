using System.Collections.Generic;
using UnityEngine;

public class TestCameraBasedOnPlayer : MonoBehaviour
{
    public Camera mainCamera;
    public BoxCollider2D player;
    public CameraTransitionSquare[] cameraEndLocationTransforms;

    private const float LerpDuration = 0.5f;
    float timeElapsed;
    private Vector3 endMarkerPos;
    private Vector3 startMarkerPos;
    public float speed = 1;

    Dictionary<CameraTransitionSquare, CameraState> cameraState = new Dictionary<CameraTransitionSquare, CameraState>();
    
    void Start()
    {
        mainCamera = Camera.main;
        cameraEndLocationTransforms = GameManager.GetCameraTransitionSquares();
        foreach (var cT in cameraEndLocationTransforms) {
            cameraState.Add(cT, new CameraState());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
            player.transform.localPosition += Vector3.left * speed;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
            player.transform.localPosition += Vector3.right * speed;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
            player.transform.localPosition += Vector3.down * speed;
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
            player.transform.localPosition += Vector3.up * speed;
        }

        GameManager.InterpolateActiveCamera(mainCamera.transform, cameraState, ref timeElapsed, LerpDuration, startMarkerPos, endMarkerPos);
    }
}
