using UnityEngine;

namespace StageSystem.Player
{
    public class PlayerHpManager : MonoBehaviour,IPlayerDamage
    {
        [SerializeField] public int damage;
        [SerializeField] public int _hp;
        private IPlayerDamage _playerHpImplementation;
        
        public void TakeDamage(int damage)
        {
            _hp -= damage;
        }
    }

}
