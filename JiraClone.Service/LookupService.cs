using AutoMapper;
using JiraClone.Data;
using JiraClone.Service.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service
{
    public class LookupService
    {
        private List<UnitTreeDto> BuildTree(IEnumerable<UnitTreeDto> allUnits, int? rootUnit = null)
        {
            var tree = new List<UnitTreeDto>();

            List<UnitTreeDto> roots = new List<UnitTreeDto>();
            if (rootUnit == null)
            {
                roots = allUnits.Where(x => x.ParentId == null).ToList();
            }
            else
            {
                var rootNode = allUnits.FirstOrDefault(x => x.Id == rootUnit);
                if (rootNode != null)
                {
                    roots.Add(rootNode);
                }
            }

            foreach (var r in roots)
            {
                tree.Add(r);
                UnitTreeDto nextNode = r;
                UnitTreeDto currentNode;
                UnitTreeDto parentNode;
                int index;
                int level = 1;


                while (nextNode != null)
                {
                    currentNode = nextNode;
                    nextNode = null;

                    foreach (var child in allUnits.Where(x => x.ParentId == currentNode.Id))
                    {
                        child.Parent = currentNode;
                        currentNode.Children.Add(child);
                    }

                    if (currentNode.Children.Any())
                    {
                        nextNode = currentNode.Children[0];
                        level++;
                    }
                    else
                    {
                        while (currentNode.Parent != null)
                        {
                            parentNode = currentNode.Parent;
                            index = parentNode.Children.IndexOf(currentNode);
                            if (index < parentNode.Children.Count - 1)
                            {
                                nextNode = parentNode.Children[index + 1];
                                break;
                            }
                            else
                            {
                                currentNode = parentNode;
                                level--;
                            }
                        }
                    }
                }
            }

            return tree;
        }

    }
}
