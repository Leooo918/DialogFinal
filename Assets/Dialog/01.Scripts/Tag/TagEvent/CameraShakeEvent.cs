using Dialog.Events.Cinemachine;
using Dialog.Tag;
using UnityEngine;

namespace Dialog.Tag
{
    public class CameraShakeEvent : EventTextTag
    {
        public float shakePower;
        public float shakeDuration;

        protected override void PlayEvent()
        {
            CameraManager.Instance.ShakeCamera(shakePower, shakeDuration);
        }

        public override TextTag Instantiate()
        {
            CameraShakeEvent cameraShakeEvent = new CameraShakeEvent();
            cameraShakeEvent.shakePower = shakePower;
            cameraShakeEvent.shakeDuration = shakeDuration;
            cameraShakeEvent.calcurateEndIndex = false;

            return cameraShakeEvent;
        }

    }
}
