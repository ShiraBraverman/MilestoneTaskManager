namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id; ;
    }

    public void Delete(int id)
    {
        DataSource.Dependencies.Remove(Read(e => e.Id == id));
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter!);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        return filter == null ? DataSource.Dependencies.Select(item => item) : DataSource.Dependencies.Where(filter!);
    }

    public void Update(Dependency item)
    {
        var existingDependency = Read(d => d.Id == item.Id);
        if (existingDependency is null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");

        if (existingDependency.DependsOnTask != 0)
            throw new DalDeletionImpossible($"Dependency with ID={item.Id} is indelible entity");

        DataSource.Dependencies.Remove(existingDependency);
        DataSource.Dependencies.Add(item);
    }

    public void Reset()
    {
        DataSource.Dependencies.Clear();
    }
}