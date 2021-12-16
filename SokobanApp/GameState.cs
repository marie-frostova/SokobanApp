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

        public int CurrentLevel { get => currentLevel+1; }

        private void LoadLevel(int levelIndex)
        {
            LoadLevel(MapCreator.Levels[levelIndex]);
            currentLevel = levelIndex;
        }

        public void LoadLevel(string map)
        {
            Game.CreateMap(map);
            for (var i = 0; i < Game.MapWidth; ++i)
            {
                for (var j = 0; j < Game.MapHeight; ++j)
                {
                    var entity = Game.Map[i, j].Entity;
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
            var map = Game.Map;
            var strBuilder = new StringBuilder("");
            for (var j = 0; j < Game.MapHeight; ++j)
            {
                for (var i = 0; i < Game.MapWidth; ++i)
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
            var commandList = player.Act(playerX, playerY, dir);
            if (commandList == null)
                return;
            var map = Game.Map;
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
            Game.CheckIsOver();
        }
    }
}
