using MainSystem.Scene;
using StageSystem.Area;
using StageSystem.Ink;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{
public interface IActiveHandler
{
    void StopGame();
    void ActiveGame();
    bool GetActive();
}
public class GameActiveHandler : MonoBehaviour ,IActiveHandler
{
    [SerializeField] PlayerController  playerController;
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] PlayerJump playerJump;
    [SerializeField] PlayerAnimator playerAnimator;
    IAreaController _areaController;
    InkSelectManager  _inkSelectManager;
    
    [Inject]
    public void Construct(IAreaController areaController,InkSelectManager  inkSelectManager)
    {
        _areaController = areaController;
        _inkSelectManager = inkSelectManager;
    }

    bool _gameActive = true;

    void Reset()
    {
        playerController = GetComponent<PlayerController>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }
     
    public void StopGame()
    {
        if(!_gameActive) return;
        if(gameObject == false) return; 
        Debug.Log("ゲーム停止");
        _gameActive = false;
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        playerController.enabled = false;
        playerJump.enabled = false;
        _areaController.SetInputActive(false);
        _inkSelectManager.SetInputActive(false);
    }

    public void ActiveGame()
    {
        if(_gameActive) return;
        if(gameObject == false) return; 
        Debug.Log("ゲーム再開");
        _gameActive = true;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        playerController.enabled = true;
        playerJump.enabled = true;
        _areaController.SetInputActive(true);
        _inkSelectManager.SetInputActive(true);
    }

    public bool GetActive() => _gameActive;
}
}