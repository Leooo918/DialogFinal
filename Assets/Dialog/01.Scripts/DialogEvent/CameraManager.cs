using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Dialog.Events.Cinemachine
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [SerializeField] private CinemachineCamera _cinemachine;
        private CinemachineBasicMultiChannelPerlin _shaker;
        private Coroutine _cameraShakeRoutine;

        private Transform _originFocus;

        private void Awake()
        {
            _shaker = _cinemachine.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _originFocus = _cinemachine.Follow;
        }

        public void SetFocus(Transform target)
        {
            _cinemachine.Follow = target;
        }

        public void ShakeCamera(float power, float duration)
        {
            if (_cameraShakeRoutine != null) StopCoroutine(_cameraShakeRoutine);
            _cameraShakeRoutine = StartCoroutine(StartShake(power, duration));
        }

        private IEnumerator StartShake(float power, float duration)
        {
            _shaker.AmplitudeGain = power;
            _shaker.FrequencyGain = power;
            yield return new WaitForSecondsRealtime(duration);
            _shaker.AmplitudeGain = 0;
            _shaker.FrequencyGain = 0;
        }
    }
}
