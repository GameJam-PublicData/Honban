using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemActions;
using StageSystem.Ink;
using VContainer;

namespace StageSystem.Area
{
    public class AreaController : MonoBehaviour
    {
        Camera _mainCamera;
        IStrokeBuilder _strokeBuilder;
        InputActions _inputActions;
        CancellationTokenSource _drawingCts;
        IInkManager _inkManager;

        void Start()
        {
            _strokeBuilder.Health();
            _mainCamera = Camera.main;
        }
        
        [Inject]
        public void Construct(IStrokeBuilder strokeBuilder,IInkManager inkManager)
        {
            _strokeBuilder = strokeBuilder;
            _inkManager = inkManager;
        }

        void OnEnable()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();
            
            _inputActions.Player.Attack.started += OnAttackStarted;
            _inputActions.Player.Attack.canceled += OnAttackCanceled;
        }

        void OnDisable()
        {
            _inputActions.Player.Attack.started -= OnAttackStarted;
            _inputActions.Player.Attack.canceled -= OnAttackCanceled;
            _inputActions.Disable();
            
            CancelDrawing();
        }

        void OnAttackStarted(InputAction.CallbackContext ctx)
        {
            CancelDrawing();
            
            _drawingCts = new CancellationTokenSource();
            BeginDrawing(_drawingCts.Token).Forget();
        }

        void OnAttackCanceled(InputAction.CallbackContext ctx) => CancelDrawing();
        
        void CancelDrawing()
        {
            Debug.Log("CancelDrawing");
            
            _drawingCts?.Cancel();
            _drawingCts?.Dispose();
            _drawingCts = null;
        }

        public async UniTaskVoid BeginDrawing(CancellationToken token)
        {
            Debug.Log("BeginDrawing");
            
            _strokeBuilder.Clear();
            if (Camera.main == null)
            {
                Debug.LogError("Camera not found");
                return;
            }

            while (!token.IsCancellationRequested)
            {
                // 画面上のマウス位置をワールド座標に変換
                var screenPos = Mouse.current.position.ReadValue();
                var worldPos = (Vector2)_mainCamera.ScreenToWorldPoint(
                    new Vector3(screenPos.x, screenPos.y, Mathf.Abs(_mainCamera.transform.position.z)));

                if (_strokeBuilder.IsCrossing(worldPos, out var points))
                {
                    Debug.Log("交差が確認されました");
                    OnCrossed(points);
                    _inkManager.CreateInkArea(points);
                    break;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        void OnCrossed(List<Vector2> points)
        {
            Debug.Log($"交差時ポイント数: {points.Count}");
            // TODO: points を使ってエリア確定処理へ渡す
        }
    }
}