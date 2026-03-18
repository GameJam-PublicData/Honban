using StageSystem.CheckPoint;
using StageSystem.UI;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{
public class PlayerHpManager : MonoBehaviour, IPlayerHP
{
    ICheckPointManager  _checkPointManager;
    IClearUIManager  _clearUIManager;
    [SerializeField] int hp = 10;//残機

    [Inject]
    public void Construct(ICheckPointManager  checkPointManager,IClearUIManager  clearUIManager)
    { 
        _checkPointManager = checkPointManager;
        _clearUIManager = clearUIManager;
    }
     
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Debug.Log("ゲームオーバー");
            _clearUIManager.Initialize(false, gameObject);
            return;
        }
        _checkPointManager.MoveCheckPoint(transform);
    }

    public void Heal(int heal)
    {
        hp += heal;
    }

    public void SetHP(int hp)
    {
        this.hp = hp;
    }
}
}
