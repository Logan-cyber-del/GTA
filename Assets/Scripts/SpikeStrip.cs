using UnityEngine;

namespace ChaosCar
{
    public class SpikeStrip : MonoBehaviour
    {
        public float damagePerHit = 35f;

        private void OnTriggerEnter(Collider other)
        {
            ArcadeCarController car = other.GetComponentInParent<ArcadeCarController>();
            if (car != null)
                car.TakeDamage(damagePerHit);
        }
    }
}
