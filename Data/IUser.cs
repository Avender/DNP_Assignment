using Models;

namespace Assigment_1.Data
{
    public interface IUser
    {
        Adult ValidateUser(string userName, string password);
    }
}