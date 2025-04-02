using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(TrajectoryPredictor))]
public class BowShootController : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float minLaunchForce = 5f;
    [SerializeField] private float maxLaunchForce = 20f;

    [Header("Animation Settings")]
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private AnimationReferenceAsset idleAnimation;
    [SerializeField] private AnimationReferenceAsset targetAnimation;

    private TrajectoryPredictor trajectoryPredictor;
    private float currentForce;
    public bool IsAiming { get; private set; }

    private void Awake()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();
        
        if (trajectoryPredictor == null)
        {
            Debug.LogError("TrajectoryPredictor component is missing!", this);
            enabled = false;
            return;
        }

        if (shotPoint == null)
        {
            Debug.LogError("Shot Point is not assigned!", this);
            enabled = false;
        }

        if (skeletonAnimation == null)
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            if (skeletonAnimation == null)
            {
                Debug.LogError("SkeletonAnimation component is missing!", this);
                enabled = false;
            }
        }

        if (idleAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
        }
    }

    public void SetAiming(bool isAiming, float joystickMagnitude)
    {
        if (!enabled) return;
        
        IsAiming = isAiming;
        
        if (isAiming)
        {
            currentForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, joystickMagnitude);
            trajectoryPredictor.UpdateTrajectory(shotPoint.position, shotPoint.right, currentForce);
            
            if (targetAnimation != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, targetAnimation, true);
            }
        }
        else
        {
            trajectoryPredictor.HideTrajectory();
            
            if (idleAnimation != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
            }
        }
    }

    public void Shoot()
    {
        if (!enabled || currentForce < minLaunchForce) return;

        Instantiate(arrowPrefab, shotPoint.position, shotPoint.rotation)
            .GetComponent<Rigidbody2D>().velocity = shotPoint.right * currentForce;
    }
}