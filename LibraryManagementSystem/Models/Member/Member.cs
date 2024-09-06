using LibraryManagementSystem.Models.Books;
using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Member;

// uniqueness = email
internal abstract class Member
{
    public enum MemberType
    {
        Student,
        Teacher
    }

    private const int MEMBER_ID_LENGTH = 8;
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
        private set
        {
            if (!Validator.IsEmail(value))
                throw new ArgumentException($"Can't set an invalid email: '{value}' while creating system member.");

            _email = value.Trim().ToLower();
        }
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

    public static bool SelectMemberTypeUsingMenuSelector(out MemberType result,
        string message = "Use the arrow keys to navigate and press Enter to select member type:")
    {
        string selectedMemberTypeInput = MenuSelector.SelectOption(MemberTypeNames, message);
        bool isValidSelectedMemberType = Enum.TryParse(selectedMemberTypeInput, false, out MemberType validMemberType);
        result = validMemberType;
        return isValidSelectedMemberType;
    }

    public bool FindIfBookBorrowedByMemberByBooKId(string bookId, out bool validationError)
    {
        validationError = false;

        if (!Book.IsValidId(bookId))
        {
            validationError = true;
            return false;
        }

        return BorrowedBookIds.Contains(bookId);
    }

    public bool TryReturnBook(string bookId, out bool validationError)
    {
        validationError = false;

        if (!Book.IsValidId(bookId))
        {
            validationError = true;
            return false;
        }

        return BorrowedBookIds.Remove(bookId);
    }

    public bool BorrowBook(string bookId, out bool validationError)
    {
        validationError = false;

        if (string.IsNullOrWhiteSpace(bookId))
        {
            validationError = true;
            return false;
        }

        if (bookId.Length != 8)
        {
            validationError = true;
            return false;
        }

        return BorrowedBookIds.Add(bookId);
    }
}