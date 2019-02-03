using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class DiscriminatorExtractor
    {
        public static List<(string tableName, string discriminatorColumName)> GetDiscriminators<TContext>(this TContext context) where TContext : DbContext
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            var entitySets = metadata.GetItems<EntityContainer>(DataSpace.CSpace).Single().EntitySets;
            var entityTypes = metadata.GetItems<EntityType>(DataSpace.SSpace);

            var discriminators = new List<(string, string)>(entityTypes.Count);
            foreach (var entitySet in entitySets)
            {
                var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace).Single().EntitySetMappings.Single(s => s.EntitySet == entitySet);
                if (mapping.EntityTypeMappings.Count > 0)
                {
                    var hierarchyMapping = mapping.EntityTypeMappings.SingleOrDefault(etm => etm.IsHierarchyMapping);
                    if (hierarchyMapping == null)
                        continue;

                    var conditions = mapping.EntityTypeMappings.SelectMany(etm => etm.Fragments.Single().Conditions).OfType<ValueConditionMapping>().ToList();
                    if (conditions.Select(cc => cc.Column).Distinct().Count() > 1)
                    {
                        Debug.WriteLine($"{mapping.EntitySet.Name} has multiple mappings one of them being a hierachy mapping, but the fragments' conditions refer more than one distinct edm property");
                        continue;
                    }

                    if (conditions.Select(cc => cc.Column).Distinct().Count() < 1)
                    {
                        Debug.WriteLine($"{mapping.EntitySet.Name} has a hierachy mapping, but none of the fragments' conditions are ValueConditionMappings");
                        continue;
                    }

                    var table = hierarchyMapping.Fragments.Single().StoreEntitySet;
                    string tableName = $"{table.Schema}.{(string)table.MetadataProperties["Table"].Value ?? table.Name}";
                    discriminators.Add((tableName, conditions.First().Column.Name));
                }
            }
            return discriminators;
        }
    }
}
