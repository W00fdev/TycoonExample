
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    This file has a commented version with details about how each line works. 
    The commented version contains code that is easier and simpler to read. This file is minified.
*/

/// <summary>
/// Camera movement script for third person games.
/// This Script should not be applied to the camera! It is attached to an empty object and inside
/// it (as a child object) should be your game's MainCamera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    [Tooltip("Enable to move the camera by holding the right mouse button. Does not work with joysticks.")]
    public bool clickToMoveCamera = false;
    [Tooltip("Enable zoom in/out when scrolling the mouse wheel. Does not work with joysticks.")]
    public bool canZoom = true;
    [Space]
    [Tooltip("The higher it is, the faster the camera moves. It is recommended to increase this value for games that uses joystick.")]
    public float sensitivity = 5f;

    [Tooltip("Camera Y rotation limits. The X axis is the maximum it can go up and the Y axis is the maximum it can go down.")]
    public Vector2 cameraLimit = new Vector2(-45, 40);

    float mouseX;
    float mouseY;
    public float offsetDistanceY;
    public float offsetDistanceX = 0;
    public float distance;

    [SerializeField] private RaycastHit[] _anyCollider;
    [SerializeField] private LayerMask _allExceptPlayer;
    [SerializeField] private float _smoothingStep;
    Transform player;

    private float _offsetZ;

    void Start()
    {
        _anyCollider = new RaycastHit[1];
        
        player = GameObject.FindWithTag("Player").transform;
        //offsetDistanceX = transform.position.x - player.position.x;

        // Lock and hide cursor with option isn't checked
        /*if ( ! clickToMoveCamera )
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }*/
    }

    void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        _camera.transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);

        var position = _camera.transform.rotation * new Vector3(0f, 0, -distance);
        position += player.position;
        position += Vector3.up * offsetDistanceY;
        
        Ray backSpaceCamera = new Ray(player.position, -_camera.transform.forward);
        float distanceToCamera = Vector3.Distance(player.position, _camera.transform.position);
        float distanceToWall = distanceToCamera;
        
        if (Physics.RaycastNonAlloc(backSpaceCamera, _anyCollider, distanceToCamera + 0.1f, _allExceptPlayer.value) > 0)
            distanceToWall = Vector3.Distance(player.position, _anyCollider[0].point);

        _offsetZ = Mathf.SmoothStep(_offsetZ, Mathf.Abs(distanceToWall - distanceToCamera), _smoothingStep * Time.deltaTime);
        if (distanceToWall <= 10f)
            position += _camera.transform.forward * _offsetZ;
        
        _camera.transform.position = position;
    }
}