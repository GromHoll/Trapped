using System;
using System.Collections.Generic;
using TrappedGame.Model.Common;

namespace TrappedGame.Model {
    public class Path {

        public class PathLink {
            public IntVector2 From { get; private set; }
            public int FromX { get { return From.x; } }
            public int FromY { get { return From.y; } }

            public IntVector2 To { get; private set; }
            public int ToX { get { return To.x; } }
            public int ToY { get { return To.y; } }

            public PathLink PreviousLink { get; private set; }

            public PathLink(int fromX, int fromY, int toX, int toY, PathLink previousLink) {
                From = new IntVector2(fromX, fromY);
                To = new IntVector2(toX, toY);
                PreviousLink = previousLink;
            }

            public PathLink Reverse() {
                return new PathLink(ToX, ToY, FromX, FromY, this);
            }

            public bool IsFrom(int x, int y) {
                return FromX == x && FromY == y;
            }

            public bool IsWentUp() {
                return FromY < ToY;
            }
            
            public bool IsWentRight() {
                return FromX < ToX;
            }
            
            public bool IsWentDown() {
                return FromY > ToY;
            }
            
            public bool IsWentLeft() {
                return FromX > ToX;
            }

            public bool IsAdjacent() {
                return Math.Abs(FromX - ToX) <= 1
                    && Math.Abs(FromY - ToY) <= 1;
            }

            public bool IsVertical() {
                return FromX == ToX;
            }

            public bool IsHorizontal() {
                return FromY == ToY;
            }
        }

        private readonly Stack<PathLink> links = new Stack<PathLink>();
        public IEnumerable<PathLink> Links { get { return links; } }

        public PathLink AddLink(int fromX, int fromY, int toX, int toY) {
            var pathLink = new PathLink(fromX, fromY, toX, toY, GetPreviousTurn());
            links.Push(pathLink);
            return pathLink;
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
    }
}