using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameParameters")]
public class GameParameters : ScriptableObject {

    [Header("Gameplay parameters")]
    // Values
    public float ROUND_DURATION;
    public float COOLDOWN_CARRY;
    public int SCORE_MODIFIER_BY_SECOND;

    //Players
    public List<int> PLAYERS_INDEXES = new List<int>();

    [Header("Prefabs")]
    // Prefabs
    public GameObject CHARACTER_PLAYER_PREFAB;
    public GameObject CHARACTER_BOT_PREFAB;
    public GameObject ITEM_PREFAB;
    public GameObject NEUTRAL_ITEM_PREFAB;
    
    [Header("Colors")]
    // Colors
    public Color[] COLORS_PLAYER;
    public Color COLOR_NEUTRAL = Color.clear;

    [Header("Animation")]
    public float INTRO_ANIMATION_DURATION;

}
