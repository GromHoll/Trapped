using System;
using UnityEngine;

public class Game {

    private Level level;
    private Hero hero;

    public Game(Level level) {
        if (level == null) throw new ArgumentNullException("level"); 
        this.level = level;
        this.hero = new Hero(level.GetStartX(), level.GetStartY());
    }
}

