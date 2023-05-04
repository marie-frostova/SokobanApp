using Sokoban;
using System.Text;

namespace SokobanApp
{
    public class GameState
    {
        int currentLevel;
        Player player;
        int playerX;
        int playerY;

        public bool IsOver;
        public ICell[,] Map;
        public int MapWidth => Map.GetLength(0);
        public int MapHeight => Map.GetLength(1);
        public void CreateMap(string map)
        {
            Map = MapCreator.CreateMap(map);
            IsOver = false;
        }

        public void CheckIsOver()
        {
            IsOver = true;
            for (var i = 0; i < MapWidth; ++i)
            {
                for (var j = 0; j < MapHeight; ++j)
                {
                    if (Map[i, j].IsFinish() && !(Map[i, j].Entity is FinishedBox))
                    {
                        IsOver = false;
                    }
                }
            }
        }


        public int CurrentLevel { get => currentLevel+1; }

        private void LoadLevel(int levelIndex)
        {
            LoadLevel(MapCreator.Levels[levelIndex]);
            currentLevel = levelIndex;
        }

        public void LoadLevel(string map)
        {
            CreateMap(map);
            for (var i = 0; i < MapWidth; ++i)
            {
                for (var j = 0; j < MapHeight; ++j)
                {
                    var entity = Map[i, j].Entity;
                    if (entity is Player)
                    {
                        player = (Player)entity;
                        playerX = i;
                        playerY = j;
                    }
                }
            }
        }

        public GameState(int level = 0)
        {
            LoadLevel(level);
        }

        public GameState(string map)
        {
            LoadLevel(map);
            currentLevel = -1;
        }

        public string GetMap()
        {
            var map = Map;
            var strBuilder = new StringBuilder("");
            for (var j = 0; j < MapHeight; ++j)
            {
                for (var i = 0; i < MapWidth; ++i)
                {
                    strBuilder.Append(map[i, j].GetString());
                }
                strBuilder.Append("\r\n");
            }
            return strBuilder.ToString();
        }

        public void Restart()
        {
            LoadLevel(currentLevel);
        }

        public void NextLevel()
        {
            LoadLevel(currentLevel + 1);
        }

        public bool HasNextLevel()
        {
            return currentLevel + 1 < MapCreator.Levels.Length;
        }

        public void Move(Direction dir)
        {
            var commandList = player.Act(playerX, playerY, dir, this);
            if (commandList == null)
                return;
            var map = Map;
            foreach (var command in commandList)
            {
                if (command.DeltaX != 0 || command.DeltaY != 0)
                {
                    if (command.Entity == player)
                    {
                        playerX = command.TargetX;
                        playerY = command.TargetY;
                    }

                    map[command.TargetX, command.TargetY].Entity = command.TransformTo ?? command.Entity;
                    map[command.X, command.Y].Entity = null;
                }
            }
            CheckIsOver();
        }
    }
}
