using UnityEngine;

namespace StageSystem.Player
{
    public class PlayerHpManager : MonoBehaviour,IPlayerHP
    {
        [SerializeField] int damage;
        [SerializeField] int hp;
        IPlayerDamage _playerHpImplementation;
        
        public void TakeDamage(int damage)
        {
            hp -= damage;
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
