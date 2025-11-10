using System;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using MEC;
using PatchMaster.SCP2176.DoorHitCollider.Components;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace PatchMaster.SCP2176.DoorHitCollider
{
    public class Collider2176DisableZone : MonoBehaviour
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
            
            Log.SendRaw("[PatchMaster] " + "DoorHitCollider2176: Patch assigned", ConsoleColor.Green);
        }
        
        public void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            // Добавление нового обработчика для SCP2176 (Наследован от PatchMaster)
            ev.Projectile.GameObject
                .AddComponent<Collider2176DisableZone>();

            // Добавление компонента IProjectileItem (Наследован от PatchMaster)
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

            if (!isActive && iDoor.Door.IsOpen)
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