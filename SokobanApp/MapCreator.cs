using System;
using System.Collections.Generic;
using System.Linq;

namespace Sokoban
{
    internal class MapCreator
    {
        public static readonly string[] Levels = new string[]
        {
@"
##########
#        #
# +      #
#  #o o  #
#   ++o@ #
#        #
##########",
@"
  ##### 
###   # 
#+@o  # 
### o+# 
#+##o # 
# # + ##
#o Ooo+#
#   +  #
########",
@"
       ####   
########  ##  
#          ###
# @oo ##   ++#
# oo   ##  ++#
#         ####
###########   ",
@"
  #########  
  #O+O#O+O#  
  #+O+O+O+#  
  #O+O#O+O#  
  #+O+O+O+#  
  #O+O#O+O#  
  ###   ###  
    #   #    
###### ######
#           #
# o o o o o #
## o o o o ##
 #o o o o o# 
 #   o@o   # 
 #  #####  # 
 ####   #### "
        };

        private static readonly Dictionary<Tuple<string, string>, Tuple<Func<ICell>, Func<IEntity>>> CellFactory = new Dictionary<Tuple<string, string>, Tuple<Func<ICell>, Func<IEntity>>>();

        public static ICell[,] CreateMap(string map)
        {
            var rows = map.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong test map '{map}'");
            var result = new ICell[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                    result[x, y] = CreateCellBySymbol(rows[y][x]);
            return result;
        }

        private static ICell CreateCellBySymbol(char c)
        {
            switch (c)
            {
                case '@':
                    return CreateCellByTypeName(typeof(Empty), typeof(Player));
                case 'o':
                    return CreateCellByTypeName(typeof(Empty), typeof(Box));
                case 'O':
                    return CreateCellByTypeName(typeof(Finish), typeof(FinishedBox));
                case '#':
                    return CreateCellByTypeName(typeof(Wall));
                case '+':
                    return CreateCellByTypeName(typeof(Finish));
                case ' ':
                    return CreateCellByTypeName(typeof(Empty));
                default:
                    throw new Exception($"wrong character {c}");
            }
        }

        private static ICell CreateCellByTypeName(Type name, Type entityName = null)
        {
            var key = new Tuple<string, string>(name.Name, entityName == null ? null : entityName.Name);
            if (!CellFactory.ContainsKey(key))
            {
                Func<IEntity> entityCreator = null;
                if (entityName != null)
                    entityCreator = () => (IEntity)Activator.CreateInstance(entityName);
                CellFactory[key] = new Tuple<Func<ICell>, Func<IEntity>>(() => (ICell)Activator.CreateInstance(name), entityCreator);
            }

            ICell res = CellFactory[key].Item1();
            if (CellFactory[key].Item2 != null)
                res.Entity = CellFactory[key].Item2();
            return res;
        }
    }
}