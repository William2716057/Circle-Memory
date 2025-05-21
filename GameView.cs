using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;

namespace visualMemory
{
    public class Circle
    {
        public RectF Bounds;
        public bool IsClickable;

        public Circle(RectF bounds, bool isClickable)
        {
            Bounds = bounds;
            IsClickable = isClickable;
        }

        public bool Contains(float x, float y)
        {
            float cx = Bounds.CenterX();
            float cy = Bounds.CenterY();
            float radius = Bounds.Width() / 2;
            float dx = x - cx;
            float dy = y - cy;
            return dx * dx + dy * dy <= radius * radius;
        }
    }

    public class GameView : View
    {
        private Paint paint;
        private List<Circle> circles;
        private Random rand;
        private const float CircleSize = 150f;
        private bool isGameOver = false;

        public GameView(Context context) : base(context)
        {
            paint = new Paint();
            circles = new List<Circle>();
            rand = new Random();

            AddNewCircle();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            canvas.DrawColor(Color.Black);

            foreach (var c in circles)
            {
                paint.Color = c.IsClickable ? Color.White : Color.White;
                canvas.DrawOval(c.Bounds, paint);
            }
            if (isGameOver)
            {
                paint.Color = Color.White;
                paint.TextSize = 80;
                paint.TextAlign = Paint.Align.Center;
                canvas.DrawText("Game Over", Width / 2, Height / 2, paint);
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                float x = e.GetX();
                float y = e.GetY();

                var active = circles.FindLast(c => c.IsClickable);
                if (active != null && active.Contains(x, y))
                {
                    AddNewCircle();
                }
                else
                {
                    EndGame();
                }
                return true;
            }

            return base.OnTouchEvent(e);
        }

        private void AddNewCircle()
        {
            // Disable existing circles
            foreach (var c in circles)
                c.IsClickable = false;

            // Circle fits within screen bounds
            float maxX = Width - CircleSize;
            float maxY = Height - CircleSize;

            if (maxX <= 0 || maxY <= 0)
            {
                // View size not yet ready; try again later
                PostDelayed(AddNewCircle, 100);
                return;
            }

            float x = (float)(rand.NextDouble() * maxX);
            float y = (float)(rand.NextDouble() * maxY);
            var bounds = new RectF(x, y, x + CircleSize, y + CircleSize);

            circles.Add(new Circle(bounds, true));
            Invalidate();
        }

        private void EndGame()
        {
            isGameOver = true;
            Invalidate(); // Redraw the screen to show "Game Over"
        }
    }
}