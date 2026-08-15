/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * First update : 2025/10/10
 * Last update: 2026/07/11 by ジャンウォンソク
 * 
 * 
 * プレイヤーのアニメーション
 * 
 */
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator charaAnimator;
    public Animator weaponAnimator;

    private static readonly int BlendTreeHash = Animator.StringToHash("BlendTree");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float BlendTreeFadeDuration = 0.2f;

    private static readonly int StepHash = Animator.StringToHash("Step");
    private static readonly int ForwardDirectionHash = Animator.StringToHash("ForwardDirection");
    private static readonly int RightDirectionHash = Animator.StringToHash("RightDirection");
    private const float StepFadeDuration = 0.03f;

    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private const float JumpFadeDuration = 0.05f;

    private static readonly int SuperStandByHash = Animator.StringToHash("SuperStandBy");
    private static readonly int SuperAttackHash = Animator.StringToHash("SuperAttack");
    private static readonly int SuperStayHash = Animator.StringToHash("SuperStay");
    private const float SuperFadeDuration = 0.1f;

    private static readonly int DeadHash = Animator.StringToHash("Dead");
    private const float DeadFadeDuration = 0.1f;

    private static readonly int ParryHash = Animator.StringToHash("Parry");
    private static readonly int CounterHash = Animator.StringToHash("Counter");

    public void PlayMotion(int stateHash, float duration)
    {
        charaAnimator.CrossFadeInFixedTime(stateHash, duration);
    }

    public void PlayMotion(string tag, float duration)
    {
        int hash = Animator.StringToHash(tag);
        charaAnimator.CrossFadeInFixedTime(hash, duration);
    }

    public void PlayWeaponMotion(string tag, float duration)
    {
        int hash = Animator.StringToHash(tag);
        weaponAnimator.CrossFadeInFixedTime(hash, duration);
    }

    private void PlayWeaponMotion(int stateHash, float duration)
    {
        weaponAnimator.CrossFadeInFixedTime(stateHash, duration);
    }

    //ジャンプ
    public void Jump()
    {
        PlayMotion(JumpHash, JumpFadeDuration);
    }

    //移動
    public void Move(float speed)
    {
        this.charaAnimator.SetFloat(SpeedHash, speed);
        this.weaponAnimator.SetFloat(SpeedHash, speed);
    }

    //放置
    public void Idle()
    {
        PlayMotion(BlendTreeHash, BlendTreeFadeDuration);
        PlayWeaponMotion(BlendTreeHash, BlendTreeFadeDuration);
    }

    //回避
    public void Step(float _forward, float _right)
    {
        PlayMotion(StepHash, StepFadeDuration);
        this.charaAnimator.SetFloat(ForwardDirectionHash, _forward);
        this.charaAnimator.SetFloat(RightDirectionHash, _right);
    }

    //必殺技
    public void SuperStandby()
    {
        PlayMotion(SuperStandByHash, SuperFadeDuration);
        //this.weaponAnimator.SetTrigger("Super_Enter");
    }

    public bool isSuperStandbyEnded()
    {
        AnimatorStateInfo charaStateInfo = this.charaAnimator.GetCurrentAnimatorStateInfo(0);

        return (charaStateInfo.shortNameHash == SuperStandByHash && charaStateInfo.normalizedTime >= 1.0f)
                || charaStateInfo.shortNameHash == SuperStayHash;
    }

    public void SuperAttacking()
    {
        PlayMotion(SuperAttackHash, SuperFadeDuration);
        //this.weaponAnimator.SetTrigger("Super_Enter");
    }

    public void SuperStay()
    {
        PlayMotion(SuperStayHash, SuperFadeDuration);
    }

    //死亡
    public void Dead()
    {
        PlayMotion(DeadHash, DeadFadeDuration);
    }

    //パリー
    public void Parry()
    {
        PlayMotion(ParryHash, 0.1f);
    }

    public void Counter()
    {
        PlayMotion(CounterHash, 0.1f);
    }

    public float GetNormalizedTime(string tag)
    {
        AnimatorStateInfo currentInfo = charaAnimator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo    = charaAnimator.GetNextAnimatorStateInfo(0);

        if (charaAnimator.IsInTransition(0) && nextInfo.IsTag(tag))
            return nextInfo.normalizedTime;

        if (currentInfo.IsTag(tag))
        return currentInfo.normalizedTime;

        return 0f;
    }
}
