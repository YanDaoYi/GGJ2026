using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 minMaxXY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform targetPosition;
    private Vector3 LastPosition;
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private void Start()
    {
        LastPosition = targetPosition.position;
    }
    private void FixedUpdate()
    {
        Vector3 targetPosition = target.transform.position;
        targetPosition.z = -10;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -minMaxXY.x, minMaxXY.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -minMaxXY.y, minMaxXY.y);


        Vector3 deltaMovement = this.targetPosition.position - LastPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);

        LastPosition = this.targetPosition.position;
    }
}
