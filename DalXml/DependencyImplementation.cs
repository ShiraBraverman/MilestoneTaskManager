
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

internal class DependencyImplementation : IDependency
{
    const string filePath = @"dependencies";

    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;

        XElement dependenciesElement = XMLTools.LoadListFromXMLElement(filePath);

        XElement newDependencyElement = new XElement("Dependency",
             new XElement("Id", id),
             new XElement("DependentTask", item.DependentTask),
             new XElement("DependsOnTask", item.DependsOnTask)
         );

        dependenciesElement.Add(newDependencyElement);
        XMLTools.SaveListToXMLElement(dependenciesElement, filePath);
        return id;
    }

    public void Delete(int id)
    {
        XElement dependenciesElement = XMLTools.LoadListFromXMLElement(filePath);

        var dependencyToDelete = dependenciesElement.Elements("Dependency")
            .FirstOrDefault(d => (int)d.Element("Id") == id);

        if (dependencyToDelete != null)
        {
            dependencyToDelete.Remove();
            XMLTools.SaveListToXMLElement(dependenciesElement, filePath);
        }
    }


    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;

        XElement? dependencyElement = allDependencies?
                    .Elements("Dependency")
                    .FirstOrDefault(dependency => filter(new Dependency(
                        (int)dependency.Element("Id")!,
                        (int)dependency.Element("DependentTask")!,
                        (int)dependency.Element("DependsOnTask")!
                    )));

        if (dependencyElement != null)
        {
            Dependency? dependency = new Dependency(
                (int)dependencyElement.Element("Id")!,
                (int)dependencyElement.Element("DependentTask")!,
                (int)dependencyElement.Element("DependsOnTask")!
            );
            return dependency;
        }
        return null;
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement? dependenciesElement = XMLTools.LoadListFromXMLElement("dependencies");

        IEnumerable<Dependency> dependencies = dependenciesElement
            .Elements("Dependency")
            .Select(e => new Dependency(
                Id: (int)e.Element("Id")!,
                DependentTask: (int)e.Element("DependentTask")!,
                DependsOnTask: (int)e.Element("DependsOnTask")!
            ));

        if (filter != null)
        {
            dependencies = dependencies.Where(filter);
        }

        return dependencies;
    }

    public void Update(Dependency item)
    {
        XElement rootElement = XMLTools.LoadListFromXMLElement(filePath);

        XElement depElement = (from d in rootElement.Elements("Dependency")
                               where (int)d.Element("Id") == item.Id
                               select d).SingleOrDefault()!;

        if (depElement != null)
        {
            depElement.Element("DependentTask").SetValue(item.DependentTask);
            depElement.Element("DependsOnTask").SetValue(item.DependsOnTask);
        }

        XMLTools.SaveListToXMLElement(rootElement, filePath);
    }

    public void Reset()
    {
        //List<Dependency> dependencies = new List<Dependency>();
        //XMLTools.SaveListToXMLSerializer(dependencies, filePath);

        XElement arrayOfDependency = new XElement("ArrayOfDependency");

        if (File.Exists(@"..\xml\dependencies.xml"))
        {
            File.Delete(@"..\xml\dependencies.xml");
        }

        arrayOfDependency.Save(@"..\xml\dependencies.xml");

        string configFile = "data-config";
        XElement configElement = XMLTools.LoadListFromXMLElement(configFile);
        configElement.Element("NextDependencyId")?.SetValue("1");
        XMLTools.SaveListToXMLElement(configElement, configFile);
    }
}
