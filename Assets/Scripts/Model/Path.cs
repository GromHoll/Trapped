using System;
using System.Collections.Generic;
using TrappedGame.Model.Common;

namespace TrappedGame.Model {
    public class Path {

        public class PathLink {
            private IntVector2 from;
            private IntVector2 to;

            public PathLink(int fromX, int fromY, int toX, int toY) {
                from = new IntVector2(fromX, fromY);
                to = new IntVector2(toX, toY);
            }

            public bool IsFrom(int x, int y) {
                return from.x == x && from.y == y;
            }
            
            public IntVector2 GetFrom() {
                return from;
            }

            public int GetFromX() {
                return from.x;
            }

            public int GetFromY() {
                return from.y;
            }

            public IntVector2 GetTo() {
                return to;
            }

            public int GetToX() {
                return to.x;            
            }

            public int GetToY() {
                return to.y;
            }

            public bool IsWentUp() {
                return from.y < to.y;
            }
            
            public bool IsWentRight() {
                return from.x < to.x;
            }
            
            public bool IsWentDown() {
                return from.y > to.y;
            }
            
            public bool IsWentLeft() {
                return from.x > to.x;
            }

            public bool IsAdjacent() {
                return Math.Abs(from.x - to.x) <= 1
                    && Math.Abs(from.y - to.y) <= 1;
            }

            public bool IsVertical() {
                return from.x == to.x;
            }

            public bool IsHorizontal() {
                return from.y == to.y;
            }
        }

        private readonly Stack<PathLink> links = new Stack<PathLink>();

        public void AddLink(int fromX, int fromY, int toX, int toY) {
            links.Push(new PathLink(fromX, fromY, toX, toY));
        }

        public PathLink RemoveLink() {
            return links.Pop();
        }

        public PathLink GetPreviousTurn() {
            return !IsEmpty() ? links.Peek() : null;
        }

        public bool IsEmpty() {
            return links.Count == 0;
        }

        public IEnumerable<PathLink> GetLinks() {
            return links;
        }
    }
}