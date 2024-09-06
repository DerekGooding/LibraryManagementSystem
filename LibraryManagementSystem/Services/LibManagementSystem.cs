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

    public void RegisterMember()
    {
        // TODO: Validate member input details

        string firstName = Ask("Enter first name: ");
        string lastName = Ask("Enter last name: ");
        string email = Ask("Enter email: ", isEmail: true);
        Member.MemberType? memberType = Member.SelectType();

        if (memberType == null)
        {
            WriteLine("[ERROR]: Error while selecting member type");
        }
        else if (memberType == Member.MemberType.Student)
        {
            HandleAddStudent(firstName, lastName, email);
        }
        else if (memberType == Member.MemberType.Teacher)
        {
            HandleAddTeacher(firstName, lastName, email);
        }
        else
        {
            WriteLine("[ERROR]: Received invalid member type while registering member in the system.");
        }

        ReadKey();
    }
    private void HandleAddStudent(string firstName, string lastName, string email)
    {
        StudentMember newMember = new(firstName, lastName, email);
        if (!Members.Add(newMember))
        {
            WriteLine($"[ALERT]: member with email = '{email.Trim().ToLower()}' has already been registered in the system!!");
        }
        else
        {
            WriteLine("[SUCCESS]: Student member successfully registered with following details: !!");
            WriteLine(newMember);
        }
    }
    private void HandleAddTeacher(string firstName, string lastName, string email)
    {
        TeacherMember newMember = new(firstName, lastName, email);
        if (!Members.Add(newMember))
        {
            WriteLine($"[ALERT]: member with email = '{email.Trim().ToLower()}' has already been registered in the system!!");
        }
        else
        {
            WriteLine("[SUCCESS]: Teacher member successfully registered with following details: !!");
            WriteLine(newMember);
        }
    }

    private bool FindMemberByEmail(string email, out Member? result)
    {
        result = null;

        if (!Validator.IsEmail(ref email))
            return false;

        email = email.Trim();
        result = Members.FirstOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        return result != null;
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

        WriteLine(Books.Add(newBook)
                ? "[SUCCESS] : Physical book has been successfully added to the system!!)"
                : "[ALERT]: Physical book with the following details already exists in the system!!");
        WriteLine(newBook);
    }
    private void HandleAddEBook(string title, string author)
    {
        string link = Ask("Enter book download link: ", isUrl: true);
        Book newBook = new EBook(title: title, author: author, downloadLink: link);

        WriteLine(Books.Add(newBook)
            ? "[SUCCESS] : E-book book has been successfully added to the system!!)"
            : "[ALERT]: E-book book with the following details already exists in the system!!");
        WriteLine(newBook);
    }

    public void BorrowBook()
    {
        string email = Ask("Enter member email: ", isEmail: true);

        if (!FindMemberByEmail(email, out Member? member))
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
        else if (!member.BorrowBook(id: book.BookId, out bool validationError))
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

        if (!FindMemberByEmail(memberEmail, out Member? member))
        {
            WriteLine($"[NOT FOUND ERROR]: Operation failed because member with email = '{memberEmail}' doesn't exists in the system");
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
    /// <param name="isEmail">Adds a validation step to see if this is a valid email</param>
    /// <param name="isUrl">Adds a validation step to see if this is a valid url</param>
    /// <returns>The user's valid response</returns>
    private string Ask(string request, bool isEmail = false, bool isUrl = false)
    {
        Write(request);
        string response = ReadLine().Trim().ToLower();

        if (string.IsNullOrWhiteSpace(response))
        {
            WriteLine($"[Invalid Input]: value can't be empty, or only contains whitespace, entered value = '{response}'");
            return Ask(request);
        }

        if(isEmail && !Validator.IsEmail(ref response))
        {
            WriteLine($"[INVAID INPUT]: Received invalid email, entered value = '{response}'");
            return Ask(request);
        }

        if (isUrl && !Validator.IsURL(ref response))
        {
            WriteLine($"[Invalid Input]: book download link URL is invalid, entered value = '{response}'");
            return Ask(request);
        }

        return response;
    }
}