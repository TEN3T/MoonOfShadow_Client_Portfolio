using Spine.Unity;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpineAnimatorManager : MonoBehaviour
{
    private Animator animator;

    private float animationSpeed = 1f;

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        animator.speed = animationSpeed;
    }

    private void Init()
    {
        animator = GetComponentInChildren<Animator>();
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
    }

    public void SetDirection(Transform transform, Vector3 direction, Transform child)
    {
        Vector3 angles = transform.localEulerAngles;
        Vector3 childPos = child.localPosition;
        Collider2D collider = transform.GetComponent<Collider2D>();
        Vector2 colPos = collider.offset;
        if (direction.x < 0)
        {
            angles.y = 0.0f;
            if (childPos.x < 0)
            {
                childPos.x *= -1;
                colPos.x *= -1;
                collider.offset = colPos;
            }
        }
        else if (direction.x > 0)
        {
            angles.y = 180.0f;
            if (childPos.x > 0)
            {
                childPos.x *= -1;
                colPos.x *= -1;
                collider.offset = colPos;
            }
        }
        transform.localEulerAngles = angles;
        child.localPosition = childPos;
    }

    public void SetSpineSpeed(float speed)
    {
        float weight = 0;
        if (speed > 5)
        {
            weight = ((float)Math.Pow(speed - 5, 2.0f / 3.0f) + (float)Math.Sqrt(speed - 5) - 1.0f) / 10.0f;
        }
        else
        {
            weight = (0.5f - speed / 10.0f) * -1.0f;
        }
        animationSpeed = 1 + weight;
    }

    #region Animation Control
    public void PlayAnimation(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    public void PlayAnimation(string parameter, int value)
    {
        animator.SetInteger(parameter, value);
    }

    public void PlayAnimation(string parameter, float value)
    {
        animator.SetFloat(parameter, value);
    }

    public void PlayAnimation(string parameter)
    {
        animator.SetTrigger(parameter);
    }
    #endregion
}
