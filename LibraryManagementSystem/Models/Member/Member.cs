using LibraryManagementSystem.Models.Books;
using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Member;

// uniqueness = email
internal abstract class Member
{
    public const int MEMBER_ID_LENGTH = 8;

    public enum MemberType
    {
        Student,
        Teacher
    }

    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty; // unique prop

    protected HashSet<string> BorrowedBookIds { get; set; } = [];
    public static readonly List<string> MemberTypeNames = [.. Enum.GetNames(typeof(MemberType))];
    public MemberType Type { get; protected set; }
    public string Name => $"{FirstName} {LastName}";
    public string MemberId { get; } = CustomUtils.GenerateUniqueID(0, MEMBER_ID_LENGTH);

    public string FirstName
    {
        get => _firstName;
        protected set => _firstName = !string.IsNullOrWhiteSpace(value)
            ? value.Trim().ToLower()
            : string.Empty;
    }

    public string LastName
    {
        get => _lastName;
        protected set => _lastName = !string.IsNullOrWhiteSpace(value)
            ? value.Trim().ToLower()
            : string.Empty;
    }

    public string Email
    {
        get => _email;
        private set => _email = !Validator.IsEmail(value)
                ? throw new ArgumentException($"Can't set an invalid email: '{value}' while creating system member.")
                : value.Trim().ToLower();
    }

    protected Member(string firstName, string lastName, string email, MemberType type)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Type = type;
    }

    // uniqueness = email
    public override bool Equals(object obj) => obj is Member member && Email.Equals(member.Email.Trim(), StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Email.GetHashCode();

    public override string ToString()
        => $"Member details:\n\tid: '{MemberId}'" +
            $"\n\tname: '{Name}'" +
            $"\n\temail: '{Email}'" +
            $"\n\ttype: '{Type}'";

    public static MemberType? SelectType(string message = "Use the arrow keys to navigate and press Enter to select member type:")
    {
        string input = MenuSelector.SelectOption(MemberTypeNames, message);
        bool isValidType = Enum.TryParse(input, false, out MemberType memberType);
        return isValidType ? memberType : null;
    }

    public bool FindIfBookBorrowedByMemberByBooKId(string id, out bool error)
    {
        error = !Book.IsValidId(id);

        return !error && BorrowedBookIds.Contains(id);
    }

    public bool TryReturnBook(string id, out bool error)
    {
        error = !Book.IsValidId(id);

        return !error && BorrowedBookIds.Remove(id);
    }

    public bool BorrowBook(string id, out bool error)
    {
        error = string.IsNullOrWhiteSpace(id) || id.Length != Book.BOOK_ID_LENGTH;

        return !error && BorrowedBookIds.Add(id);
    }
}