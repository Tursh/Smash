using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    /// <summary>
    /// Transforms a vector2 into a vector3
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns>Vector3 resulting with Z value of 0</returns>
    public static Vector3 Vec22Vec3(Vector2 vector2) => new Vector3(vector2.x,vector2.y);
    
    /// <summary>
    /// Copy of Degree2Vec2
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 NormalizedVectorFromAngle(float angle) => 
        new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad));
    /// <summary>
    /// Transform a Vector2 into its Degree value
    /// </summary>
    /// <param name="vec2">Vector2 to transform</param>
    /// <returns>Value will always yield over 0</returns>
    public static float Vec22Degree(Vector2 vec2)
    {
        float stickValue = Mathf.Atan2(-vec2.x, vec2.y) * Mathf.Rad2Deg;
        if (stickValue < 0)
            stickValue += 360f;
        return stickValue;
    }
    /// <summary>
    /// Transforms a Degree value to a unit Vector2
    /// </summary>
    /// <param name="degree">Degree value to turn into a Vector</param>
    /// <returns>Unit Vector2</returns>
    public static Vector2 Degree2Vec2(float degree) => new Vector2(
        Mathf.Cos(degree * Mathf.Deg2Rad),
        Mathf.Sin(degree * Mathf.Deg2Rad));
    
    /// <summary>
    /// Takes in a Vector3 and turns it into a Vector2 ignoring the Z coordinate
    /// </summary>
    /// <param name="vec3">Vector you want transformed</param>
    /// <returns>Resulting vector</returns>
    public static Vector2 Vec32Vec2(Vector3 vec3) => new Vector2(vec3.x,vec3.y);
    
    
    /// <summary>
    /// Scales rotation in degrees
    /// </summary>
    /// <param name="startDegrees">Degrees of where the character is</param>
    /// <param name="targetDegrees">Degrees of where you want the character to be</param>
    /// <param name="scale">How much of this rotation you want to be done when called</param>
    /// <returns>Final degree value</returns>
    public static float RotateGradually(float startDegrees,float targetDegrees, float scale = 1)
    {
        float rotationToBeDone = targetDegrees -startDegrees;
        
        if (rotationToBeDone > 180f)
            rotationToBeDone -= 360f;
        else if (rotationToBeDone < -180f)
            rotationToBeDone += 360f;

        rotationToBeDone *= scale;

        return rotationToBeDone;
    }
}
