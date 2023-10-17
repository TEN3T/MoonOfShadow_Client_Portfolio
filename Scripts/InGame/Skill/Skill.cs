using System.Collections;

public interface Skill
{
    //public abstract void Init();
    public abstract IEnumerator Activation();
    public IEnumerator SkillActivation();
    //public void DeActivation();
    //public void SkillUpdate();
    //public void SkillDataUpdate();
    //public void SkillDataUpdate(float coolTime, int count, float damage, float speed, float splashRange, float size);
    public void SkillLevelUp();
    public void SetSkillData(int skillId);
}
