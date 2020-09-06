using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ExploreCalifornia.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCalifornia.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        /* DataContext is set and done, all that's left is to simply inject an instance of the BlogDataContext class
         * into the controllers.
         * Inject BlogDataContext instance into the constructor */
        private readonly BlogDataContext _db;
        public BlogController(BlogDataContext db)
        {
            _db = db;
        }

        /* DbSet type works as a collection, which means that this can be interacted with trough linq statements.
         * Ex get all posts -> _db.Posts.ToArray(); */
        [Route("")]
        public IActionResult Index()
        {
            var posts = _db.Posts.OrderByDescending(x => x.Posted).Take(5).ToArray();

            return View(posts);
        }

        // This is were the user is redirected after creating a post, check bellow function and create.cshtml for details
        [Route(@"{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, string key) 
        {
            var post = _db.Posts.FirstOrDefault(x => x.Key == key);
            /*var post = new Post - hardcoded version..
            {
                Title = "My blog post",
                Posted = DateTime.Now,
                Author = "Dumitru Laurentiu",
                Body = "This is a great blog post, don't you think?"
            };*/

            return View(post);
        }
        //this is called for the view with the post areas..
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }
        //this is called for actually creating a post with the data inserted ( pressing submit button )
        [HttpPost, Route("create")]
        public IActionResult Create(Post post)
        {
            /* Checking for validaiton error in the controller is easy.
             If it's needed to check if the data inserted by the user is proper, checking the ModelState property
            available in every action ( this contains a lot of info of the Model ) */
            if(!ModelState.IsValid)
            {
                return View();
            }

            post.Author = User.Identity.Name;
            post.Posted = DateTime.Now;

            /* Saving the post is a 2 step process, EF works on the unit of work pattern
             * First you tell the dataContext everything that you want to do ( adding a post.. )
             * Second is to tell the DataContext to go ahead and execute what was asked initially */

            _db.Posts.Add(post);
            _db.SaveChanges();

            return RedirectToAction("Post", "Blog", new
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });
        }

        public class CreatePostRequest
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }
}
