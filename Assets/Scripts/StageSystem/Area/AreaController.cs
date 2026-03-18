using System;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemActions;
using StageSystem.Ink;
using StageSystem.Player;
using VContainer.Unity;

namespace StageSystem.Area
{
public interface IAreaController
{
    void SetInputActive(bool active);
}
public class AreaController : IPostStartable, IDisposable,IAreaController
{
    UnityEngine.Camera _mainCamera;
    IStrokeBuilder _strokeBuilder;
    InputActions _inputActions;
    CancellationTokenSource _drawingCts;
    IInkManager _inkManager;
    ICursorTrail _cursorTrail;
    IInkAmount _inkAmount;

    public AreaController(IStrokeBuilder strokeBuilder, IInkManager inkManager, ICursorTrail cursorTrail, IInkAmount inkAmount)
    {
        _strokeBuilder = strokeBuilder;
        _inkManager = inkManager;
        _cursorTrail = cursorTrail;
        _inkAmount = inkAmount;
    }

    public void PostStart()
    {
        Debug.Log("Initialize");

        _mainCamera = UnityEngine.Camera.main;

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
        _drawingCts = new CancellationTokenSource();
        BeginDrawing(_drawingCts.Token).Forget();

        if (PlayerAnimator.Instance != null)
        {
            PlayerAnimator.Instance.DrawStart();
        }
    }

    void OnAttackCanceled(InputAction.CallbackContext ctx) => CancelDrawing();

    void CancelDrawing()
    {
        Debug.Log("CancelDrawing");

        _drawingCts?.Cancel();
        _drawingCts?.Dispose();
        _drawingCts = null;

        _cursorTrail.FadeOut();


        if (PlayerAnimator.Instance != null)
        {
            PlayerAnimator.Instance.DrawEnd();
        }
    }

    async UniTaskVoid BeginDrawing(CancellationToken token)
    {
        Debug.Log("BeginDrawing");

        _strokeBuilder.Clear();
        if (UnityEngine.Camera.main == null)
        {
            Debug.LogError("Camera not found");
            return;
        }

        while (!token.IsCancellationRequested)
        {
            // 現在のインク量から使用できるかの判別
            if (!_inkAmount.IsInkAvailable()) CancelDrawing();

            // 画面上のマウス位置をワールド座標に変換
            var screenPos = Mouse.current.position.ReadValue();
            var worldPos = (Vector2)_mainCamera.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, Mathf.Abs(_mainCamera.transform.position.z)));

            // 直前で記録したポイントとの距離を計算しmin値以上か判別
            if (!_strokeBuilder.DistanceCheck(worldPos))
            {
                bool isCrossing = _strokeBuilder.IsCrossing(worldPos, out var points);

                _inkAmount.ConsumeInk();
                _cursorTrail.Draw(points);

                // 交差しているか
                if (isCrossing)
                {
                    OnCrossed(points);
                    break;
                }
            }

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    void OnCrossed(List<Vector2> points)
    {
        Debug.Log($"交差時ポイント数: {points.Count}");
        _cursorTrail.FadeOut();
        _inkManager.CreateInkArea(points);
        _inkAmount.IsCompleteArea();
    }

    public void SetInputActive(bool active)
    {
        if (active)
        {
            _inputActions.Player.Enable();
        }
        else
        {
            _inputActions.Player.Disable();
            CancelDrawing();
        }
    }
}
}