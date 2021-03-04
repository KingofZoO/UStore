using System;
using System.Collections.Generic;

namespace UStore.Models {
    public class Role {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<User> Users { get; set; }

        public Role() {
            Users = new List<User>();
        }
    }

    public static class RoleTypes {
        public const string AdminRole = "admin";
        public const string UserRole = "user";
    }
}
