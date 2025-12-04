using Exiled.API.Features.Doors;
using UnityEngine;

namespace PatchMaster.Projectiles.DoorHitCollider.Components
{
    public class InteractableDoor : MonoBehaviour
    {
        public Door Door { get; private set; }
        
        public void Init(Door door, bool makeInteractiveTrigger = true)
        {
            Door = door;
            
            if (makeInteractiveTrigger)
            {
                // Колайдер для стабилизации пролёта SCP2176 через дверную рамму. (Фикс бага)
                GameObject colliderObject = new GameObject("DoorTriggerZone");
                colliderObject.transform.position = door.Position;
                colliderObject.transform.localPosition += new Vector3(0, 1.5f, 0);
                colliderObject.transform.localScale = new Vector3(1.6f, 2f, 2f);
                colliderObject.transform.rotation = door.Rotation;

                BoxCollider triggerCollider = colliderObject.AddComponent<BoxCollider>();
                triggerCollider.isTrigger = true;

                colliderObject.transform.SetParent(Door.Transform);
            }
        }
    }
}