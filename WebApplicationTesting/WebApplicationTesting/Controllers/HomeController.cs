using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApplicationTesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static List<Book> lstbooks = new List<Book>()
        {
            new Book() { Id = 1, BookName = "Suresh", Price = 121.5f },
            new Book() { Id = 2, BookName = "Naresh", Price = 123.5f },
            new Book() { Id = 3, BookName = "Mukesh", Price = 125.5f }
        };

        [HttpGet("AllBooks")]
        public List<Book> AllBooks()
        {
            return lstbooks;
        }

        [HttpPost("AddBook")]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Invalid book data");
            }

            newBook.Id = lstbooks.Count + 1;

            lstbooks.Add(newBook);

            // Return the ID of the newly added book
            return CreatedAtAction(nameof(BookInfoById), new { id = newBook.Id }, newBook);
        }

        [HttpGet("BookInfoById")]
        public IActionResult BookInfoById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            var book = lstbooks.Find(b => b.Id == id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            return Ok(book);
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public float Price { get; set; }
    }
}
