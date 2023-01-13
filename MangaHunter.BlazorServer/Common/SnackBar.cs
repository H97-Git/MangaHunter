using ErrorOr;

using MudBlazor;

namespace MangaHunter.BlazorServer.Common;

public static class SnackBar
{
    public static void HandleResult<T>(ErrorOr<T> result,ISnackbar snackbar)
    {
        if (result.IsError)
        {
            snackbar.Add("Failed", Severity.Error);
            snackbar.Add(result.FirstError.ToString(), Severity.Error);
        }
        else
        {
            snackbar.Add("Succeeded", Severity.Success);
        }
    }
}