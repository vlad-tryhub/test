using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeAssignment.Orchestrator;

// Entity class used to store crawlers' results in the database
[BsonIgnoreExtraElements] // this attribute is needed for entity classes without '_id' field
public class TitleEntity
{
    public string Url { get; set; }
    public string Title { get; set; }
    public DateTime ProcessedAt { get; set; }
}