using LibraryManagementSystem.Interfaces.Infrastructure.Services;
using LibraryManagementSystem.Models.Books;
using LibraryManagementSystem.Models.Member;
using LibraryManagementSystem.Utils;
using System.Collections.ObjectModel;

namespace LibraryManagementSystem.Services;

internal class LibManagementSystem : ILibraryService
{
    private HashSet<Member> Members { get; } = [];
    private HashSet<Book> Books { get; } = [];

    public long PhysicalBookCount => Books.Count(x => x is PhysicalBook);
    public long EBookCount => Books.Count(x => x is EBook);
    public long StudentCount => Members.Count(x=> x is StudentMember);
    public long TeacherCount => Members.Count(x => x is TeacherMember);
    public long MembersCount => Members.Count;
    public long BorrowedPhysicalBooks => Books.Count(x => x.IsBorrowed && x is PhysicalBook);
    public long BorrowedEBooks => Books.Count(x => x.IsBorrowed && x is EBook);

    public ReadOnlyCollection<string> BookTitles => new([.. Books.Select(x => x.Title)]);
    public long BooksCount => Books.Count;

    //methods
    public void RegisterMember()
    {
        // TODO: Validate member input details

        // member first name input
        Write("Enter first name: ");
        string firstName = ReadLine().Trim().ToLower();

        // validation: first name input
        if (string.IsNullOrWhiteSpace(firstName))
        {
            WriteLine($"[Invalid Input]: member's firstName can't be empty, or only contains whitespace, entered value = '{firstName}'");
            return;
        }

        // member last name input
        Write("Enter last name: ");
        string lastName = ReadLine().Trim().ToLower();

        // validation: last name input
        if (string.IsNullOrWhiteSpace(lastName))
        {
            WriteLine($"[Invalid Input]: member's lastName can't be empty, or only contains whitespace, entered value = '{lastName}'");
            return;
        }

        // member email input
        Write("Enter email: ");
        string email = ReadLine().Trim();

        // validation: email input
        if (!Validator.IsEmail(email))
        {
            WriteLine($"[INVAID INPUT]: Received invalid email, entered value = '{email}'");
            return;
        }

        // taking valid member type input and validating it
        bool memberTypeSelection = Member.SelectMemberTypeUsingMenuSelector(out Member.MemberType memberType);
        if (!memberTypeSelection)
        {
            WriteLine("[ERROR]: Error while selecting member type");
            return;
        }

        // registering members according to selected member type
        if (memberType == Member.MemberType.Student)
        {
            StudentMember newMember = new(firstName, lastName, email);
            bool memberRegistered = Members.Add(newMember);
            if (!memberRegistered)
            {
                WriteLine($"[ALERT]: member with email = '{email.Trim().ToLower()}' has already been registered in the system!!");
                return;
            }

            WriteLine("[SUCCESS]: Student member successfully registered with following details: !!");
            WriteLine(newMember);
        }
        else if (memberType == Member.MemberType.Teacher)
        {
            TeacherMember newMember = new(firstName, lastName, email);
            bool memberRegistered = Members.Add(newMember);
            if (!memberRegistered)
            {
                WriteLine($"[ALERT]: member with email = '{email.Trim().ToLower()}' has already been registered in the system!!");
                return;
            }

            WriteLine("[SUCCESS]: Teacher member successfully registered with following details: !!");
            WriteLine(newMember);
            return;
        }
        else
        {
            WriteLine("[ERROR]: Received invalid member type while registering member in the system.");
        }
    }

    // bool to check if some kind of error while finding member or if member exists or not
    private bool FindMemberByEmail(string email, out Member result)
    {
        bool operationSuccess = false;
        result = null;

        if (!Validator.IsEmail(email))
            return operationSuccess;

        email = email.Trim();

        // checking if member exists
        Member member = Members.FirstOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (member != null)
        {
            operationSuccess = true;
            result = member;
        }

        return operationSuccess;
    }

    public void AddBook()
    {
        string title = Ask("Enter book title: ");
        string author =  Ask("Enter book author: ");

        switch (Book.SelectType())
        {
            case Book.BookType.Physical:
                HandleAddPhysical(title, author);
                break;
            case Book.BookType.EBook:
                HandleAddEBook(title, author);
                break;
            default:
                WriteLine("[ERROR]: Error during book type selection.");
                break;
        }
        ReadKey();
    }
    private void HandleAddPhysical(string title, string author)
    {
        string bookShelfLocation = Ask("Enter book shelfLocation: ");
        Book newBook = new PhysicalBook(title: title, author: author, shelfLocation: bookShelfLocation);

        WriteLine(!Books.Add(newBook)
                ? "[ALERT]: Physical book with the following details already exists in the system!!"
                : "[SUCCESS] : Physical book has been successfully added to the system!!)");
        WriteLine(newBook);
    }
    private void HandleAddEBook(string title, string author)
    {
        string downloadLinkInput = Ask("Enter book download link: ");

        bool isValidDownloadLink = Validator.IsURL(downloadLinkInput, out string downloadLink);
        if (!isValidDownloadLink)
        {
            WriteLine($"[Invalid Input]: book download link URL is invalid, entered value = '{downloadLinkInput}'");
        }
        else
        {
            Book newBook = new EBook(title: title, author: author, downloadLink: downloadLink);

            // checking if book already exists in the "books" hash set, if not book added then it already exists
            WriteLine(!Books.Add(newBook)
                ? "[ALERT]: E-book book with the following details already exists in the system!!"
                : "[SUCCESS] : E-book book has been successfully added to the system!!)");
            WriteLine(newBook);
        }
    }

    public void BorrowBook()
    {
        string email = Ask("Enter member email: ", isEmail: true);

        // checking if member exists
        if (!FindMemberByEmail(email, out Member member))
        {
            WriteLine($"[NOT FOUND ERROR]: Operation failed because member with email = '{email}' doesn't exists in the system");
            WriteLine("[SYSTEM SUGGESTION]: Signup by creating a new member in the system");
            ReadKey();
            return;
        }

        string title = Ask("Enter book title: ");
        string author = Ask("Enter book author: ");
        Book.BookType? bookType = Book.SelectType();
        Book book = Books.FirstOrDefault(_book => _book.Title.Equals(title) && _book.Author.Equals(author) && _book.Type.Equals(bookType));

        if (bookType == null)
        {
            WriteLine("[ERROR]: Error while book type selection.");
        }
        else if (book == null)
        {
            WriteLine("[NOT FOUND]: book with entered details doesn't exist in the system!!");
        }
        else if (book.IsBorrowed)
        {
            WriteLine("[ALERT]: book with entered details has already been borrowed!!");
        }
        else if (!member.BorrowBook(bookId: book.BookId, out bool validationError))
        {
            WriteLine(validationError
                ? "[SYSTEM ERROR]: Error while borrowing book to member"
                : "[ALERT]: Book with given details has already been borrowed by the member");
        }
        else
        {
            book.Borrow();
            WriteLine($"[SUCCESS]: Book with title: '{title}' has been successfully borrowed by member with name: '{member.Name}' and email: '{member.Email}'");
        }

        ReadKey();
    }

    public void ReturnBook()
    {
        string memberEmail = Ask("Enter member email: ", isEmail: true);

        if (!FindMemberByEmail(memberEmail, out Member member))
        {
            WriteLine($"[NOT FOUND ERROR]: Operation failed because member with email = '{memberEmail}' doesn't exists in the system");
            WriteLine("[SYSTEM SUGGESTION]: Signup by creating a new member in the system");
            return;
        }

        string title = Ask("Enter book title: ");
        string author = Ask("Enter book author: ");
        Book.BookType? bookType = Book.SelectType();
        Book book = Books.FirstOrDefault(_book => _book.Title.Equals(title) && _book.Author.Equals(author) && _book.Type.Equals(bookType));

        if (bookType == null)
        {
            WriteLine("[ERROR]: Error while book type selection.");
        }
        else if (book == null)
        {
            WriteLine("[NOT FOUND]: book with entered details doesn't exist in the system!!");
        }
        else if (!book.IsBorrowed)
        {
            WriteLine("[ERROR]: book with entered details was never borrowed!!");
        }
        else if (!member.TryReturnBook(book.BookId, out bool validationError))
        {
            WriteLine(validationError
                ? "[SYSTEM ERROR]: Error while doing operation"
                : "[ERROR]: Book wasn't borrowed by you!!");
        }
        else
        {
            book.Return();
            WriteLine($"Book with given details and borrowed by '{member.Name}' has been successfully returned!");
        }
        ReadKey();
    }

    public void ConsoleAllBookTitles()
    {
        WriteLine(BookTitles.Count == 0
            ? "[ALERT]: No book titles found!!"
            : "Book titles:");

        foreach (string title in BookTitles)
            Write($"'{title}',\n");
        ReadKey();
    }

    /// <summary>
    /// Continues to loop requests from the user until a valid entry is entered
    /// </summary>
    /// <param name="request">What the user is prompt</param>
    /// <param name="isUrl">Adds a validation step to see if this is a valid url</param>
    /// <param name="isEmail">Adds a validation step to see if this is a valid email</param>
    /// <returns>The user's valid response</returns>
    private string Ask(string request, bool isUrl = false, bool isEmail = false)
    {
        Write(request);
        string response = ReadLine().Trim().ToLower();

        if (string.IsNullOrWhiteSpace(response))
        {
            WriteLine($"[Invalid Input]: value can't be empty, or only contains whitespace, entered value = '{response}'");
            return Ask(request);
        }

        if(isEmail && !Validator.IsEmail(response))
        {
            WriteLine($"[INVAID INPUT]: Received invalid email, entered value = '{response}'");
            return Ask(request);
        }

        return response;
    }
}