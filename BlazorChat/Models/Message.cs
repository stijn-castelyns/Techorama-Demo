using Microsoft.SemanticKernel.ChatCompletion;
using System.Data;

namespace BlazorChat.Models;

public class Message
{
  public string Text { get; set; }
  public AuthorRole Role { get; set; }
  public int Tokens { get; set; }
  public Message(string text, AuthorRole role, int tokens)
  {
    Text = text;
    Role = role;
    Tokens = tokens;
  }
}