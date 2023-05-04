using NUnit.Framework;
using Sokoban;
using SokobanApp;

namespace Tests
{

    [TestFixture]
    public class TestSokoban
    {
        [Test]
        public void WhenBoxMovesOnFinish_TransformToFinishedBox()
        {
            var startState = new GameState(@"
#####
#@o+#
#####"
        );
            var expectedEndState = @"
#####
# @O#
#####";
            startState.Move(Direction.right);
            var actualEndState = startState.GetMap();
            Assert.AreEqual(expectedEndState.Trim(), actualEndState.Trim());
        }

        [Test]
        public void CantMoveToWall()
        {
            var startState = new GameState(@"
###
#@#
# #
###"
        );
            var expectedEndState = @"
###
#@#
# #
###";
            startState.Move(Direction.down);
            var actualEndState = startState.GetMap();
            Assert.AreEqual(expectedEndState.Trim(), actualEndState.Trim());
        }

        [Test]
        public void WhenTwoBoxes_CantMove()
        {
            var startState = new GameState(@"
###
#@#
#o#
#o#
#+#
###"
            );
            var expectedEndState = @"
###
#@#
#o#
#o#
#+#
###";
            startState.Move(Direction.up);
            var actualEndState = startState.GetMap();
            Assert.AreEqual(expectedEndState.Trim(), actualEndState.Trim());
        }

        [Test]
        public void WhenOnlyFinishedBoxes_GameOver()
        {
            var startState = new GameState(@"
#####
#@o+#
#####"
            );
            var expected = true;
            startState.Move(Direction.right);
            var actual = startState.IsOver;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WhenMoveFinishedBoxFromFinish_GameNotOver()
        {
            var startState = new GameState(@"
######
#@o+ #
######"
            );
            var expected = false;
            startState.Move(Direction.right);
            startState.Move(Direction.right);
            var actual = startState.IsOver;
            Assert.AreEqual(expected, actual);
        }
    }
}
