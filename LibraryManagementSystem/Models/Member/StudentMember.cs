using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Member;

internal class StudentMember(
    string firstName,
    string lastName,
    string email) : Member(firstName: firstName, lastName: lastName, email: email, type: MemberType.Student)
{
    public string StudentId { get; } = CustomUtils.GenerateUniqueID(0, 8);

    public override string ToString() => base.ToString() + $"\n\tstudent id: '{StudentId}'";
}