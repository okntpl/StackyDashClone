using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] float moveSpeed;
  

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    





}
