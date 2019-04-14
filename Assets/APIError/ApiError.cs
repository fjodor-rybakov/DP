using Microsoft.AspNetCore.Mvc;

namespace APIError
{
    public class Error
    {
        public string Message;
    }
    
    public class ApiError : Controller
    {
        public IActionResult IncorrectUidOrLimit => BadRequest(new Error{Message = "Такого uid не существует или закончился лимит сообщений"});
    }
}
