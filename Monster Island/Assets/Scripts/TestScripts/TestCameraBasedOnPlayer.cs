using System.Collections.Generic;
using System.Linq;
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
            player.transform.localPosition += Vector3.left * speed;
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            player.transform.localPosition += Vector3.right * speed;
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            player.transform.localPosition += Vector3.down * speed;
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            player.transform.localPosition += Vector3.up * speed;
            var (hasCollided, locationInfo) =
                GameManager.WillCollideCameraLocation(player, Vector3.left * speed, cameraEndLocationTransforms);
            if (hasCollided) {
                (startMarkerPos, endMarkerPos) = GameManager.UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
            }
        }

        GameManager.InterpolateActiveCamera(mainCamera.transform, cameraState, ref timeElapsed, LerpDuration, startMarkerPos, endMarkerPos);
    }
}
