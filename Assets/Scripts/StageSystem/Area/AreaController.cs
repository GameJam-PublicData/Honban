using System;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemActions;
using StageSystem.Ink;
using StageSystem.UI.Mouse;
using VContainer.Unity;

namespace StageSystem.Area
{
    public class AreaController : IPostStartable, IDisposable
    {
        Camera _mainCamera;
        IStrokeBuilder _strokeBuilder;
        InputActions _inputActions;
        CancellationTokenSource _drawingCts;
        IInkManager _inkManager;
        ICursorTrail _cursorTrail;
        
        public AreaController(IStrokeBuilder strokeBuilder, IInkManager inkManager, ICursorTrail cursorTrail)
        {
            Debug.Log("AreaController");
            
            _strokeBuilder = strokeBuilder;
            _inkManager = inkManager;
            _cursorTrail = cursorTrail;
        }
        
        public void PostStart()
        {
            Debug.Log("Initialize");
            
            _mainCamera = Camera.main;
            
            _inputActions = new InputActions();
            _inputActions.Enable();
            
            _inputActions.Player.Attack.started += OnAttackStarted;
            _inputActions.Player.Attack.canceled += OnAttackCanceled;
        }

        public void Dispose()
        {
            Debug.Log("Dispose");
            
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

                if (_strokeBuilder.IsCrossing(worldPos, _cursorTrail.Draw, out var points))
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