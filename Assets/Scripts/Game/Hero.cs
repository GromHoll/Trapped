using System;
using UnityEngine;


namespace TrappedGame {
    public class Hero {

        private IntVector2 position;
        private Path path = new Path();

        private bool isDead = false;
        private int deadCounter = 0;

        public Hero(int x, int y) {
            position = new IntVector2(x, y);
        }

        public int GetX() {
            return position.x;
        }

        public int GetY() {
            return position.y;
        }

        public void MoveTo(int x, int y) {
            path.AddLink(position.x, position.y, x, y);
            position.x = x;
            position.y = y;
        }

        public void MoveBack() {
            if (!path.IsEmpty()) {
                var link = path.RemoveLink();
                position.x = link.GetFromX();
                position.y = link.GetFromY();
            }
        }
        
        public void SetDead(bool isDead) {
            this.isDead = isDead;
            if (isDead) {
                deadCounter++;
            }
        }

        public bool IsDead() {
            return isDead;
        }

        public int GetDeaths() {
            return deadCounter;
        }

        public Path GetPath() {
            return path;
        }

        public Path.PathLink GetPreviousTurn() {
            return path.GetPreviousTurn();
        }

        public bool WasHere(int x, int y) {
            foreach (var link in path.GetLinks()) {
                if (link.GetFromX() == x && link.GetFromY() == y) {
                    return true;
                }
            }
            return false;
        }


    }
}