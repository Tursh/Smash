using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestMySpline;
using UnityEngine;
using Random = UnityEngine.Random;

public class SplineController : MonoBehaviour
{
    private Vector2 StartMapPosition, EndMapPosition, MapSize;
    private Bounds platformBounds;
    private Vector2[] splinePoints;

    [SerializeField] private uint WantedPointCount = 25, Precision = 50;

    public void Start()
    {
        Camera cam = Camera.main;
        StartMapPosition = cam.ScreenToWorldPoint(new Vector3(0, 0, 12));
        EndMapPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 12));
        MapSize = EndMapPosition - StartMapPosition;

        splinePoints = GenerateSpline(WantedPointCount, Precision);
    }

    private int time = 0;

    private void Update()
    {
        transform.position = splinePoints[(time++) % (25 * 25)];
        if (time % 10 == 0)
            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position, Quaternion.identity);
    }

    Vector2 getRandomPointInMap(Vector2 centerPoint, float radius)
    {
        Vector2 point = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        point = centerPoint + point.normalized * radius;
        return point;
    }

    Vector2[] GenerateSpline(uint pointCount, uint precision)
    {
        List<Vector2> keyPoints = new List<Vector2>();

        //Get the first point
        keyPoints.Add(new Vector2(Random.Range(StartMapPosition.x, EndMapPosition.x),
            Random.Range(StartMapPosition.y, EndMapPosition.y)));

        //Generate the rest of the points
        for (int i = 1; i < pointCount; ++i)
        {
            keyPoints.Add(getRandomPointInMap(keyPoints[i - 1], 5));
            if (i > 1)
            {
                float angleBetween = Vector2.Angle(keyPoints[i - 1], keyPoints[i]) -
                                     Vector2.Angle(keyPoints[i - 2], keyPoints[i - 1]);
                if (angleBetween > 60.0f)
                    --i;
            }
        }

        int extraPointCount = -1;
        Vector2Int offset = Vector2Int.FloorToInt((keyPoints[(int) pointCount - 1] - StartMapPosition) / MapSize);
        Vector2 startPointWithOffset = keyPoints[0] + offset * MapSize;
        Vector2 Distance;

        do
        {
            Vector2 lastPoint = keyPoints[(int) (pointCount + extraPointCount)];
            Distance = startPointWithOffset - lastPoint;
            keyPoints.Add(lastPoint + Distance.normalized * Math.Min(Distance.magnitude, 5.0f));
            ++extraPointCount;
        } while (Distance.magnitude < 1.0f);

        //Generate the spline
        float[] x = new float[keyPoints.Count];
        float[] y = new float[keyPoints.Count];

        for (int i = 0; i < keyPoints.Count; ++i)
        {
            x[i] = keyPoints[i].x;
            y[i] = keyPoints[i].y;
        }

        float[] xs = new float[keyPoints.Count * precision];
        float[] ys = new float[keyPoints.Count * precision];

        CubicSpline.FitParametric(x, y, (int) (keyPoints.Count * precision), out xs, out ys);

        Vector2[] points = new Vector2[pointCount * precision];

        for (int i = 0; i < pointCount * precision; ++i)
        {
            points[i] = new Vector2(xs[i], ys[i]);
            if (points[i].x < StartMapPosition.x)
                points[i].x += MapSize.x;
            else if (points[i].x > EndMapPosition.x)
                points[i].x -= MapSize.x;
            if (points[i].y < StartMapPosition.y)
                points[i].y += MapSize.y;
            else if (points[i].y > EndMapPosition.y)
                points[i].y -= MapSize.y;
        }

        return points;
    }
}