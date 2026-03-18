using UnityEngine;

namespace StageSystem.Camera
{
public class FollowObject : MonoBehaviour
{
    void Start()
    {
        CameraPort.Instance.SetCameraFollow(transform);
    }
}
}
