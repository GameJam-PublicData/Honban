using UnityEngine;

namespace StageSystem.Player
{ 
    public interface IPlayerDamage
    {
        public void TakeDamage(int damage);
    }

public interface IPlayerHP : IPlayerDamage
{
    public void Heal(int heal);
    public void SetHP(int hp);
}
}
