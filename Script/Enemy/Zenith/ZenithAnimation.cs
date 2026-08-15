using UnityEngine;

public class ZenithAnimation : MonoBehaviour
{
    private static readonly int MeleeRightHash = Animator.StringToHash("MeleeRight");
    private static readonly int MeleeLeftHash = Animator.StringToHash("MeleeLeft");
    private static readonly int DoubleMeleeHash = Animator.StringToHash("DoubleMelee");

    private static readonly int ShotHash = Animator.StringToHash("Shot");

    private static readonly int BeamHash = Animator.StringToHash("Beam");
    private static readonly int BeamFinishHash = Animator.StringToHash("BeamFinish");

    private static readonly int WingHash = Animator.StringToHash("Wing");
    private static readonly int WingFinishHash = Animator.StringToHash("WingFinish");

    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private static readonly int BackStepHash = Animator.StringToHash("BackStep");

    private static readonly int DashStartHash = Animator.StringToHash("DashStart");
    private static readonly int DashHash = Animator.StringToHash("Dash");
    private static readonly int DashFinishHash = Animator.StringToHash("DashFinish");

    private const float ActionDuration = 0.05f;

    private static readonly int BasicMotionHash = Animator.StringToHash("BasicMotion");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    
    private static readonly int ImpactMotionHash = Animator.StringToHash("Impact");

    private static readonly int DeathMotionHash = Animator.StringToHash("Death");
    private const float IdleDuration = 0.1f;

    [SerializeField] private Animator charaAnimator;

    private void PlayMotion(int stateHash, float transitionDuration)
    {
       charaAnimator.CrossFadeInFixedTime(stateHash, transitionDuration);
    }

    public void Idle()
    {
        PlayMotion(BasicMotionHash, IdleDuration);
    }

    public void SetBlendMotion(float speed)
    {
        this.charaAnimator.SetFloat(SpeedHash, speed);
    }

    public void MeleeRight()
    {
        PlayMotion(MeleeRightHash, ActionDuration);
    }

    public void MeleeLeft()
    {
        PlayMotion(MeleeLeftHash, ActionDuration);
    }

    public void DoubleMelee()
    {
        PlayMotion(DoubleMeleeHash, ActionDuration);
    }

    public void Shot()
    {
        PlayMotion(ShotHash, ActionDuration);
    }

    public void Beam()
    {
        PlayMotion(BeamHash, ActionDuration);
    }

    public void BeamFinish()
    {
        PlayMotion(BeamFinishHash, ActionDuration);
    }

    public void Wing()
    {
        PlayMotion(WingHash, ActionDuration);
    }

    public void WingFinish()
    {
        PlayMotion(WingFinishHash, ActionDuration);
    }

    public void DashStart()
    {
        PlayMotion(DashStartHash, ActionDuration);
    }

    public void Dash()
    {
        PlayMotion(DashHash, ActionDuration);
    }

    public void DashFinish()
    {
        PlayMotion(DashFinishHash, ActionDuration);
    }

    public void BackStep()
    {
        PlayMotion(BackStepHash, ActionDuration);
    }

    public void Impact()
    {
        PlayMotion(ImpactMotionHash, IdleDuration);
    }

    public void Death()
    {
        PlayMotion(DeathMotionHash, IdleDuration);
    }

    public float GetNormalizedTime(string tagName)
    {
        AnimatorStateInfo currentInfo = charaAnimator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = charaAnimator.GetNextAnimatorStateInfo(0);

        if (charaAnimator.IsInTransition(0) && nextInfo.IsTag(tagName))
        {
            return nextInfo.normalizedTime;
        }
        else if (!charaAnimator.IsInTransition(0) && currentInfo.IsTag(tagName))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

}
