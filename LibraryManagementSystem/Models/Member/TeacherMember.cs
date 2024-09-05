using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Member;

internal class TeacherMember(
    string firstName,
    string lastName,
    string email) : Member(firstName: firstName, lastName: lastName, email: email, type: MemberType.Teacher)
{
    public string TeacherId { get; } = CustomUtils.GenerateUniqueID(0, 8);

    public override string ToString() => base.ToString() + $"\n\tteacher id: '{TeacherId}'";
}