using HPScreen;
using HPScreen.Admin;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnipop.Entities
{
    public class Showcase
    {
        public Queue<Head> DefeatedHeads { get; set; } = new Queue<Head>();
        public Head CurHead { get; set; }
        public const int DISPLAY_DURATION = 150;
        public const int DISPLAY_DURATION_LAST = 300;
        public int DisplayTimer { get; set; }
        public Head LastHead { get; set; }
        public int LastHeadSize { get; set; }
        public int LastHeadSizeTarget { get; set; }
        public bool LastHeadShowcaseComplete { get; set; }

        public Showcase()
        {
            LastHeadShowcaseComplete = false;
            ResetTimer();
        }
        public void ResetTimer()
        {
            DisplayTimer = DISPLAY_DURATION;
        }
        public void AddHead(Head head)
        {
            head.SetPosition(Graphics.Current.ScreenMidX, Graphics.Current.ScreenHeight + 200);
            DefeatedHeads.Enqueue(head);
        }
        public void SetLastHead(Head head)
        {
            LastHead = head;
            CurHead = null;
            DefeatedHeads.Clear();
            DisplayTimer = DISPLAY_DURATION_LAST;
            LastHeadSize = head.Radius * 2;
            LastHeadSizeTarget = 600;
        }
        public void Update()
        {
            if (LastHead != null)
            {
                DisplayTimer--;
                LastHeadSize = Global.Ease(LastHeadSize, LastHeadSizeTarget, 0.05f);
                LastHead.X = Global.Ease(LastHead.X, Graphics.Current.ScreenMidX, 0.05f);
                LastHead.Y = Global.Ease(LastHead.Y, Graphics.Current.ScreenMidY, 0.05f);
                if (DisplayTimer <= 0)
                {
                    // Reset the showcase
                    LastHead = null;
                    CurHead = null;
                    DefeatedHeads.Clear();
                    LastHeadShowcaseComplete = true;
                    ResetTimer();
                }
                return;
            }
            if (CurHead == null && DefeatedHeads.Count > 0)
            {
                CurHead = DefeatedHeads.Dequeue();
            }
            if (CurHead != null)
            {
                float posy = Global.Ease(CurHead.Y, Graphics.Current.ScreenMidY, 0.075f);
                CurHead.SetPosition(CurHead.X, (int)posy);
                DisplayTimer--;
                if (DisplayTimer <= 0)
                {
                    CurHead = null;
                    ResetTimer();
                }
            }
        }
        public void Draw()
        {
            Graphics.Current.SpriteB.Begin();
            if (CurHead != null)
            {
                int spritesize = 200;
                Rectangle rect = new Rectangle(CurHead.X - spritesize/2, CurHead.Y - spritesize / 2, spritesize, spritesize);
                Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName[CurHead.SpriteName], rect, null, Color.White);
                Graphics.Current.DrawString($"{CurHead.SpriteName} was defeated" , new Vector2(CurHead.X, CurHead.Y + spritesize / 2 + 15), ScreenSaver.SmallWhite, true, true, false, null);}

            if (LastHead != null)
            {
                Rectangle rect = new Rectangle(LastHead.X - LastHeadSize / 2, LastHead.Y - LastHeadSize / 2, LastHeadSize, LastHeadSize);
                Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName[LastHead.SpriteName], rect, null, Color.White);
                Graphics.Current.DrawString($"{LastHead.SpriteName} is victorious!", new Vector2(LastHead.X, LastHead.Y + LastHeadSize / 2 + 30), ScreenSaver.MediumGold, true, true, false, null);
            }
            Graphics.Current.SpriteB.End();
        }
    }
}
