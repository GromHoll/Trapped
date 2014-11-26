using UnityEngine;
using System.Collections;
using TrappedGame;

public class HeroController : MonoBehaviour {
    
    public static readonly string IS_DEAD_KEY = "IsDead";
    
    private Animator aminator;
    private Hero hero; 

    void Start() {
        aminator = GetComponent<Animator>();
	}
	
    void Update() {
        aminator.SetBool(IS_DEAD_KEY, IsDead());
	}

    public void SetHero(Hero hero) {
        this.hero = hero;
    }

    private bool IsDead() {
        return hero != null ? hero.IsDead() : false;
    }

}
