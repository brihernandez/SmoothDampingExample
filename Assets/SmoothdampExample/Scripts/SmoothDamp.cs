using UnityEngine;

// Thanks to Rory Driscoll
// http://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
public static class SmoothDamp
{
    /// <summary>
    /// Creates dampened motion between a and b that is framerate independent.
    /// </summary>
    /// <param name="from">Initial parameter</param>
    /// <param name="to">Target parameter</param>
    /// <param name="speed">Smoothing factor</param>
    /// <param name="dt">Time since last damp call</param>
    static public Quaternion Rotate(Quaternion from, Quaternion to, float speed, float dt)
    {
        return Quaternion.Slerp(from, to, 1 - Mathf.Exp(-speed * dt));
    }

    /// <summary>
    /// Creates dampened motion between a and b that is framerate independent.
    /// </summary>
    /// <param name="from">Initial parameter</param>
    /// <param name="to">Target parameter</param>
    /// <param name="speed">Smoothing factor</param>
    /// <param name="dt">Time since last damp call</param>
    static public Vector3 Move(Vector3 from, Vector3 to, float speed, float dt)
    {
        return Vector3.Lerp(from, to, 1 - Mathf.Exp(-speed * dt));
    }

    /// <summary>
    /// Creates dampened motion between a and b that is framerate independent.
    /// </summary>
    /// <param name="from">Initial parameter</param>
    /// <param name="to">Target parameter</param>
    /// <param name="speed">Smoothing factor</param>
    /// <param name="dt">Time since last damp call</param>
    static public Vector2 Move(Vector2 from, Vector2 to, float speed, float dt)
    {
        return Vector2.Lerp(from, to, 1 - Mathf.Exp(-speed * dt));
    }

    /// <summary>
    /// Creates dampened motion between a and b that is framerate independent.
    /// </summary>
    /// <param name="from">Initial parameter</param>
    /// <param name="to">Target parameter</param>
    /// <param name="speed">Smoothing factor</param>
    /// <param name="dt">Time since last damp call</param>
    static public float Move(float from, float to, float speed, float dt)
    {
        return Mathf.Lerp(from, to, 1 - Mathf.Exp(-speed * dt));
    }

    /// <summary>
    /// Creates dampened motion between angles a and b that is framerate independent.
    /// Meant to be used on angles that range from -180 to 180, and instead moves between
    /// angles 0 to 360 without discontinuities.
    /// </summary>
    /// <param name="from">Initial parameter</param>
    /// <param name="to">Target parameter</param>
    /// <param name="speed">Smoothing factor</param>
    /// <param name="dt">Time since last damp call</param>
    static public float MoveAngle(float from, float to, float speed, float dt)
    {
        float delta = Mathf.Repeat((to - from), 360.0F);
        if (delta > 180.0F)
            delta -= 360.0F;
        to = from + delta;
        return Mathf.Lerp(from, to, 1 - Mathf.Exp(-speed * dt));
    }
}
