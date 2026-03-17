using InputSystemActions;
using SyskenTLib.LicenseMaster;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainMenu
{
public class LicenseAdapter : MonoBehaviour
{
    [Header("更新するにはResetしてください")]
    [SerializeField] LicenseManager licenseManager;
    [SerializeField] TextMeshProUGUI licenseText;
    [SerializeField] float moveSpeed = 1500f;
    [SerializeField] float upperMargin = 400f;
    [SerializeField] float bottomMargin = 0f;

    InputActions _inputActions;
    
    float _yVelocity;
    float _bottomLimitY;
    float _upperLimitY;
    
#if UNITY_EDITOR
    //LicenseOnlyShowRootConfigをUpdateして全ての表示する必要があるライセンスListを取得する
    void Reset()
    {
        var type = typeof(SyskenTLib.LicenseMasterEditor.ManageLicenseWindow);
        var method = type.GetMethod("OutPutFile",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        method?.Invoke(null, new object[] { });

        licenseManager = UnityEditor.AssetDatabase.LoadAssetAtPath<LicenseManager>(
            "Assets/Plugins/LicenseMaster/Prefabs/LicenseManager.prefab"
        );
        licenseText = GetComponentInChildren<TextMeshProUGUI>();

        licenseText.text = licenseManager.GetLicenseConfigsTxt();
    }
#endif

    void Start()
    {
        licenseText.text = licenseManager.GetLicenseConfigsTxt();
    }
    
    void OnEnable()
    {
        _inputActions ??= new InputActions();
        _inputActions.UI.Enable();
        _inputActions.UI.Navigate.performed += RequestMoveText;
        licenseText.transform.localPosition = Vector3.zero;

        _inputActions.UI.Click.performed += EndLicenceView;
        _inputActions.UI.Cancel.started += EndLicenceView;
        _inputActions.UI.Submit.started += EndLicenceView;
        _inputActions.Player.Jump.Enable();
        _inputActions.Player.Jump.started += EndLicenceView;
        
        
        SetMoveBounds();
    }
    
    void EndLicenceView(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }
    
    
    void SetMoveBounds()
    {
        licenseText.ForceMeshUpdate();
        Bounds bounds = licenseText.textBounds;
        float topWorldY = bounds.max.y;
        float bottomWorldY = bounds.min.y;
        _upperLimitY = transform.InverseTransformPoint(new Vector3(0, topWorldY, 0)).y * -1;
        _bottomLimitY = transform.InverseTransformPoint(new Vector3(0, bottomWorldY, 0)).y * -1;
        
        _upperLimitY -= upperMargin;
        _bottomLimitY += bottomMargin;
        
        Debug.Log($"License Text Move Bounds Set: upperY={_upperLimitY}, bottomY={_bottomLimitY}");
    }
    
    
    void RequestMoveText(InputAction.CallbackContext context)
    {
        _yVelocity = context.ReadValue<Vector2>().y * -1;
        Debug.Log($"Request Move License Text: velocityY={_yVelocity}");
    }

    void Update()
    {
        Vector3 pos = licenseText.transform.localPosition;
        pos.y += _yVelocity * moveSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y,_upperLimitY, _bottomLimitY);
        licenseText.transform.localPosition = pos;
    }


    void OnDisable()
    {
        _inputActions.UI.Navigate.performed -= RequestMoveText;
        licenseText.transform.localPosition = new Vector3(0,_upperLimitY,0);
        
        _inputActions.UI.Click.performed -= EndLicenceView;
        _inputActions.UI.Cancel.started -= EndLicenceView;
        _inputActions.UI.Submit.started -= EndLicenceView;
        _inputActions.Player.Jump.started -= EndLicenceView;
        
        _inputActions.UI.Disable();
        _inputActions.Player.Jump.Disable();
    }
    
    
    
}
}