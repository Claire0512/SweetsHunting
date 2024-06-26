﻿using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        private bool isDying = false;

        IEnumerator ResetDeathFlag()
        {
            yield return new WaitForSeconds(1); // Adjust the time as needed
            isDying = false;
        }
        public override void Execute()
        {
            if (isDying) return;

            isDying = true;
            var player = model.player;
            player.life -= 1;
            player.UpdateLifeText();
            if (player.health.IsAlive)
            {

                player.health.Die();
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                player.controlEnabled = false;

                if (player.audioSource && player.ouchAudio)
                    player.audioSource.PlayOneShot(player.ouchAudio);
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
            if (CoroutineManager.Instance != null)
            {
                CoroutineManager.Instance.StartManagedCoroutine(ResetDeathFlag());
            }
            else
            {
                Debug.LogError("CoroutineManager.Instance is null!");
            }

        }

    }
}