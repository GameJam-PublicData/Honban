using System.Collections.Generic;
using StageSystem.Player;
using UnityEngine;

namespace StageSystem.Ink.Inks
{
    public class LowGravityEffect : IInkEffect
    {
        float _speedUpMultiplier = 3.5f;
        float _gravityDownMultiplier = 0.35f;
        
        public string MaterialName => "SpeedUpInkMaterial";
        public float EffectUsageRate => 1;
        
        public void UpdateInkArea(Rigidbody2D body)
        {
           
        }

        public void StartInkArea(Rigidbody2D body)
        {
            if (body.gravityScale > 0f)
            {
                body.gravityScale = _gravityDownMultiplier;
            }
            else if (body.gravityScale < 0f)
            {
                body.gravityScale = -_gravityDownMultiplier;
            }

            var player = body.GetComponent<PlayerController>();
            if (player == null) return;
            player.MultiplySpeedUpMultiplier(_speedUpMultiplier);
            
            var playerJump = body.GetComponent<PlayerJump>();
            if (playerJump == null) return;
            playerJump.SetJumpPower(_speedUpMultiplier);
        }

        public void StopInkArea(Rigidbody2D rigidbody)
        {
            if (rigidbody.gravityScale > 0f)
            {
                rigidbody.gravityScale = 1;
            }
            else if (rigidbody.gravityScale < 0f)
            {
                rigidbody.gravityScale = -1;
            }
            
            var player = rigidbody.GetComponent<PlayerController>();
            if(player == null) return;
            player.RestoreSpeedUpMultiplier(Vector2.one);
            
            var playerJump = rigidbody.GetComponent<PlayerJump>();
            if (playerJump == null) return;
            playerJump.InitJumpPower();
        }


    }
}
