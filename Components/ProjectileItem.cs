using Exiled.API.Features.Pickups.Projectiles;
using UnityEngine;

namespace PatchMaster.Components
{
    public class ProjectileItem : MonoBehaviour
    {
        public Projectile Projectile { get; private set; }
        public void Init(Projectile projectile) => Projectile = projectile;
    }
}