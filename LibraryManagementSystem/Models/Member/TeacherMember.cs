using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Member
{
    internal class TeacherMember : Member
    {
        public string TeacherId { get; } = CustomUtils.GenerateUniqueID(0, 8);

        public TeacherMember(
            string firstName,
            string lastName,
            string email) : base(firstName: firstName, lastName: lastName, email: email, type: MemberType.Teacher)
        {
        }

        public override string ToString() => base.ToString() + $"\n\tteacher id: '{TeacherId}'";
    }
}