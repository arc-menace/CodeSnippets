using CodeSnippets.Data.Entities;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Mappings
{
    public class AutoMapper : DefaultAutomappingConfiguration
    {
        public override bool IsId(Member member)
        {
            return member.Name == "Id";
        }
    }
}
