public class Shrink : Collectable
{
    public float NewWidth = .8f;

    protected override void ApplyEffect()
    {
        Paddle thePaddle = FindObjectOfType<Paddle>();
        if (thePaddle != null && !thePaddle.PaddleIsTransforming)
        {
            thePaddle.StartWidthAnimation(NewWidth);
        }
    }
}