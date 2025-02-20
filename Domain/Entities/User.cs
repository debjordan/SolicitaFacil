namespace SolicitaFacil.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Age { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<Service> Services { get; private set; } = new List<Service>();
    public Subscription? Subscription { get; private set; }

    public User(string name, string email, string password, string phoneNumber, string role, int age = 0)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        Role = role ?? throw new ArgumentNullException(nameof(role));
        Age = age;
        CreatedAt = DateTime.UtcNow;
    }

    // Para EF Core
    private User() { }

    public void Update(string name, string email, string phoneNumber, string role)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        Role = role ?? throw new ArgumentNullException(nameof(role));
    }
}