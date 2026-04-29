using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Json;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json;
using System;
using System.IO;
using System.Text;

namespace HexMapEditor.Data;


public class AssetList
{
	private readonly string path = @"Content\assetlist.json";
	List<Asset> assetList = [];
	
	public AssetList() {;}
	public AssetList(string jsonInput)
	{
		ParseJson(jsonInput);
		return;
	}

	public void AddAsset(Asset asset)
	{
		try
		{
			assetList.Add(asset);
		} catch (Exception)
		{
			Console.Error.WriteLine("Something went wrong adding asset to asset list");
		}
	}

	public Asset GetAsset(string name)
	{
		try
		{
			return assetList.Find(x => x.Name == name);
		} catch (ArgumentNullException)
		{
			Console.Error.WriteLine("Failed to find asset.");
			return null;
		} catch
		{
			Console.Error.WriteLine("Something went wrong with finding asset.");
			return null;
		}
	}

	public bool SaveToFile()
	{
        string jsonString = ToJson();
		File.WriteAllText(path, jsonString);
		return true;
	}

	public void PullFromFile()
	{
		string fileLines = string.Join("", File.ReadLines(path).ToArray());
		ParseJson(fileLines);
	}


	public string ToJson()
	{
		return JsonSerializer.Serialize(assetList);
	}

	private void ParseJson(string input)
	{
		JsonArray node = JsonNode.Parse(input).AsArray();

		foreach (dynamic item in node)
		{
			// Console.WriteLine(item["Values"].AsArray()[0]);
			assetList.Append(new Asset
			{
				Name = (string)item["Name"],
				Filename = (string)item["Filename"],
				Scale = (double)item["Scale"]
			});
		}
	}

}