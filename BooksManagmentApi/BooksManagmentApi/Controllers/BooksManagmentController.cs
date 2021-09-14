using Amazon.DynamoDBv2.DataModel;
using Amazon.SQS;
using BooksManagmentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BooksManagmentApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BooksManagmentController : ControllerBase
  {
    private readonly IDynamoDBContext _dynamoDbContext;
    private readonly IAmazonSQS _amazonSQS;
    private readonly IConfiguration _configuration;
    private readonly string _sqsUrl;

    public BooksManagmentController(
      IDynamoDBContext dynamoDbContext,
      IAmazonSQS amazonSQS,
      IConfiguration configuration)
    {
      _dynamoDbContext = dynamoDbContext;
      _amazonSQS = amazonSQS;
      _configuration = configuration;
      _sqsUrl = _configuration["SQS:QueueUrl"];
    }

    [HttpGet("GetAllBooks")]
    public async Task<ActionResult<IEnumerable<Books>>> GetAllBooks()
    {
      var condition = new List<ScanCondition>();
      List<Books> books = await _dynamoDbContext.ScanAsync<Books>(condition).GetRemainingAsync();
      return books;
    }

    [HttpGet("GetBook")]
    public async Task<ActionResult<Books>> GetBook(string isbn)
    {
      Books book = await _dynamoDbContext.LoadAsync<Books>(isbn);

      if (book == null)
      {
        return NotFound();
      }

      return book;
    }

    [HttpPost("AddBook")]
    public async Task<ActionResult<Books>> AddBook(Books book)
    {
      try
      {
        await _dynamoDbContext.SaveAsync(book);

        var msg = new SqsMessage() { Date = DateTime.Now, Message = $"New book with ISBN:{book.ISBN} was added;" };
        await _amazonSQS.SendMessageAsync(_sqsUrl, JsonSerializer.Serialize(msg));

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
        await _dynamoDbContext.DeleteAsync(isbn);

        var msg = new SqsMessage() { Date = DateTime.Now, Message = $"Book with ISBN:{isbn} was deleted;" };
        await _amazonSQS.SendMessageAsync(_sqsUrl, JsonSerializer.Serialize(msg));

        return Accepted();
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }

    [HttpPut("UpdateBook")]
    public async Task<ActionResult> UpdatePerson(Books book)
    {
      try
      {
        await _dynamoDbContext.SaveAsync(book);

        var msg = new SqsMessage() { Date = DateTime.Now, Message = $"Updates were applied to book with ISBN:{book.ISBN};" };
        await _amazonSQS.SendMessageAsync(_sqsUrl, JsonSerializer.Serialize(msg));

        return Accepted();
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }
  }
}
