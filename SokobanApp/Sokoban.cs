using System;
using System.Collections.Generic;
using SokobanApp;

namespace Sokoban
{
    public enum Direction { left, right, up, down };

    public class Player : IEntity
    {
        public List<Command> Act(int x, int y, Direction KeyPressed, GameState game)
        {
            var commands = new List<Command>();
            var res = new Command(this, x, y);
            var map = game.Map;
            if (KeyPressed == Direction.up && y < game.MapHeight - 1)
                res.DeltaY += 1;
            else if (KeyPressed == Direction.down && y > 0)
                res.DeltaY -= 1;
            else if (KeyPressed == Direction.right && x < game.MapWidth - 1)
                res.DeltaX += 1;
            else if (KeyPressed == Direction.left && x > 0)
                res.DeltaX -= 1;

            var cell = map[res.DeltaX + x, res.DeltaY + y];
            if (cell is Wall)
            {
                res.DeltaX = 0;
                res.DeltaY = 0;
            }
            else if (cell.Entity != null && cell.Entity.IsMovable())
            {
                var nextCell = map[2 * res.DeltaX + x, 2 * res.DeltaY + y];
                if (!(nextCell is Empty) || nextCell.Entity != null)
                {
                    res.DeltaX = 0;
                    res.DeltaY = 0;
                }
                Command move = new Command(cell.Entity, x + res.DeltaX, y + res.DeltaY);
                move.DeltaX = res.DeltaX;
                move.DeltaY = res.DeltaY;
                move.TransformTo = cell.Entity.Transform(nextCell);
                commands.Add(move);
            }

            commands.Add(res);
            return commands;
        }

        public string GetImageFileName()
        {
            return "Player.png";
        }

        public string GetString()
        {
            return "@";
        }

        public bool IsMovable()
        {
            return false;
        }

        public IEntity Transform(ICell cell)
        {
            return this;
        }
    }

    public class Box : IEntity
    {
        public virtual string GetImageFileName()
        {
            return "Box.png";
        }

        public virtual string GetString()
        {
            return "o";
        }

        public bool IsMovable()
        {
            return true;
        }
        public IEntity Transform(ICell cell)
        {
            return cell.IsFinish() ? new FinishedBox() : new Box();
        }
    }

    public class FinishedBox : Box
    {
        public override string GetImageFileName()
        {
            return "FinishedBox.png";
        }

        public override string GetString()
        {
            return "O";
        }
    }

    public class Wall : ICell
    {
        IEntity ICell.Entity { get => null; set => throw new NotImplementedException(); }

        public string GetImageFileName()
        {
           return "Wall.png";
        }

        public string GetString()
        {
            return "#";
        }

        public bool IsFinish()
        {
            return false;
        }
    }

    public class Empty : ICell
    {
        protected IEntity entity;

        IEntity ICell.Entity { get => entity; set => entity = value; }

        public virtual string GetImageFileName()
        {
            return entity == null ? null : entity.GetImageFileName();
        }

        public virtual string GetString()
        {
            return entity == null ? " " : entity.GetString();
        }

        public virtual bool IsFinish()
        {
            return false;
        }
    }

    public class Finish : Empty
    {
        public override bool IsFinish()
        {
            return true;
        }

        public override string GetString()
        {
            return entity == null ? "+" : entity.GetString();
        }

        public override string GetImageFileName()
        {
            return entity == null ? "Finish.png" : entity.GetImageFileName();
        }
    }
}
