using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoleDao
    {
        public static List<Role> GetAllRoles()
        {
            var listRoles = new List<Role>();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    listRoles = context.Roles.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listRoles;
        }
        public static Role FindRoleById(int roleId)
        {
            Role p = new Role();
            try
            {
                using (var context = new PRN231_ProjectContext())
                {
                    p = context.Roles//.Include(x => x.Category)
                        .SingleOrDefault(x => x.RoleId == roleId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return p;
        }
    }
}
