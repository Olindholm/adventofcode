using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode {
    class DonutMazePortal {

        string Name;
        Point2D PortalPosition, DestinationPosition;
        int LevelIncrement;

        public DonutMazePortal(string name, Point2D portalPosition, Point2D destinationPosition, int levelIncrement = 0) {
            Name = name;
            PortalPosition = portalPosition;
            DestinationPosition = destinationPosition;
            LevelIncrement = levelIncrement;
        }

        public string GetName() {
            return Name;
        }

        public Point2D GetPortalPosition() {
            return PortalPosition;
        }

        public Point2D GetDestinationPosition() {
            return DestinationPosition;
        }

        public int GetLevelIncrement(bool recursiveSpaces = false) {
            return recursiveSpaces ? LevelIncrement : 0;
        }

        public DonutMazeState Use(DonutMazeState state, bool recursiveSpaces = false) {
            if (!state.GetPosition().Equals(GetPortalPosition()))
                throw new Exception("Cannot use portal unless on it's position!");

            return new DonutMazeState(GetDestinationPosition(), state.GetLevel() + GetLevelIncrement(recursiveSpaces));
        }

        override public string ToString() {
            return String.Format("Portal( Pos: {0}, Dest: {1}, Lvl Inc: {2} )", GetPortalPosition(), GetDestinationPosition(), GetLevelIncrement(true));
        }
    }
}
