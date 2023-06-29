
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_DataAccess.Repository
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        private ApplicationDbContext _db;
        public UserRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(UserRole obj)
        {
            _db.UserRoles.Update(obj);
        }
    }
}
