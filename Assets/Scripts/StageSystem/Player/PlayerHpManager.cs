using UnityEngine;

namespace StageSystem.Player
{
    public class PlayerHpManager : MonoBehaviour,IPlayerHP
{
    [SerializeField] int hp = 1;
        
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
