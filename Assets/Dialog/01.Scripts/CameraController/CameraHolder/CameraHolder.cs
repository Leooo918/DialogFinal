using UnityEngine;

namespace CameraControllers.CameraHolders
{

    public class CameraHolder : MonoBehaviour
    {
        [SerializeField] private float _holderRange;
        [SerializeField] private LayerMask _holdPointLayer;
        private CameraManager _cameraManager;

        void Start()
        {
            _cameraManager = CameraManager.Instance;
        }

        private void CheckHoldPoint()
        {
            Collider2D target = Physics2D.OverlapCircle(transform.position, _holderRange, _holdPointLayer);
            if (target != null)
            {
                _cameraManager.SetFollow(target.transform);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _holderRange);
        }
#endif
    }

}