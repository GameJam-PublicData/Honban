using System.Collections.Generic;
using StageSystem.Player;
using UnityEngine;

namespace StageSystem.Ink.Inks
{
    public class SpeedUpEffect : IInkEffect
    {
        readonly float _speedUpMultiplier;
        readonly Dictionary<Rigidbody2D, Vector2> _originalVelocities = new();
        
        
        public SpeedUpEffect(float speedUpMultiplier)
        {
            _speedUpMultiplier = Mathf.Max(0f, speedUpMultiplier);
        }
        
        public void UpdateInkArea(Rigidbody2D body)
        {
           
        }

        public void StartInkArea(Rigidbody2D body)
        {
            if (_originalVelocities.ContainsKey(body)) return;
            
            var player = body.GetComponent<PlayerController>();
            if (player == null) return;
            
            _originalVelocities[body] = player.SpeedUpMultiplier;
            player.MultiplySpeedUpMultiplier(_speedUpMultiplier);
        }

        public void StopInkArea(Rigidbody2D rigidbody)
        {
            if (!_originalVelocities.ContainsKey(rigidbody)) return;
            
            var player = rigidbody.GetComponent<PlayerController>();
            if (player == null)
            {
                _originalVelocities.Remove(rigidbody);
                return;
            }

            player.RestoreSpeedUpMultiplier(_originalVelocities[rigidbody]);
            _originalVelocities.Remove(rigidbody);
        }

        public string MaterialName => "SpeedUpInkMaterial";
    }
}
