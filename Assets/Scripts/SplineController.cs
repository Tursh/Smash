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

    [SerializeField] private int KeyPointCount = 5, Precision = 75;
    private int PointCount;

    public void Start()
    {
        Camera cam = Camera.main;
        StartMapPosition = cam.ScreenToWorldPoint(new Vector3(0, 0, 12));
        EndMapPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 12));
        MapSize = EndMapPosition - StartMapPosition;

        splinePoints = GenerateSpline(KeyPointCount, Precision);
    }

    private int time = 0;

    private void Update()
    {
        transform.position = splinePoints[(time++) % PointCount];
    }

    Vector2 getRandomPointInMap(Vector2 centerPoint, float radius)
    {
        Vector2 point = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        point = centerPoint + (point + (-centerPoint + Vector2.down * 2).normalized * 0.2f).normalized * radius;
        return point;
    }

    Vector2[] GenerateSpline(int pointCount, int precision)
    {
        List<Vector2> keyPoints = new List<Vector2>();

        //Get the first point
        keyPoints.Add(new Vector2(Random.Range(StartMapPosition.x, EndMapPosition.x),
            Random.Range(StartMapPosition.y, EndMapPosition.y)));

        //Generate the rest of the points
        for (int i = 1; i < pointCount; ++i)
        {
            Vector2 randomPoint = getRandomPointInMap(keyPoints[i - 1], 5);
            if (i > 2)
            {
                float angleBetween = Vector2.Angle(keyPoints[i - 1], randomPoint) -
                                     Vector2.Angle(keyPoints[i - 2], keyPoints[i - 1]);
                if (angleBetween > 60.0f)
                    --i;
                else
                    keyPoints.Add(randomPoint);
            }
            else
                keyPoints.Add(randomPoint);
        }

        Vector2Int offset = Vector2Int.FloorToInt((keyPoints[(int) pointCount - 1] - StartMapPosition) / MapSize);
        Vector2 startPointWithOffset = keyPoints[0] + offset * MapSize;
        Vector2 Distance = startPointWithOffset - keyPoints[keyPoints.Count - 1];

        do
        {
            keyPoints.Add(keyPoints[keyPoints.Count - 1] + Distance.normalized * Math.Min(Distance.magnitude, 5.0f));
            Distance = startPointWithOffset - keyPoints[keyPoints.Count - 1];
        } while (Distance.magnitude > 0.5f);


        
        int actualPointCount = keyPoints.Count;
        keyPoints[keyPoints.Count - 1] = keyPoints[0];
        keyPoints.Add(keyPoints[1]);
        keyPoints.Add(keyPoints[2]);
        keyPoints.Add(keyPoints[3]);
        keyPoints.Add(keyPoints[4]);
        keyPoints.Add(keyPoints[5]);
        
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

        Vector2[] points = new Vector2[actualPointCount * precision];

        for (uint i = 0; i < actualPointCount * precision; ++i)
        {
            points[i] = new Vector2(xs[i + precision * 3], ys[i + precision * 3]);
            if (points[i].x < StartMapPosition.x)
                points[i].x += MapSize.x * (float)Math.Abs(Math.Floor((points[i].x - StartMapPosition.x) / MapSize.x));
            else if (points[i].x > EndMapPosition.x)
                points[i].x -= MapSize.x * (float)Math.Abs(Math.Floor((points[i].x - StartMapPosition.x) / MapSize.x));
            if (points[i].y < StartMapPosition.y)
                points[i].y += MapSize.y * (float)Math.Abs(Math.Floor((points[i].y - StartMapPosition.y) / MapSize.y));
            else if (points[i].y > EndMapPosition.y)
                points[i].y -= MapSize.y * (float)Math.Abs(Math.Floor((points[i].y - StartMapPosition.y) / MapSize.y));
        }

        int stopPoint = actualPointCount * precision;
        
        for (int i = (actualPointCount - 1) * precision; i < actualPointCount * precision; ++i)
            if ((points[i] - points[0]).magnitude < 0.1f)
            {
                stopPoint = i;
                break;
            }
        
        Vector2[] abreviatedPoints = new Vector2[stopPoint];
        
        PointCount = stopPoint;
        
        for (int i = 0; i < stopPoint; ++i)
            abreviatedPoints[i] = points[i];

        return abreviatedPoints;
    }
}