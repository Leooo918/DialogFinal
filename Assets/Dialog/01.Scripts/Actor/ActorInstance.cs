using System;
using UnityEngine;

namespace Dialog
{
    public class ActorInstance : MonoBehaviour
    {
        [SerializeField] private Actor _actor;

        private void OnEnable()
        {
            DialogActorManager.AddActor(_actor.actorInfo.actorName, _actor);
        }

        private void OnDisable()
        {
            DialogActorManager.RemoveActor(_actor.actorInfo.actorName, _actor);
        }
    }
}
