using BusinessObjects.Models;

namespace Repositories.Interface { 
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(int uId);
        void DeleteUser(User u);
        void InsertUser(User u);
        void UpdateProduct(User u);
        public User checkLogin(string email, string password);
        public string GetRoleByEmail(string email);
        public User GetUserByEmail(string email);
        //temporary.FutureToDo: create IRoleRepository
        List<Role> GetAllRoles();
        Role GetRoleById(int rid);
    }
}
