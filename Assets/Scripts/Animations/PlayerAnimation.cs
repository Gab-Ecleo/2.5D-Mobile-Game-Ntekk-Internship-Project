using System;
using EventScripts;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Animator _anim;

    private void Awake()
    {
        PlayerEvents.ON_PLAYER_JUMP += ON_PLAYER_JUMP;
        PlayerEvents.ON_PLAYER_PICKUP += ON_PICK_UP;
        PlayerEvents.ON_PLAYER_DROP += ON_DROP;
        PlayerEvents.PLAYER_ISGROUNDED += CHECK_PLAYER_IS_GROUNDED;
    }

    private void OnDestroy()
    {
        PlayerEvents.ON_PLAYER_JUMP -= ON_PLAYER_JUMP;
        PlayerEvents.ON_PLAYER_PICKUP -= ON_PICK_UP;
        PlayerEvents.ON_PLAYER_DROP -= ON_DROP;
        PlayerEvents.PLAYER_ISGROUNDED -= CHECK_PLAYER_IS_GROUNDED;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CHECK_PLAYER_MOVEMENT(_playerMovement.IsWalking());
    }

    private void ON_PICK_UP()
    {
        _anim.SetTrigger("OnPickUp");
    }

    private void ON_DROP()
    {
        _anim.SetTrigger("OnDrop");
    }
    
    private void ON_PLAYER_JUMP()
    {
        _anim.SetTrigger("OnJump");
    }
    
    private void CHECK_PLAYER_MOVEMENT(bool isMoving)
    {
        _anim.SetBool("IsRunning", isMoving);
    }

    private void CHECK_PLAYER_IS_GROUNDED(bool isGrounded)
    {
        _anim.SetBool("IsGrounded", isGrounded);
    }
}
