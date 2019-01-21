public class ExtendPaddle : Collectable
{
    public float NewWidth = 2.5f;

    protected override void ApplyEffect()
    {
        Paddle thePaddle = FindObjectOfType<Paddle>();
        if (thePaddle != null && !thePaddle.PaddleIsTransforming)
        {
            thePaddle.StartWidthAnimation(NewWidth);
        }
    }
}
