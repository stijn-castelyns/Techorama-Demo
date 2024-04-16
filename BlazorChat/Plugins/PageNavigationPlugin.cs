using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace BlazorChat.Plugins;

public class PageNavigationPlugin
{
  private NavigationManager _navigationManager;

  public PageNavigationPlugin(NavigationManager navigationManager)
  {
    _navigationManager = navigationManager;
  }

  [KernelFunction, Description("Navigates the user to a different web page on the same website")]
  public string NavigateUser([Description("The route to navigate to")] string route)
  {
    _navigationManager.NavigateTo(route);
    return "Correctly navigated";
  }
}
