using System;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using MEC;
using PatchMaster.Components;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace PatchMaster.Handlers
{
    public class ProjectileCollisionSafeZone : MonoBehaviour
    {
        public void OnWaitingForPlayers()
        {
            foreach (var door in Door.List)
            {
                var interactableDoor = door.GameObject.AddComponent<InteractableDoor>();
                
                if (interactableDoor != null)
                {
                    if (door.IsElevator || door.IsGate)
                        interactableDoor.Init(door, false);
                    else
                        interactableDoor.Init(door);
                }
            }
            
            Log.SendRaw("[PatchMaster] " + "ProjectileCollisionSafeZone: Patch assigned", ConsoleColor.Green);
        }
        
        public void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            ev.Projectile.GameObject
                .AddComponent<ProjectileCollisionSafeZone>();
            
            var iProjectile = ev.Projectile.GameObject
                .AddComponent<ProjectileItem>();
                
            iProjectile.Init(ev.Projectile);
        }

        private void OnTriggerEnter(Collider other) => TrySwitchCollisionState(other, false);
        private void OnTriggerExit(Collider other) => TrySwitchCollisionState(other, true);

        private void TrySwitchCollisionState(Collider target, bool isActive)
        {
            var iDoor = target.GetComponentInParent<InteractableDoor>();
            var projectile = this.GetComponentInParent<ProjectileItem>();
    
            if (iDoor == null || projectile == null)
                return;

            if (!isActive && (iDoor.Door.IsOpen || iDoor.Door.IsConsideredOpen))
            {
                projectile.Projectile.Rigidbody.detectCollisions = false;
            }
            else if (isActive)
            {
                Timing.CallDelayed(0.05f, () => 
                    { projectile.Projectile.Rigidbody.detectCollisions = true; });
            }
        }
    }
}