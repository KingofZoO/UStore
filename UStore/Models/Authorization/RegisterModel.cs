using System;
using System.ComponentModel.DataAnnotations;

namespace UStore.Models.Authorization {
    public class RegisterModel {
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Phone(ErrorMessage = "Введите номер телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
