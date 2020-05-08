using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static Vector3 Vec22Vec3(Vector2 vector2) => new Vector3(vector2.x,vector2.y);
    public static Vector2 NormalizedVectorFromAngle(float angle) => 
        new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad));
    public static float Vec22Degree(Vector2 vec2)
    {
        float stickValue = Mathf.Atan2(-vec2.x, vec2.y) * Mathf.Rad2Deg;
        if (stickValue < 0)
            stickValue += 360f;
        return stickValue;
    }

    public static Vector2 Degree2Vec2(float degree) => new Vector2(
        Mathf.Cos(degree * Mathf.Deg2Rad),
        Mathf.Sin(degree * Mathf.Deg2Rad));
    
    public static Vector2 Vec32Vec2(Vector3 vec3) => new Vector2(vec3.x,vec3.y);
}
