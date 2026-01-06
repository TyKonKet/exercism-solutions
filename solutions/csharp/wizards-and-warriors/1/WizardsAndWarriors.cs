abstract class Character
{
    protected Character(string characterType) { }

    public abstract int DamagePoints(Character target);

    public virtual bool Vulnerable() => false;
}

class Warrior : Character
{
    public Warrior() : base("Warrior") { }

    public override int DamagePoints(Character target) => target.Vulnerable() ? 10 : 6;

    public override string ToString() => "Character is a Warrior";
}

class Wizard : Character
{
    private bool spellReady = false;
    
    public Wizard() : base("Wizard") { }

    public override int DamagePoints(Character target) => spellReady ? 12 : 3;

    public void PrepareSpell() => spellReady = true;

    public override bool Vulnerable() => !spellReady;
    
    public override string ToString() => "Character is a Wizard";
}
