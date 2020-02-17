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

    public void Start()
    {
        Camera cam = Camera.main;
        StartMapPosition = cam.ScreenToWorldPoint(new Vector3(0, 0, 12));
        EndMapPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 12));
        MapSize = EndMapPosition - StartMapPosition;

        splinePoints = GenerateSpline(25, 25);
    }

    private int time = 0;

    private void Update()
    {
        transform.position = splinePoints[(time++) % (25 * 25)];
    }

    Vector2 getRandomPointInMap(Vector2 centerPoint, float radius)
    {
        Vector2 point = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        point = centerPoint + point.normalized * radius - StartMapPosition;
        point.x = (point.x % MapSize.x) + StartMapPosition.x;
        point.y = (point.y % MapSize.y) + StartMapPosition.y;
        return point;
    }

    Vector2[] GenerateSpline(uint pointCount, uint precision)
    {
        Vector2[] points = new Vector2[pointCount * precision];
        Vector2[] keyPoints = new Vector2[pointCount];

        //Get the first point
        keyPoints[0] = new Vector2(Random.Range(StartMapPosition.x, EndMapPosition.x),
            Random.Range(StartMapPosition.y, EndMapPosition.y));

        //Get the rest of the key points
        for (int i = 1; i < pointCount; ++i)
        {
            keyPoints[i] = getRandomPointInMap(keyPoints[i - 1], 5);
            if (i > 1)
            {
                float angleBetween = Vector2.Angle(keyPoints[i - 1], keyPoints[i]) +
                                     Vector2.Angle(keyPoints[i - 2], keyPoints[i - 1]);
                if (angleBetween > 45.0f)
                    --i;
            }
        }

        float[] x = new float[pointCount];
        float[] y = new float[pointCount];

        for (int i = 0; i < pointCount; ++i)
        {
            x[i] = keyPoints[i].x;
            y[i] = keyPoints[i].y;
        }

        float[] xs = new float[pointCount * precision];
        float[] ys = new float[pointCount * precision];

        CubicSpline.FitParametric(x, y, (int) (pointCount * precision), out xs, out ys);

        for (int i = 0; i < pointCount * precision; ++i)
        {
            points[i] = new Vector2(xs[i], ys[i]);
        }

        return points;
    }
}