using UnityEngine;

public class SPlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _swordAnimator;
    [SerializeField]
    private AnimationClip _swordAttackClip;

    private SPlayerInput _input;

    private void Awake()
    {
        _input = GameObject.Find("Player").GetComponent<SPlayerInput>();
    }

    private void Update()
    {
        if (_input.Attacking)
        {
            PlayAttack();
            _input.Attacking = false;
        }
    }

    public void PlayAttack()
    {
        _swordAnimator.SetTrigger("Attack Trigger");
    }
}
