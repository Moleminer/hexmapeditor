using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HexMapEditor.Models;
using HexMapEditor.Data;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Html;
using System.Text.Json.Nodes;

namespace HexMapEditor.ViewComponents;

public class RenderMapViewComponents : ViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync(
                                            int maxPriority, bool isDone)
    {
        // var items = await GetItemsAsync(maxPriority, isDone);
        return View();
    }

    // private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
    // {
    //     return db!.ToDo!.Where(x => x.IsDone == isDone &&
    //                          x.Priority <= maxPriority).ToListAsync();
    // }
}
