
using JetBrains.Annotations;

namespace SKILLCONSTANT
{
    public enum SKILL_EFFECT
    {
        STUN,
        SLOW,
        KNOCKBACK,
        EXPLORE,
        MOVEUP,
        EXECUTE,
        SPAWNMOB,
        CHANGEFORM,
        BOUNCE,
        DRAIN,
        DELETE,
        RESTRAINT,
        PULL,
        METASTASIS,
    }

    public enum SKILL_PASSIVE
    {
        MAGNET,
        SHIELD,
        MOVESPEED,
        EXP,
        PROJECTILESIZE,
        PROJECTILECOUNT,
        PROJECTILESPLASH,
        PROJECTILESPEED,
        PROJECTILEDISTANCE,
        SKILLDAMAGE,
        HP,
        HPREGEN,
        ATTACK,
        ATTACKSPEED,
        ARMOR,
    }

    public enum SKILL_TARGET
    {
        MELEE,
        FRONT,
        BACK,
        TOP,
        BOTTOM,
        RANDOM,
        BOSS,
        PLAYER,
        //ALL,
        LOWEST,
        MOUSE,
    }

    public enum CALC_MODE
    {
        PLUS,
        MULTI,
    }
}