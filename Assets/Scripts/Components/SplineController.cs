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

        splinePoints = GenerateSpline();
    }

    private int time = 0;

    private void Update()
    {
        transform.position = splinePoints[(time++) % PointCount];
    }

    Vector2 getPositionInScreen(Vector2 position)
    {
        if (position.x < StartMapPosition.x)
            position.x += MapSize.x * (float) Math.Abs(Math.Floor((position.x - StartMapPosition.x) / MapSize.x));
        else if (position.x > EndMapPosition.x)
            position.x -= MapSize.x * (float) Math.Abs(Math.Floor((position.x - StartMapPosition.x) / MapSize.x));
        if (position.y < StartMapPosition.y)
            position.y += MapSize.y * (float) Math.Abs(Math.Floor((position.y - StartMapPosition.y) / MapSize.y));
        else if (position.y > EndMapPosition.y)
            position.y -= MapSize.y * (float) Math.Abs(Math.Floor((position.y - StartMapPosition.y) / MapSize.y));
        return position;
    }

    Vector2[] GenerateSpline()
    {
        List<Vector2> keyPoints = generateKeyPoints(KeyPointCount);
        keyPoints = MakeKeyPointIntoALoop(keyPoints);

        Vector2[] points = GenerateSpline(keyPoints.ToArray());
        return getLoopedPoints(points);
    }

    List<Vector2> generateKeyPoints(int KeyPointCount)
    {
        List<Vector2> keyPoints = new List<Vector2>();

        //Get the 2 first to start the spline
        for (int i = 0; i < 2; ++i)
            keyPoints.Add(new Vector2(Random.Range(StartMapPosition.x, EndMapPosition.x),
                Random.Range(StartMapPosition.y, EndMapPosition.y)));

        //Generate the key points
        for (int i = 2; i < KeyPointCount; ++i)
        {
            Vector2 randomPoint = getRandomPointInMap(keyPoints[i - 1], 5);
            //If the angle is too large, get a new one
            float angleBetween = Vector2.Angle(keyPoints[i - 1], randomPoint) -
                                 Vector2.Angle(keyPoints[i - 2], keyPoints[i - 1]);
            if (angleBetween > 60.0f)
                --i;
            else
                keyPoints.Add(randomPoint);
        }

        return keyPoints;
    }

    Vector2 getRandomPointInMap(Vector2 centerPoint, float radius)
    {
        Vector2 point = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        point = centerPoint + point.normalized * radius;
        return point;
    }

    List<Vector2> MakeKeyPointIntoALoop(List<Vector2> keyPoints)
    {
        //Generate new key points to make a loop
        Vector2Int windowOffset =
            Vector2Int.FloorToInt((keyPoints[(int) KeyPointCount - 1] - StartMapPosition) / MapSize);
        Vector2 startPointWithOffset = keyPoints[0] + windowOffset * MapSize;
        Vector2 Distance = startPointWithOffset - keyPoints[keyPoints.Count - 1];

        do
        {
            keyPoints.Add(keyPoints[keyPoints.Count - 1] +
                          Distance.normalized * Math.Min(Distance.magnitude, 5.0f));
            Distance = startPointWithOffset - keyPoints[keyPoints.Count - 1];
        } while (Distance.magnitude > 0.5f);

        //If the generated key point is too close to the start key point, remove it
        if ((startPointWithOffset - keyPoints[keyPoints.Count - 1]).magnitude < 0.001f)
            keyPoints.RemoveAt(keyPoints.Count - 1);

        //Get how much real key points was generated to make the loop
        KeyPointCount = keyPoints.Count;

        //Double the points to create a loop
        keyPoints.AddRange(keyPoints);
        windowOffset = Vector2Int.FloorToInt((keyPoints[keyPoints.Count - 1] - StartMapPosition) / MapSize);
        for (int i = this.KeyPointCount; i < keyPoints.Count; ++i)
            keyPoints[i] += windowOffset * MapSize;
        //Double the points a second time
        windowOffset = Vector2Int.FloorToInt((keyPoints[keyPoints.Count - 1] - StartMapPosition) / MapSize);
        keyPoints.AddRange(keyPoints.GetRange(0, this.KeyPointCount));
        for (int i = this.KeyPointCount * 2; i < keyPoints.Count; ++i)
            keyPoints[i] += windowOffset * MapSize;

        return keyPoints;
    }

    Vector2[] GenerateSpline(Vector2[] keyPoints)
    {
        //Get key points into float arrays
        float[] x = new float[keyPoints.Length];
        float[] y = new float[keyPoints.Length];
        for (int i = 0; i < keyPoints.Length; ++i)
        {
            x[i] = keyPoints[i].x;
            y[i] = keyPoints[i].y;
        }

        //Get buffer to generate the spline trajectory
        float[] xs = new float[keyPoints.Length * Precision];
        float[] ys = new float[keyPoints.Length * Precision];

        //Generate the spline
        CubicSpline.FitParametric(x, y, (int) (keyPoints.Length * Precision), out xs, out ys);

        //Get all the points in vec2 form
        Vector2[] points = new Vector2[this.KeyPointCount * 2 * Precision];
        for (int i = this.KeyPointCount * Precision; i < keyPoints.Length * Precision; ++i)
            points[i - this.KeyPointCount * Precision] = getPositionInScreen(new Vector2(xs[i], ys[i]));

        return points;
    }

    Vector2[] getLoopedPoints(Vector2[] points)
    {
        //If never stopped, the last point is the last point generated
        int stopPoint = 2 * KeyPointCount * Precision;

        //Last distance calculated between the start point and the trajectory point
        float lastDistance = 100;

        for (int i = (KeyPointCount - 1) * Precision; i < stopPoint; ++i)
            if ((points[i] - points[0]).magnitude > lastDistance && lastDistance < 2)
            {
                stopPoint = i;
                break;
            }
            else
                lastDistance = (points[i] - points[0]).magnitude;

        PointCount = stopPoint;
        Vector2[] splinePoints = new Vector2[PointCount];
        
        Debug.Log(lastDistance);

        for (int i = 0; i < stopPoint; ++i)
            splinePoints[i] = points[i];

        return splinePoints;
    }
}