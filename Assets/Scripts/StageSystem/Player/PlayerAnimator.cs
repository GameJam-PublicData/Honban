using System.Collections;
using UnityEngine;

namespace StageSystem.Player
{
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] float animationFrameRate = 0.12f;

    [SerializeField] Sprite[] idleSprite;
    [SerializeField] Sprite[] walkSprite;
    [SerializeField] Sprite[] jumpSprite;
    [SerializeField] Sprite[] jumpFallSprite;
    [SerializeField] Sprite[] drawSprite;

    PlayerAnimationState _animationState = PlayerAnimationState.Idle;
    int _nowAnimationFrame = 0;

    bool _isDraw = false;
    bool _isJump = false;
    bool _isFall = false;
    bool _isWalk = false;
    
    [SerializeField]
    SpriteRenderer spriteRenderer;
    
    //アニメーション優先度　上から順
    //1.書いている時
    //2.ジャンプしている時
    //3.落ちている時
    //4.歩いている時
    //5.止まっている時
    

    void Start()
    {
        StartCoroutine(Animate());
    }

    void UpdateAnimationStateByPriority()
    {
        // Draw中は外部ロジックを優先し、ここでは遷移させない
        if (_animationState == PlayerAnimationState.Draw) return;

        PlayerAnimationState nextState;
        if (_isJump)
        {
            nextState = PlayerAnimationState.Jump;
        }
        else if (_isFall)
        {
            nextState = PlayerAnimationState.JumpFall;
        }
        else if (_isWalk)
        {
            nextState = PlayerAnimationState.Walk;
        }
        else
        {
            nextState = PlayerAnimationState.Idle;
        }

        if (_animationState != nextState)
        {
            _animationState = nextState;
            _nowAnimationFrame = 0;
        }
    }

    public void Falling()
    {
        if(_animationState == PlayerAnimationState.Draw ||
           _animationState == PlayerAnimationState.Jump) return;
        _isFall = true;
        UpdateAnimationStateByPriority();
    }

    public void FallEnd()
    {
        _isFall = false;
        UpdateAnimationStateByPriority();
    }

    public void WalkStart()
    {
        if(_isWalk) return;
        _isWalk = true;
        UpdateAnimationStateByPriority();
    }

    public void WalkEnd()
    {
        if(!_isWalk) return;
        _isWalk = false;
        UpdateAnimationStateByPriority();
    }

    public void JumpStart()
    {
        _isJump = true;
        if(_animationState == PlayerAnimationState.Draw) return;
        UpdateAnimationStateByPriority();
    }

    void JumpEnd()
    {
        _isJump = false;
        if(_animationState == PlayerAnimationState.Draw) return;
        UpdateAnimationStateByPriority();
    }
    
    IEnumerator Animate()
    {
        yield return new WaitForSeconds(animationFrameRate);
        switch (_animationState)
        {
            case PlayerAnimationState.Idle:
                spriteRenderer.sprite = idleSprite[_nowAnimationFrame];
                
                if (_nowAnimationFrame == idleSprite.Length - 1) {
                    _nowAnimationFrame = 0; }
                
                else {
                    _nowAnimationFrame++; }
                
                break;
            case PlayerAnimationState.Walk:
                spriteRenderer.sprite = walkSprite[_nowAnimationFrame];
                
                if (_nowAnimationFrame == walkSprite.Length - 1) {
                    _nowAnimationFrame = 0; }
                
                else {
                    _nowAnimationFrame++; }
                
                break;
            case PlayerAnimationState.Jump:
                
                spriteRenderer.sprite = jumpSprite[_nowAnimationFrame];
                
                if(_nowAnimationFrame == jumpSprite.Length - 1) {
                    _nowAnimationFrame = 0; 
                    JumpEnd();
                }
                
                else {
                    _nowAnimationFrame++; }
                
                break;
            case PlayerAnimationState.JumpFall:
                spriteRenderer.sprite = jumpFallSprite[_nowAnimationFrame];
                
                if(_nowAnimationFrame == jumpFallSprite.Length - 1) {
                    _nowAnimationFrame = 0; }
                
                else {
                    _nowAnimationFrame++; }
                
                break;
            case PlayerAnimationState.Draw:
                spriteRenderer.sprite = drawSprite[_nowAnimationFrame];
                
                if(_nowAnimationFrame == drawSprite.Length - 1) {
                    _nowAnimationFrame = 0; }
                
                else {
                    _nowAnimationFrame++; }
                
                break;
        }
        
        StartCoroutine(Animate());
    }
}

public enum PlayerAnimationState
{
    Idle,
    Walk,
    Jump,
    JumpFall,
    Draw
}
}
