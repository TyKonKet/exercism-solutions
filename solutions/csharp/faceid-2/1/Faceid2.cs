public class FacialFeatures
{
    public string EyeColor { get; }
    public decimal PhiltrumWidth { get; }

    public FacialFeatures(string eyeColor, decimal philtrumWidth)
    {
        EyeColor = eyeColor;
        PhiltrumWidth = philtrumWidth;
    }

    public override bool Equals(object? obj)
    {
        if (obj is FacialFeatures other)
        {
            return EyeColor == other.EyeColor && PhiltrumWidth == other.PhiltrumWidth;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(EyeColor);
        hash.Add(PhiltrumWidth);
        return hash.ToHashCode();
    }
}

public class Identity
{
    public string Email { get; }
    public FacialFeatures FacialFeatures { get; }

    public Identity(string email, FacialFeatures facialFeatures)
    {
        Email = email;
        FacialFeatures = facialFeatures;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Identity other)
        {
            return Email == other.Email && FacialFeatures.Equals(other.FacialFeatures);
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Email);
        hash.Add(FacialFeatures);
        return hash.ToHashCode();
    }
}

public class Authenticator
{
    private readonly List<Identity> _registeredIdentities = [];

    private static readonly Identity AdminIdentity = new("admin@exerc.ism", new FacialFeatures("green", 0.9m));

    public static bool AreSameFace(FacialFeatures faceA, FacialFeatures faceB) => faceA.Equals(faceB);

    public bool IsAdmin(Identity identity) => identity.Equals(AdminIdentity);

    public bool Register(Identity identity)
    {
        if (IsRegistered(identity))
            return false;

        _registeredIdentities.Add(identity);
        return true;
    }

    public bool IsRegistered(Identity identity) => _registeredIdentities.Contains(identity);

    public static bool AreSameObject(Identity identityA, Identity identityB) => ReferenceEquals(identityA, identityB);
}
