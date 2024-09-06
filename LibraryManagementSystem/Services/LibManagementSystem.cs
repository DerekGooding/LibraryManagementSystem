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
        if (!Validator.IsValidEmail(email))
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

        if (!Validator.IsValidEmail(email))
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

        // book type input
        switch (Book.SelectBookTypeUsingMenuSelector())
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

        bool isValidDownloadLink = Validator.IsValidURL(downloadLinkInput, out string downloadLink);
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
        // member email input
        Write("Enter member email: ");
        string memberEmail = ReadLine().Trim();

        // validation: member email input
        if (!Validator.IsValidEmail(memberEmail))
        {
            WriteLine($"[INVALID INPUT]: Received invalid email, entered value = '{memberEmail}'");
            return;
        }

        // checking if member exists
        bool memberExists = FindMemberByEmail(memberEmail, out Member member);
        if (!memberExists)
        {
            WriteLine($"[NOT FOUND ERROR]: Operation failed because member with email = '{memberEmail}' doesn't exists in the system");
            WriteLine("[SYSTEM SUGGESTION]: Signup by creating a new member in the system");
            return;
        }

        // asking book details
        // book title input
        Write("Enter book title: ");
        string bookTitle = ReadLine().Trim().ToLower();

        // validating book title
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            WriteLine($"[Invalid Input]: book title can't be empty, or only contains whitespace, entered value = '{bookTitle}'");
            return;
        }

        // book author input
        Write("Enter book author: ");
        string bookAuthor = ReadLine().Trim().ToLower();

        // validating book author
        if (string.IsNullOrWhiteSpace(bookAuthor))
        {
            WriteLine($"[Invalid Input]: book author can't be empty, or only contains whitespace, entered value = '{bookAuthor}'");
            return;
        }

        // book type input
        bool bookTypeSelectionSuccess = Book.SelectBookTypeUsingMenuSelector(out Book.BookType selectedBookType);

        // system error
        if (!bookTypeSelectionSuccess)
        {
            WriteLine("[ERROR]: Error while book type selection.");
            return;
        }

        // checking if book exists with given book details
        Book book = Books.FirstOrDefault(_book => _book.Title.Equals(bookTitle) && _book.Author.Equals(bookAuthor) && _book.Type.Equals(selectedBookType));
        if (book == null)
        {
            WriteLine("[NOT FOUND]: book with entered details doesn't exist in the system!!");
            return;
        }

        // check if book has already been borrowed
        if (book.IsBorrowed)
        {
            WriteLine("[ALERT]: book with entered details has already been borrowed!!");
            return;
        }

        // Add book to the 'borrowed books list' of member
        bool memberBorrowBookOperationSuccess = member.BorrowBook(bookId: book.BookId, out bool validationError);
        if (!memberBorrowBookOperationSuccess)
        {
            if (validationError)
            {
                WriteLine("[SYSTEM ERROR]: Error while borrowing book to member");
                return;
            }
            WriteLine("[ALERT]: Book with given details has already been borrowed by the member");
            return;
        }

        // set book isBorrowed to true
        book.BorrowBook();

        WriteLine($"[SUCCESS]: Book with title: '{bookTitle}' has been successfully borrowed by member with name: '{member.Name}' and email: '{member.Email}'");
    }

    public void ReturnBook()
    {
        // member email input
        Write("Enter member email: ");
        string memberEmail = ReadLine().Trim();

        // validation: member email input
        if (!Validator.IsValidEmail(memberEmail))
        {
            WriteLine($"[INVALID INPUT]: Received invalid email, entered value = '{memberEmail}'");
            return;
        }

        // checking if member exists
        bool memberExists = FindMemberByEmail(memberEmail, out Member member);
        if (!memberExists)
        {
            WriteLine($"[NOT FOUND ERROR]: Operation failed because member with email = '{memberEmail}' doesn't exists in the system");
            WriteLine("[SYSTEM SUGGESTION]: Signup by creating a new member in the system");
            return;
        }

        // asking book details
        // book title input
        Write("Enter book title: ");
        string bookTitle = ReadLine().Trim().ToLower();

        // validating book title
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            WriteLine($"[Invalid Input]: book title can't be empty, or only contains whitespace, entered value = '{bookTitle}'");
            return;
        }

        // book author input
        Write("Enter book author: ");
        string bookAuthor = ReadLine().Trim().ToLower();

        // validating book author
        if (string.IsNullOrWhiteSpace(bookAuthor))
        {
            WriteLine($"[Invalid Input]: book author can't be empty, or only contains whitespace, entered value = '{bookAuthor}'");
            return;
        }

        // book type input
        bool bookTypeSelectionSuccess = Book.SelectBookTypeUsingMenuSelector(out Book.BookType selectedBookType);

        // system error
        if (!bookTypeSelectionSuccess)
        {
            WriteLine("[ERROR]: Error while book type selection.");
            return;
        }

        // checking if book exists with given book details
        Book book = Books.FirstOrDefault(_book => _book.Title.Equals(bookTitle) && _book.Author.Equals(bookAuthor) && _book.Type.Equals(selectedBookType));
        if (book == null)
        {
            WriteLine("[NOT FOUND]: book with entered details doesn't exist in the system!!");
            return;
        }

        // check if book was previously borrowed or not
        if (!book.IsBorrowed)
        {
            WriteLine("[ERROR]: book with entered details was never borrowed!!");
            return;
        }

        //  check if book borrowed by member by trying to return book
        string bookId = book.BookId;
        if (!member.TryReturnBook(bookId, out bool validationError))
        {
            if (validationError)
            {
                WriteLine("[SYSTEM ERROR]: Error while doing operation");
                return;
            }

            WriteLine("[ERROR]: Book wasn't borrowed by you!!");
            return;
        }

        book.ReturnBook();

        WriteLine($"Book with given details and borrowed by '{member.Name}' has been successfully returned!");
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

    private string Ask(string request)
    {
        Write(request);
        string response = ReadLine().Trim().ToLower();

        if (string.IsNullOrWhiteSpace(response))
        {
            WriteLine($"[Invalid Input]: value can't be empty, or only contains whitespace, entered value = '{response}'");
            return Ask(request);
        }

        return response;
    }
}