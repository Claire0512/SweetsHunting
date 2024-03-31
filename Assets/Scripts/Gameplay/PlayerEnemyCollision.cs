using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;
        //public bool isDying = false;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;


            if (willHurtEnemy)
            {
                AudioSource.PlayClipAtPoint(enemy.ouch, enemy.transform.position);
                var enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Decrement();
                    if (!enemyHealth.IsAlive)
                    {
                        Schedule<EnemyDeath>().enemy = enemy;
                        player.Bounce(2);
                    }
                    else
                    {
                        player.Bounce(7);
                    }
                }
                else
                {
                    Schedule<EnemyDeath>().enemy = enemy;
                    player.Bounce(2);
                }
            }
            else 
            {
               // Debug.Log($"player.isDying: {player.isDying}");
                //player.transform.position = new Vector3(3.48f, -0.5396699f, 1);
                //player.UpdateLifeText();
                if (!player.isDying)
                {
                    player.isDying = true;
                    Schedule<PlayerDeath>();

                }
                //isDying = false;
            }
        }
    }
}