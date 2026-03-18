using TMPro.Examples;
using Unity.Cinemachine;
using UnityEngine;

namespace StageSystem.Camera
{
public class CameraPort : MonoBehaviour
{
    public static CameraPort Instance;
    
    CinemachineCamera brain;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        brain = GetComponent<CinemachineCamera>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void SetCameraFollow(Transform followObject)
    {
        if (brain != null)
        {
            brain.Follow = followObject;
            Debug.Log(brain.Follow);
        }
    }
}
}