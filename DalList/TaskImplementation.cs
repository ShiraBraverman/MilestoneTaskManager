namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        var taskToDelete = Read(t => t.Id == id);
        if (taskToDelete is null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");

        if (DataSource.Dependencies.Any(d => d.DependsOnTask == id))
            throw new DalDeletionImpossible($"Task with ID={id} has a depends task");

        DataSource.Tasks.Remove(taskToDelete);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter!);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        return filter == null ? DataSource.Tasks.Select(item => item) : DataSource.Tasks.Where(filter!);
    }

    public void Update(Task item)
    {
        var existingTask = Read(t => t.Id == item.Id);
        if (existingTask is null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        DataSource.Tasks.Remove(existingTask);
        DataSource.Tasks.Add(item);
    }

    public void Reset()
    {
        DataSource.Tasks.Clear();
    }
}