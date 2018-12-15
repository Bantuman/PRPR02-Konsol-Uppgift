using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Battleships.Objects.Projectile;

namespace Battleships.Objects
{
    public class AIPlayer : Ship
    {
        bool moveTo;

        public AIPlayer(Vector2 position, Color color) : base(null, position)
        {
            moveTo = position.X > 0;
            SetMissileGuideAI(GuideMissile);
        }

        private void MoveToPoint(Vector2 targetPosition)
        {
            if (Energy > 200)
                Acceleration = Vector2.Normalize(targetPosition - Position) * 40f;
            else
                Acceleration = Vector2.Zero;
        }
        
        private void AimTowards(Vector2 targetPosition, int shipSide = 1)
        {
            Acceleration = MathLibrary.ConstructVector(MathLibrary.Direction(targetPosition - Position) + (MathHelper.PiOver2 * shipSide));
        }

        float timer = 0;

        public override void Act()
        {
            timer += 0.01f;

            if (timer > 2f)
            {
                AimTowards(EnemyShip.Position);
                SetShooting(true);
                if (timer > 2.5f)
                {
                    timer = 0;
                }
            }
            else
            {
                SetShooting(false);
                if (MissileCount > 0)
                {
                    LaunchMissile(MathLibrary.Direction(EnemyShip.Position - Position));
                }
                if (moveTo && EnemyShip != null)
                {
                    MoveToPoint(new Vector2(-100, -100));
                }
                else
                {
                    MoveToPoint(new Vector2(0, Position.Y));
                }
            }
        }

        public void GuideMissile(GameTime deltaTime, IMissile missile)
        {
            missile.RotateTo(MathLibrary.Direction(EnemyShip.Position - missile.Position));
        }
    }
}