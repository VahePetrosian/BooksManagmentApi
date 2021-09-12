using System;

namespace BooksManagmentApi.Models
{
  public class SqsMessage
  {
    public string Message { get; set; }
    public DateTime Date { get; set; }
  }
}
