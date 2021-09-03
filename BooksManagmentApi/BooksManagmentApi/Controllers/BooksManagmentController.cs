using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManagmentApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BooksManagmentController : ControllerBase
  {
    private readonly ILogger<BooksManagmentController> _logger;

    private List<Book> testData = new List<Book>
    {
      new Book { ISBN = "number", Title = "title", Description = "desc" },
      new Book { ISBN = "number", Title = "title", Description = "desc" },
      new Book { ISBN = "number", Title = "title", Description = "desc" }
    };

    public BooksManagmentController(ILogger<BooksManagmentController> logger)
    {
      _logger = logger;
    }

    [HttpGet("GetAllBooks")]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
    {
      //return db.Books.ToList();
      return testData;
    }

    [HttpGet("GetBook")]
    public async Task<ActionResult<Book>> GetBook(string isbn)
    {
      //var book = db.Find(id);
      var book = testData.Where(r => r.ISBN == isbn).FirstOrDefault();
      if (book == null)
      {
        return NotFound();
      }

      return book;
    }

    [HttpPost("AddBook")]
    public async Task<ActionResult<Book>> AddBook(Book book)
    {
      try
      {
        //db.Add(book);
        return Accepted();
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }

    [HttpDelete("DeleteBook")]
    public async Task<ActionResult> DeleteBook(string isbn)
    {
      try
      {
        //db.Delete(isbn);
        return Accepted();
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }

    [HttpPut("UpdateBook")]
    public async Task<ActionResult> UpdatePerson(Book book)
    {
      try
      {
        //db.Update(book);
        return Accepted();
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }
  }
}
