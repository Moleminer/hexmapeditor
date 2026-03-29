using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HexMapEditor.Data;


public static class Users
{
	
	public static JsonObject GetUsers()
	{
		StreamReader sr = new StreamReader(@"Content\users.json");
		
		JsonObject j = JsonNode.Parse(sr.ReadToEnd()).AsObject();
		// Console.WriteLine(j[0].AsObject());
		return j[0].AsObject();
	}

	public static string ToJson()
	{
		return System.Text.Json.JsonSerializer.Serialize("grid");
	}

}