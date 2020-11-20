
namespace Access.Primitives.EFCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;


    /// <summary>
    /// An ICollection implementation over DbSet which allows us to work with the abstraction of a collection
    /// while being backed by a DbSet. This is mostly used in a bounded domain context.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class EFList<T> : ICollection<T>
        where T : class
    {
        private readonly DbSet<T> dbSet;

        public EFList(DbSet<T> dbSet)
        {
            this.dbSet = dbSet;
        }

        public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public int Count => dbSet.Local.Count;
        
        public bool IsReadOnly => dbSet.Local.IsReadOnly;
        
        public void Add(T item)
        {
            dbSet.Add(item);
        }
        
        public void Clear()
        {
            dbSet.Local.Clear();
        }
        
        public bool Contains(T item)
        {
            return dbSet.Local.Contains(item);
        }
        
        public void CopyTo(T[] array, int arrayIndex)
        {
            dbSet.Local.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dbSet.Local.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return dbSet.Remove(item) != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dbSet.Local.GetEnumerator();
        }
    }
}
