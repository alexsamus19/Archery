using UnityEngine;

public class TrajectoryPredictor : MonoBehaviour
{
    [Header("Trajectory Settings")]
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private int numberOfPoints = 15;
    [SerializeField] private float minSpaceBetweenPoints = 0.1f;
    [SerializeField] private float maxSpaceBetweenPoints = 0.3f;

    private GameObject[] points;
    private float maxLaunchForce;

    public void SetMaxLaunchForce(float force)
    {
        maxLaunchForce = force;
    }

    private void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
            points[i].SetActive(false);
        }
    }

    public void UpdateTrajectory(Vector2 startPosition, Vector2 direction, float force)
    {
        float spacing = Mathf.Lerp(minSpaceBetweenPoints, maxSpaceBetweenPoints, force / maxLaunchForce);

        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].SetActive(true);
            points[i].transform.position = CalculatePointPosition(startPosition, direction, force, i * spacing);
        }
    }

    public void HideTrajectory()
    {
        foreach (var point in points)
        {
            point.SetActive(false);
        }
    }

    private Vector2 CalculatePointPosition(Vector2 startPos, Vector2 dir, float force, float t)
    {
        return startPos + (dir * force * t) + 0.5f * Physics2D.gravity * (t * t);
    }
}