using UnityEngine;

public class HomingDetector : MonoBehaviour
{
    public GameObject target;
    [SerializeField] HomingBullet bullet;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target=other.gameObject;
            bullet.FollowTarget(target);

        }
        else
        {
            target = null;
        }
    }
}
