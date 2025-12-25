using System;
using Exiled.API.Features;
using PatchMaster.Marshmallow;
using PatchMaster.Projectiles;
using UnityEngine;
using Object = UnityEngine.Object;
using events = Exiled.Events.Handlers;

namespace PatchMaster
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        public static GameObject MonobehaviorGameObject;

        public override string Author => "Morkamo";
        public override string Name => "PatchMaster";
        public override string Prefix => Name;
        public override Version Version => new Version(1, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 1, 0);

        private ProjectileCollisionSafeZone _projectileCollisionSafeZone;
        private MarshmallowHandler _marshmallowHandler;

        private void Init()
        {
            MonobehaviorGameObject = new GameObject("MonobehaviorGameObject");
            Object.DontDestroyOnLoad(MonobehaviorGameObject);
            _projectileCollisionSafeZone = MonobehaviorGameObject.AddComponent<ProjectileCollisionSafeZone>();
            
            _marshmallowHandler = new MarshmallowHandler();
        }

        private void DeInit()
        {
            _projectileCollisionSafeZone = null;
            MonobehaviorGameObject = null;
            
            _marshmallowHandler = null;
        }

        private void RegisterEvents()
        {
            events.Server.WaitingForPlayers += _projectileCollisionSafeZone.OnWaitingForPlayers;
            events.Player.ThrownProjectile += _projectileCollisionSafeZone.OnThrownProjectile;
            events.Player.ItemAdded += _marshmallowHandler.OnGetMarshmallowItem;
        }

        private void UnRegisterEvents()
        {
            events.Server.WaitingForPlayers -= _projectileCollisionSafeZone.OnWaitingForPlayers;
            events.Player.ThrownProjectile -= _projectileCollisionSafeZone.OnThrownProjectile;
            events.Player.ItemAdded -= _marshmallowHandler.OnGetMarshmallowItem;
        }
        
        public override void OnEnabled()
        {
            Instance = this;
            Init();
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnRegisterEvents();
            DeInit();
            Instance = null;
            base.OnDisabled();
        }
    }
}