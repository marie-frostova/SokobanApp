namespace Sokoban
{
    public interface IEntity
    {
        string GetImageFileName();

        bool IsMovable();

        IEntity Transform(ICell targetCell);

        string GetString();
    }
}
