﻿using System.Text.Json.Serialization;

using MangaHunter.Contracts.Mangadex.Models.Types;

namespace MangaHunter.Contracts.Mangadex.Models.Thread;

/// <summary>
/// Represents a request to create a thread
/// </summary>
public class ThreadCreate
{
	/// <summary>
	/// The type of thread to create
	/// </summary>
	[JsonPropertyName("type")]
	public ThreadType Type { get; set; }

	/// <summary>
	/// The ID of the object to create the thread for
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;
}
