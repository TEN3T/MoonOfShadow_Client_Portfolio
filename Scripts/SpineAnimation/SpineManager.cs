using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineManager : MonoBehaviour
{
    private const int DEFAULT_TRACK = 0;
    private const float DEFAULT_DELAY = 0.0f;
    private const float DEFAULT_SPEED = 1.0f;

    //[SpineAnimation][SerializeField] private string[] clipNames;

    private SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    public void SetAnimation(string clipName, bool loop, int track = DEFAULT_TRACK, float speed = DEFAULT_SPEED)
    {
        if (skeletonAnimation.AnimationName != clipName)
        {
            skeletonAnimation.AnimationState.SetAnimation(track, clipName, loop).TimeScale = SetSpineSpeed(speed);
        }
    }

    public void AddAnimation(string clipName, bool loop, int track = DEFAULT_TRACK, float delay = DEFAULT_DELAY, float speed = DEFAULT_SPEED)
    {
        if (skeletonAnimation.AnimationName != clipName)
        {
            skeletonAnimation.AnimationState.AddAnimation(track, clipName, loop, delay).TimeScale = SetSpineSpeed(speed);
        }
    }

    public void SetDirection(Transform transform, Vector3 direction)
    {
        Vector3 angles = transform.localEulerAngles;
        if (direction.x < 0)
        {
            angles.y = 0.0f;
        }
        else if (direction.x > 0)
        {
            angles.y = 180.0f;
        }
        transform.localEulerAngles = angles;

        //if (direction.x < 0)
        //{
        //    skeletonAnimation.skeleton.ScaleX = 1.0f;
        //}
        //else if (direction.x > 0)
        //{
        //    skeletonAnimation.skeleton.ScaleX = -1.0f;
        //}

        //if (direction.x < 0)
        //{

        //}
        //else if (direction.x > 0)
        //{
        //}
    }

    public float SetSpineSpeed(float speed)
    {
        float weight = 1;
        if (speed > 5)
        {
            weight += ((float)Math.Pow(speed - 5, 2.0f / 3.0f) + (float)Math.Sqrt(speed - 5) - 1.0f) / 10.0f;
        }
        else
        {
            weight += (0.5f - speed / 10.0f) * -1.0f;
        }
        return weight;
    }

    public void SetColor(Color color)
    {
        skeletonAnimation.skeleton.SetColor(color);
    }

    public string GetAnimationName()
    {
        return skeletonAnimation.AnimationName;
    }
}
