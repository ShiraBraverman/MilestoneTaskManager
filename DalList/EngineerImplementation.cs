namespace Dal;
using DalApi;
    using DO;
    using System;
    using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        int id = item.Id;
        if (DataSource.Engineers.Any(e => e.Id == id))
            throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");

        DataSource.Engineers.Add(item);
        return id;
    }

    public void Delete(int id)
    {
        //if ((Read(t => t.Id == id)) != null)
        //{
        //    if ((DataSource.Tasks.Find(t => t.EngineerId == id)) == null)
        //    {
        //        DataSource.Engineers.Remove(DataSource.Engineers.Find(t => t.Id == id));
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException($"Cannot delete engineer with ID:{id} because he have task.");
        //    }
        //}
        //else
        //{
        //    throw new InvalidOperationException($"Engineer with ID:{id} does not exists.");
        //}
        var engineerToDelete = Read(e => e.Id == id);
        if (engineerToDelete is null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");

        if (DataSource.Tasks.Any(t => t.EngineerId == id))
            throw new DalDeletionImpossible($"Engineer with ID={id} has a task to do");

        DataSource.Engineers.Remove(engineerToDelete);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter!);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        return filter == null ? DataSource.Engineers.Select(item => item) : DataSource.Engineers.Where(filter!);
    }

    public void Update(Engineer item)
    {
        var existingEngineer = Read(e => e.Id == item.Id);
        if (existingEngineer is null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");

        DataSource.Engineers.Remove(existingEngineer);
        DataSource.Engineers.Add(item);
    }

    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}