using UnityEngine;
using Spine.Unity;
using Spine;

public class SpineBodyController : MonoBehaviour 
{
    [Header("Spine Settings")]
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private string bodyBoneName = "body";
    
    [Header("Joystick Settings")] 
    [SerializeField] private Joystick joystick;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float maxRotationAngle = 60f;
    [SerializeField] private bool invertHorizontal = false;
    [SerializeField] private bool invertVertical = true;

    private float horizontal;
    private float vertical;

    private Bone bodyBone;

    private void Start()
    {
        bodyBone = skeletonAnimation.Skeleton.FindBone(bodyBoneName);
        if(bodyBone == null)
            Debug.LogError($"Bone '{bodyBoneName}' not found!");
    }

    private void Update()
    {
        if(bodyBone == null) return;
        
        if (invertHorizontal)
        {
            horizontal = -joystick.Horizontal;
        }
        else
        {
            horizontal = joystick.Horizontal;
        }
        
        if (invertVertical)
        {
            vertical = -joystick.Vertical;
        }
        else
        {
            vertical = joystick.Vertical;
        }
        
        if(Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            float targetAngle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
            targetAngle = Mathf.Clamp(targetAngle, -maxRotationAngle, maxRotationAngle);
            
            bodyBone.Rotation = Mathf.LerpAngle(bodyBone.Rotation, targetAngle, rotationSpeed * Time.deltaTime);
        }
        else
        {
            bodyBone.Rotation = Mathf.LerpAngle(bodyBone.Rotation, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}