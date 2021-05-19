using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InputManager : MonoBehaviour
{
    #region Private
    [SerializeField]
    private float swipeOffset = 10f;

    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;
    #endregion

    #region Public
    [HideInInspector]
    public static bool isSwiped;
    #endregion

    #region Events
    public static event System.Action<Vector3> Swipe;
    #endregion


    public Vector3 SwipeDirection;
   

    void Start()
    {
        isSwiped = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    fingerUpPos = touch.position;
                    fingerDownPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    fingerDownPos = touch.position;
                    DetectSwipe();
                    break;
            }
        }




    }

    void DetectSwipe()
    {
        if (CanSwipe() && !isSwiped)
        {
            if (IsVertical())
            {
                var direction = fingerDownPos.y - fingerUpPos.y > 0 ? Vector3.forward : Vector3.back;

                Swipe(direction);
              
              

            }
            else
            {
                var direction = fingerDownPos.x - fingerUpPos.x > 0 ? Vector3.right : Vector3.left;

                Swipe(direction);

            }

            isSwiped = true;
            fingerUpPos = fingerDownPos;
           
        }
    }


    bool CanSwipe()
    {
        if (VerticalSwipe() > swipeOffset || HorizontalSwipe() > swipeOffset)
            return true;
        else
            return false;
    }

    float VerticalSwipe()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
        
    }

    float HorizontalSwipe()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    bool IsVertical()
    {
        return VerticalSwipe() - HorizontalSwipe() > 0;
    }




}


