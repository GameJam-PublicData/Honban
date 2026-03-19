using System;
using Cysharp.Threading.Tasks;
using InputSystemActions;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace StageSystem.Ink
{
public interface IInkAmount
{
    float Ink { get; }
    bool IsInkAvailable();
    void RecoverInk();
    void ConsumeInk();
    void IsCompleteArea();
}
public class InkAmount : MonoBehaviour,IInkAmount
{
    [SerializeField] float recoverInkUsageSecond = 10f;
    [SerializeField] float consumeInkUsage = 1f;
    [SerializeField] float recoverInkAmount = 5f;
    [SerializeField] float notRecoverTime = 5f;
    
    [Inject]
    public void Construct(ICurrentInkEffect currentInkEffect)
    {
        currentInkEffect.Get.Subscribe(a).AddTo(this);
    }

    void a(IInkEffect aas)
    {
        if(aas == null) return;
        consumeInkUsage = aas.EffectUsageRate;
    }
    
    bool _isHolding;
    InputActions  _inputActions;

    public float Ink { get; private set; } = 100;
    const float MaxInk = 100;
    public bool IsInkAvailable() => Ink > 0;
    
    public void RecoverInk()
    {
        if (Ink >= MaxInk || _isHolding) return;
        float completeRecoveryRate = _isNotComplete ? recoverInkAmount : 1; 
        Ink += recoverInkUsageSecond * Time.deltaTime * completeRecoveryRate;
        if(Ink > MaxInk) Ink = MaxInk;
    }

    public void ConsumeInk()
    {
        if (Ink <= 0) return;
        Ink -= consumeInkUsage;
        if(Ink < 0) Ink = 0;
        if(_isNotComplete == false && _isNotRecoverTime == false) _isNotComplete  = true;
    }

    [SerializeField]bool _isNotComplete = true;//円を描き切れたか
    bool _isNotRecoverTime;
    public void IsCompleteArea()
    {
        _isNotComplete = false;
        _isNotRecoverTime = true;
        UniTask.Delay(TimeSpan.FromSeconds(notRecoverTime),cancellationToken:destroyCancellationToken)
            .ContinueWith(() => _isNotRecoverTime = false).Forget();
    }

    void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Attack.started += OnClickStart;
        _inputActions.Player.Attack.canceled += OnClickEnd;
    }

    void OnClickStart(InputAction.CallbackContext context)
    { 
        _isHolding = true;
    }

    void OnClickEnd(InputAction.CallbackContext context)
    {
        _isHolding = false;
    }

    void OnDisable()
    {
        _inputActions.Player.Attack.started -= OnClickStart;
        _inputActions.Player.Attack.canceled -= OnClickEnd;
        _inputActions.Disable();
    }
}
}