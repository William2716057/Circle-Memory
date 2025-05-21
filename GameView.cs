using Android.Content;
using Android.Graphics;
using Android.Views;

namespace visualMemory
{
    public class GameView : View
    {
        private Paint paint;

        public GameView(Context context) : base(context)
        {
            paint = new Paint();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.Black); 
        }
    }
}