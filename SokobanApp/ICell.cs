namespace Sokoban
{
    public interface ICell
    {
        bool IsFinish();
        IEntity Entity { get; set; }
        string GetImageFileName();
        string GetString();
    }
}
