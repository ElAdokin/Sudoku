using System.Collections.Generic;
using UnityEngine;

public class MathsUtilities
{
    public List<Vector3> CalculatePointsArroundCircle(int amountPositions, Vector3 center, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < amountPositions; i++)
        {
            float radians = 2 * Mathf.PI / amountPositions * i;

            float vertical = Mathf.Sin(radians);
            float horizontal = Mathf.Cos(radians);

            Vector3 spawnDir = new Vector3(horizontal, vertical, 0);
            Vector3 newPoint = center + spawnDir * radius;

            points.Add(newPoint);
        }

        return points;
    }

    public float CalculateHypotenuseOfTriangle(float aSide, float bSide)
    {
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(aSide, 2) + Mathf.Pow(bSide, 2));

        return hypotenuse;
    }

    public List<Vector3> CalculatePointsInsideSphere(int amountPositions, Vector3 center, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < amountPositions; i++)
        {
            Vector3 newPoint = Vector3.zero;

            float k = i + 0.5f;
            float r = Mathf.Sqrt((k) / amountPositions);
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * k;
            float x = r * Mathf.Cos(theta) * radius;
            float y = r * Mathf.Sin(theta) * radius;

            newPoint = new Vector3(x, y, 0);

            points.Add(newPoint);
        }

        return points;
    }

    public Vector3 GetMiddlePositionWithTwoPoints(Vector3 point_01, Vector3 point_02)
    {
        Vector3 middlePoint = Vector3.Lerp(point_01, point_02, 0.5f); 
        return middlePoint;
    }

    public Vector3 GetPointInsideLine(Vector3 beginPosition, Vector3 endPosition, float factor)
    {
        Vector3 point = factor * Vector3.Normalize(endPosition - beginPosition) + beginPosition;

        return point;
    }
}
