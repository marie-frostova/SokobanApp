namespace Sokoban
{
    public class Command
    {
        public Command(IEntity entity, int x, int y)
        {
            Entity = entity;
            X = x;
            Y = y;
        }

        public IEntity Entity;
        public int X;
        public int Y;
        public int DeltaX;
        public int DeltaY;
        public IEntity TransformTo;

        public int TargetX { get => X + DeltaX; }
        public int TargetY { get => Y + DeltaY; }
    }
}
