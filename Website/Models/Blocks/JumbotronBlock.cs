using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer;

namespace BaseSite.Models.Blocks
{
    [ContentType(DisplayName = "Jumbotron", GUID = "266895d7-46fa-44b9-b009-0b3f57b9e1f0", Description = "")]
    public class JumbotronBlock : CalloutBlock
    {
    
    }
}