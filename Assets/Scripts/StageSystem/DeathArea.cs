using StageSystem.Player;
using UnityEngine;

namespace StageSystem
{
public class DeathArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ダメージ");
            other.GetComponent<IPlayerDamage>().TakeDamage(1);
        }
    }
}
}
  
  
