using System;
using System.ComponentModel.DataAnnotations;

namespace UStore.Models.Authorization {
    public class LoginModel {
        [Required(ErrorMessage = "Введите номер телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
