using System.Linq;
using UnityEngine;

public class StatManager : MonoBehaviour 
{
    public void ProcessAction(CharacterAction characterAction)
    {
        if(characterAction is Attack)
        {
            processAttack(characterAction as Attack);
        }
    }

    void processAttack(Attack attack)
    {
        foreach(Character target in attack.Targets)
        {
            int damageReduction = 0;
            Defend defend;
            if(target.ActiveAction is Defend && (defend = target.ActiveAction as Defend).Targets.Contains(attack.Actor))
            {
                damageReduction += defend.Defense;
                target.UseActiveAction();
            }
            target.Damage(Mathf.Max(attack.Damage - damageReduction, 0));
        }
    }
}
