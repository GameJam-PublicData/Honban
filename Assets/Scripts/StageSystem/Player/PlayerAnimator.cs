using System;
using System.Collections;
using Cysharp.Threading.Tasks;
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
    [SerializeField] Sprite[] damageSprite;

    PlayerAnimationState _animationState = PlayerAnimationState.Idle;
    int _nowAnimationFrame = 0;

    public bool AnimationActive;
    
    
    bool _isDraw = false;
    bool _isJump = false;
    bool _isFall = false;
    bool _isWalk = false;
    bool _isDamage = false;
    
    [SerializeField]
    SpriteRenderer spriteRenderer;

    public static PlayerAnimator Instance;
    
    //アニメーション優先度　上から順
    //1.書いている時
    //2.ジャンプしている時
    //3.落ちている時
    //4.歩いている時
    //5.止まっている時
    

    void Start()
    {
        Animate().Forget();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("PlayerAnimatorのインスタンスが複数存在しています。");
            Destroy(this);
        }
      
    }

    void UpdateAnimationStateByPriority()
    {
        PlayerAnimationState nextState;
        
        if(_isDamage)
        {
            nextState = PlayerAnimationState.Damage;
        }
        else if(_isDraw)
        {
            nextState = PlayerAnimationState.Draw;
        }
        else if (_isJump)
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

    public void Damage()
    {
        _isDamage = true;
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

    public void DrawStart()
    {
        if(_isDraw) return;
        _isDraw = true;
        
        Debug.Log("DrawStart");
        
        _animationState = PlayerAnimationState.Draw;
        _nowAnimationFrame = 0;
    }

    public void DrawEnd()
    {
        if(!_isDraw) return;
        _isDraw = false;
        
        UpdateAnimationStateByPriority();
    }
    
    async UniTask Animate()
    {
        while (destroyCancellationToken.IsCancellationRequested == false)
        {
            await UniTask.WaitUntil(() => AnimationActive, cancellationToken: destroyCancellationToken);
            await UniTask.Delay(TimeSpan.FromSeconds(animationFrameRate),cancellationToken: destroyCancellationToken);
            

            Sprite[] frames = null;
            bool endJumpOnLastFrame = false;

            switch (_animationState)
            {
                case PlayerAnimationState.Idle:
                    frames = idleSprite;
                    break;
                case PlayerAnimationState.Walk:
                    frames = walkSprite;
                    break;
                case PlayerAnimationState.Jump:
                    frames = jumpSprite;
                    endJumpOnLastFrame = true;
                    break;
                case PlayerAnimationState.JumpFall:
                    frames = jumpFallSprite;
                    break;
                case PlayerAnimationState.Draw:
                    frames = drawSprite;
                    break;
                case PlayerAnimationState.Damage:
                    frames = damageSprite;
                    break;
            }

            if (frames == null || frames.Length == 0)
            {
                continue;
            }

            if (_nowAnimationFrame < 0 || _nowAnimationFrame >= frames.Length)
            {
                _nowAnimationFrame = 0;
            }

            spriteRenderer.sprite = frames[_nowAnimationFrame];

            bool isLastFrame = (_nowAnimationFrame == frames.Length - 1);
            if (isLastFrame)
            {
                _nowAnimationFrame = 0;
                if (endJumpOnLastFrame)
                {
                    JumpEnd();
                }
                if(_animationState == PlayerAnimationState.Damage && _isDamage)
                {
                    _isDamage = false;
                }
            }
            else
            {
                _nowAnimationFrame++;
            }
        }
    }
}

public enum PlayerAnimationState
{
    Idle,
    Walk,
    Jump,
    JumpFall,
    Draw,
    Damage
}
}
