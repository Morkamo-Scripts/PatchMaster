using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using UnityEngine;
using Item = Exiled.API.Features.Items.Item;
using Round = Exiled.API.Features.Round;
using Scp1344Item = InventorySystem.Items.Usables.Scp1344.Scp1344Item;

namespace PatchMaster.Marshmallow
{
    public class MarshmallowHandler
    {
        public void OnUsingItemComplete(UsingItemCompletedEventArgs ev)
        {
            /*if (ev.Item.Type == ItemType.Scp021J && !Round.IsEnded)
            {
                if (ev.Player.Items.GetItemTypes().Contains(ItemType.SCP1344))
                {
                    ev.Player.DisableEffect(EffectType.SeveredEyes);
                }
            }*/
        }

        public void OnGetMarshmallowItem(ItemAddedEventArgs ev)
        {
            if (ev.Item.Type == ItemType.MarshmallowItem && !Round.IsEnded)
            {
                var scp1344 = ev.Player.Items.FirstOrDefault(i => i.Type == ItemType.SCP1344);
                if (scp1344 != null)
                {
                    Item.Get(scp1344.Base).CreatePickup(ev.Player.Position).Spawn();
                    
                    ev.Player.RemoveItem(scp1344);
                    
                    ev.Player.DisableEffect(EffectType.Scp1344);
                    ev.Player.DisableEffect(EffectType.Blinded);
                    ev.Player.DisableEffect(EffectType.SeveredEyes);
                }
            }
        }
    }
}