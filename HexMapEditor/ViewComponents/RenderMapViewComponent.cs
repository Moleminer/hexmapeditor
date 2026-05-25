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

[ViewComponent]
public class RenderMapViewComponent : ViewComponent
{

    // public async Task<IViewComponentResult> InvokeAsync(
    //                                         HtmlString tileMapString, HtmlString assetList)
    // {
	// 	RenderMapViewModel viewModel = new(){
	// 		TileMapString = tileMapString,
	// 		AssetList = assetList
	// 		// TODO: Could put the svg element in here?
	// 	};
    //     // var items = await GetItemsAsync(maxPriority, isDone);
    //     return View(viewModel);
    // }

	public IViewComponentResult Invoke(HtmlString tileMapString, HtmlString assetList)
        {
 
            RenderMapViewModel viewModel = new(){
			TileMapString = tileMapString,
			AssetList = assetList
			// TODO: Could put the svg element in here?
		};
        // var items = await GetItemsAsync(maxPriority, isDone);
        return View(viewModel);
        }


    // private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
    // {
    //     return db!.ToDo!.Where(x => x.IsDone == isDone &&
    //                          x.Priority <= maxPriority).ToListAsync();
    // }
}
