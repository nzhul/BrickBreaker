using Assets.Scripts;

public class FireBall : Collectable
{
    protected override void ApplyEffect()
    {
        foreach (Ball ball in BallsManager.Instance.Balls)
        {
            ball.StartFireBall();
        }
    }
}
