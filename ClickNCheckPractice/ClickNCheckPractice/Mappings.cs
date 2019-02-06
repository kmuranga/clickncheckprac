using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickNCheckPractice
{
    public static class Mappings
    {
        public static UsersTableEntity ToTableEntity(this Users users)
        {
            return new UsersTableEntity()
            {
                PartitionKey = "Users",
                RowKey = users.Id,
                FirstName = users.FirstName,
                LastName = users.LastName,
                Username = users.Username,
                Password = users.Password,
                Salt = users.Salt
            };
        }

        public static Users ToUsers(this UsersTableEntity todo)
        {
            return new Users()
            {
                Id = todo.RowKey,
                FirstName = todo.FirstName,
                LastName = todo.LastName,
                Username = todo.Username,
                Password = todo.Password,
                Salt = todo.Salt
            };
        }
    }
}
