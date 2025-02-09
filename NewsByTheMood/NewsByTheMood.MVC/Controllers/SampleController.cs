/*using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;

namespace NewsByTheMood.MVC.Controllers
{
    //[NonController]
    public class SampleController : Controller
    {
        *//*public IActionResult Index()
        {
            return View();
        }*//*

        //Non routed
        [NonAction]
        public IActionResult NonAction()
        {
            return Ok();
        }

        //Non routed
        private IActionResult NonAction1()
        {
            return Ok();
        }

        //Bad practice
        [ActionName("NonAction3")]
        public IActionResult NonAction2()
        {
            return Ok();
        }

        //The ChildActionOnly attribute ensures that an action method can be called only as a child method from within a view.
        //Only in ASP.NET
        //[ChildActionOnly]
        public string Hello(string name)
        {
            return string.IsNullOrWhiteSpace(name) ? "Hello world" : $"Hellow {name}";
        }

        //Get
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Its GET");
            //return new ContentResult();
            //return Content("string...");
            //return new EmptyResult(); //anologue of void -> 200OK
            //return NoContent(); //204
            //return File(); //downloadable file, in general use
            //return new FileContentResult() //byte array
            //return FileStream() //file stream
            //return new ObjectResult(new {A=1, B=2}) //almost never use
            //return StatusCode() //set status code
            //return Unauthorized(); //401
            //return NotFound(); //404
            //return BadREquest() 400
            //
            //... and other for certain http status code
            //return Json(); //almost never used, replaced OK
            //return PartialView();
            //return View(); // return new ViewResult() -> return prepared View()
            //return Redirect(); //RedirectToAction("ActionName", "ControllerName")
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginProcess(SampleModel login)
        {
            return Ok(login);
        }
    }
}
*/