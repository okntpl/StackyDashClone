using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public class StackController : SuperClass
{


    #region Private
    [SerializeField] Transform stacks;
    [SerializeField] Transform player;
    [SerializeField] PlayerSettings playerSettings;

   
    private int stacksLeftToBridge;
    private int currentBridgeLenght;
    
    private Vector3 direction;
    private List<Transform> currentBridges = new List<Transform>();
    private List<Transform> passedBridges = new List<Transform>();
    private bool swipeMove, bridgeMove, passedMove;
    private bool isHole = false;
    #endregion





    private void Start()
    {
        swipeMove = true;
    }
    private void OnEnable()
    {
        InputManager.Swipe += SwipeDirection;
        
        
        
    }

    private void OnDisable()
    {
        InputManager.Swipe -= SwipeDirection;
    }


    private void FixedUpdate()
    {

        #region SwipeMovement
        if (swipeMove)
        {
            Ray ray = new Ray(stacks.transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 0.65f))
            {

                if (hit.transform.CompareTag("Stack"))
                {

                    EventManager.OnCollectStack?.Invoke();
                    Haptic.LightTaptic();

                    hit.transform.tag = "Collected";
                    hit.transform.GetComponent<BoxCollider>().enabled = false;
                    hit.transform.SetParent(stacks);
                    hit.transform.localPosition = new Vector3(0, 0.1f * (stacks.childCount), 0);
                    MovePlayerY();

                }

                else if (hit.transform.CompareTag("Floor"))
                {

                    InputManager.isSwiped = false;
                    return;

                }

                else if (hit.transform.CompareTag("Bridge") && swipeMove)
                {

                    InitializeCurrentBridge(hit.transform.parent);
                    bridgeMove = true;
                    swipeMove = false;
                }
                else if (hit.transform.CompareTag("Passed") && swipeMove)
                {
                    passedMove = true;
                    swipeMove = false;
                }



            }

            transform.position += new Vector3(direction.x, 0, direction.z) * Time.fixedDeltaTime * playerSettings.MoveSpeed;


        }
        #endregion

        #region SetStackIntoBridge
        if (bridgeMove)
        {

            if (stacks.childCount - 1 > 0)
            {

               
                transform.position = Vector3.Lerp(transform.position,
                new Vector3(currentBridges[0].transform.position.x, transform.position.y,
                currentBridges[0].transform.position.z), 0.4f);


                if (Vector3.Distance(transform.position, currentBridges[0].transform.position) < 0.2f)
                {
                    Haptic.LightTaptic();
                    stacks.GetChild(stacks.childCount - 1).transform.parent = currentBridges[0].transform;
                    currentBridges[0].transform.GetChild(0).transform.localPosition = new Vector3(0, 1, 0);
                    currentBridges[0].transform.tag = "Passed";
                    currentBridges[0].transform.GetChild(0).GetComponent<Renderer>().material.color = Color.gray;
                    MovePlayerY();
                    passedBridges.Add(currentBridges[0]);
                    currentBridges.RemoveAt(0);
                    stacksLeftToBridge--;
                   

                }
            }
            if (currentBridges.Count == 0)
            {
           
                stacksLeftToBridge = 0;
                bridgeMove = false;
                swipeMove = true;
            }
            else if (stacks.childCount < 2 && currentBridges.Count != 0)
            {
                InputManager.isSwiped = false;
                bridgeMove = false;
                passedMove = true;
                swipeMove = false;
            }



        }
        #endregion

        #region AUTO BRIDGE MOVE
        if (passedMove)
        {
           

            if (direction == Vector3.back)
            {

                if (currentBridges.Count<currentBridgeLenght)
                {

                    transform.position = Vector3.Lerp(transform.position, new Vector3(
                        passedBridges.Last().position.x,
                        transform.position.y, passedBridges.Last().position.z
                        ), 0.3f);

                    if (Vector3.Distance(transform.position, passedBridges.Last().position) < 0.2f)
                    {
                        currentBridges.Add(passedBridges.Last());
                        passedBridges.RemoveAt(passedBridges.Count - 1);

                    }
                   
                }

                else
                {
                   
                    passedMove = false;
                    swipeMove = true;

                }

            

            }

            if (direction == Vector3.forward)
            {
                
                    if (currentBridges.Count-stacksLeftToBridge > 0)
                    {

                        transform.position = Vector3.Lerp(transform.position, new Vector3(
                          currentBridges.Last().position.x,
                          transform.position.y, currentBridges.Last().position.z
                          ), 0.3f);

                        if (Vector3.Distance(transform.position, currentBridges.Last().position) < 0.2f)
                        {
                            passedBridges.Add(currentBridges.Last());
                            currentBridges.RemoveAt(currentBridges.Count - 1);

                        }

                    }

                    else if(stacks.childCount>1)
                    {    
                    swipeMove = true;
                    passedMove = false;
                    }
                    else
                    {
                   
                   
                    direction = Vector3.zero;
                    InputManager.isSwiped = false;
                    
                    }

                

            }
        }
        #endregion
    }


    void InitializeCurrentBridge(Transform bridges)
    {
        if (currentBridges.Count == 0)
        {
            currentBridgeLenght = bridges.childCount;
            foreach (Transform bridge in bridges)
            {
               
                stacksLeftToBridge++;
                currentBridges.Add(bridge);
            }
        }
    }

   

    private void SwipeDirection(Vector3 direction)
    {

        this.direction = direction;
    }



    private void MovePlayerY()
    {
        player.transform.localPosition = new Vector3(player.transform.localPosition.x, 0.1f * (stacks.childCount), player.transform.localPosition.z);
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("UpRight"))
        {
            Haptic.LightTaptic();
         
            if (Vector3.Distance(transform.position, other.transform.position) < 0.25f)
            {
                
                if (direction == Vector3.left)
                {
                    other.transform.DOLocalRotate(Vector3.forward * 90, 0.5f,RotateMode.Fast);
                    direction = Vector3.forward;
                    
                }
                if(direction == Vector3.back)
                {
                    other.transform.DOLocalRotate(Vector3.forward * 0, 0.5f, RotateMode.Fast);
                    direction = Vector3.right;
                }

            }
        }

        if (other.CompareTag("UpLeft"))
        {
            Haptic.LightTaptic();
            if (Vector3.Distance(transform.position, other.transform.position) < 0.25f)
            {

                if (direction == Vector3.right)
                {
                    other.transform.DOLocalRotate(Vector3.forward * 90, 0.5f, RotateMode.Fast);
                    direction = Vector3.forward;
                }
                 if(direction==Vector3.back)
                {
                    other.transform.DOLocalRotate(Vector3.forward * 180, 0.5f, RotateMode.Fast);
                    direction = Vector3.left;
                }

            }
        }
        if (other.CompareTag("DownLeft"))
        {
            Haptic.LightTaptic();
          
            if (Vector3.Distance(transform.position, other.transform.position) < 0.25f)
            {
                if (direction == Vector3.right)
                {
                    other.transform.DOLocalRotate(Vector3.forward * -90, 0.5f, RotateMode.Fast);
                    direction = Vector3.back;
                }
               if(direction==Vector3.forward)
                {
                    other.transform.DOLocalRotate(Vector3.forward * 180, 0.5f, RotateMode.Fast);
                    direction = Vector3.left;
                }

            }
        }

        if (other.CompareTag("DownRight"))
        {
            Haptic.LightTaptic();

            if (Vector3.Distance(transform.position, other.transform.position) < 0.25f)
            {

                if (direction == Vector3.forward)
                {
                    other.transform.DOLocalRotate(Vector3.forward* 0 , 0.5f, RotateMode.Fast);
                    direction = Vector3.right;
                }
                if(direction==Vector3.left)
                {
                    other.transform.DOLocalRotate(Vector3.forward*-90, 0.5f, RotateMode.Fast);
                    direction = Vector3.back;
                }



            }
        }

        else if(other.CompareTag("Hole"))
        {

            Haptic.LightTaptic();
            if (Vector3.Distance(transform.position, other.transform.position) <0.25f && isHole)
            {
                other.tag = "DeathHole";
                isHole = false;
                direction = Vector3.zero;
                

                var parentHole = other.transform.parent;
               
                other.transform.parent=null;
               
               
                
                Vector3 dir = parentHole.GetChild(0).transform.position;
                parentHole.GetChild(0).transform.tag = "DeathHole";
               
                transform.DOMoveY(-2, 0.5f);


                transform.DOMove(new Vector3(dir.x, -0.3f*stacks.childCount, dir.z), 0.5f).SetDelay(0.5f).
                OnComplete(() =>
                 transform.DOMoveY(DEFAULT_PLAYER_Y, 0.2f).OnComplete(()=>
                 InputManager.isSwiped = false
                 
                 )
                 
                );

                other.transform.parent = parentHole;
                other.tag = "Hole";
                
            }

  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("DeathHole"))
        {
            Haptic.LightTaptic();
            other.tag = "Hole";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Hole"))
        {
            Haptic.LightTaptic();
            isHole = true;
         
        }
   

        if (other.CompareTag("Bonus"))
        {
            Haptic.LightTaptic();
            if (other.gameObject.transform.childCount < DEFAULT_BONUS_CHILD_COUNT + 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (stacks.childCount > 0)
                    {
                        stacks.GetChild(stacks.childCount - 1).transform.DOLocalMove(new Vector3(1, 0, i + 1), i*0.15f);
                        stacks.GetChild(stacks.childCount - 1).transform.parent = other.gameObject.transform;
                        MovePlayerY();
                    }
                    else
                    {

                        direction = Vector3.zero;
                        player.transform.parent = other.gameObject.transform;
                        if (other.gameObject.transform.childCount > DEFAULT_BONUS_CHILD_COUNT)
                        {
                            player.transform.DOLocalMove(other.gameObject.transform.GetChild(DEFAULT_BONUS_CHILD_COUNT + i - 1).transform.localPosition, 0.5f);

                        }
                        else
                        {
                            player.transform.DOLocalMove(other.gameObject.transform.GetChild(DEFAULT_BONUS_CHILD_COUNT - 2).transform.localPosition, 0.5f);
                        }
                        EventManager.OnGameFinish?.Invoke();
                        return;
                    }
                }

            }
        }

                if (other.CompareTag("Finish"))
                {
                      Haptic.MediumTaptic();
                   if (Level.Instance.levelMode != Level.LevelMode.CLEAR)
                    {
                        Level.Instance.SetLevelMode(Level.LevelMode.CLEAR);

                        EventManager.OnGameClear?.Invoke();
                    }
                }

                if (other.CompareTag("Podium"))
                {
                   Haptic.MediumTaptic();
                    direction = Vector3.zero;
                    player.transform.parent = null;
                    player.DOMove(new Vector3(player.transform.position.x, 3, player.transform.position.z + 3), 1f);

                    player.DORotate(Vector3.up * 180, 1f, RotateMode.Fast);

                    Invoke("SetFinish", 2f);
                   
                }
        
    }

    void SetFinish()
    {
        EventManager.OnGameFinish?.Invoke();
    }



    private void OnDrawGizmos()
    {
       
         if(swipeMove)
         {
             Gizmos.DrawRay(stacks.transform.position, direction);
         }
     
        

      
    }
}
