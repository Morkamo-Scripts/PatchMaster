using System;
using Exiled.API.Features;
using PatchMaster.SCP2176.DoorHitCollider;
using UnityEngine;
using Object = UnityEngine.Object;
using evArgsServer = Exiled.Events.Handlers.Server;
using evArgsMap = Exiled.Events.Handlers.Map;
using evArgs = Exiled.Events.Handlers.Player;

namespace PatchMaster
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        public static GameObject MonobehaviorGameObject;

        public override string Author => "Morkamo";
        public override string Name => "PatchMaster";
        public override string Prefix => Name;
        public override Version Version => new Version(1, 0, 0);

        public Collider2176DisableZone Collider2176DisableZone;

        private void Init()
        {
            MonobehaviorGameObject = new GameObject("MonobehaviorGameObject");
            Object.DontDestroyOnLoad(MonobehaviorGameObject);
            Collider2176DisableZone = MonobehaviorGameObject.AddComponent<Collider2176DisableZone>();
        }

        private void DeInit()
        {
            Collider2176DisableZone = null;
            MonobehaviorGameObject = null;
        }

        private void RegisterEvents()
        {
            evArgsServer.WaitingForPlayers += Collider2176DisableZone.OnWaitingForPlayers;
            evArgs.ThrownProjectile += Collider2176DisableZone.OnThrownProjectile;
        }

        private void UnRegisterEvents()
        {
            evArgsServer.WaitingForPlayers -= Collider2176DisableZone.OnWaitingForPlayers;
            evArgs.ThrownProjectile -= Collider2176DisableZone.OnThrownProjectile;
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