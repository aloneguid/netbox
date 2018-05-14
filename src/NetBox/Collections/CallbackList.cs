using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Collections
{
   /// <summary>
   /// Calls back on common list operations, allowing to get a notification without subclassing the whole plethora of methods.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class CallbackList<T> : IList<T>
   {
      private readonly List<T> _list;

      /// <summary>
      /// Called when an element is about to be added. You have a chance to override which element value is to be added by
      /// returning an element different from the one that is passed in.
      /// </summary>
      public Func<T, T> OnAdd { get; set; }

      /// <summary>
      /// Called when an element is about to be inserted. You have a chance to override which element is to be inserted by
      /// returning an element different from the one that is passed in.
      /// </summary>
      public Func<int, T, T> OnInsert { get; set; }

      /// <summary>
      /// Called when an element is removed
      /// </summary>
      public Action<T> OnRemove { get; set; }

      /// <summary>
      /// Called when an element is removed at a specific position
      /// </summary>
      public Action<int> OnRemoveAt { get; set; }

      /// <summary>
      /// Called when the list is cleared
      /// </summary>
      public Action OnClear { get; set; }

      /// <summary>
      /// Creates a new instance of <see cref="CallbackList{T}"/>
      /// </summary>
      public CallbackList()
      {
         _list = new List<T>();
      }

      /// <summary>
      /// Creates a new instance of <see cref="CallbackList{T}"/>
      /// </summary>
      public CallbackList(IEnumerable<T> collection)
      {
         _list = new List<T>(collection);
      }

      /// <summary>
      /// Creates a new instance of <see cref="CallbackList{T}"/>
      /// </summary>
      public CallbackList(int capacity)
      {
         _list = new List<T>(capacity);
      }

      #region [ IList methods ]

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public T this[int index] { get => _list[index]; set => _list[index] = value; }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public int Count => _list.Count;

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public bool IsReadOnly => false;

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public void Add(T item)
      {
         if (OnAdd != null) item = OnAdd(item);

         _list.Add(item);
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public void Clear()
      {
         OnClear?.Invoke();

         _list.Clear();
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public bool Contains(T item)
      {
         return _list.Contains(item);
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public void CopyTo(T[] array, int arrayIndex)
      {
         _list.CopyTo(array, arrayIndex);
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public IEnumerator<T> GetEnumerator()
      {
         return _list.GetEnumerator();
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public int IndexOf(T item)
      {
         return _list.IndexOf(item);
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public void Insert(int index, T item)
      {
         if (OnInsert != null) item = OnInsert(index, item);

         _list.Insert(index, item);
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public bool Remove(T item)
      {
         OnRemove?.Invoke(item);

         return _list.Remove(item);
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      public void RemoveAt(int index)
      {
         OnRemoveAt?.Invoke(index);

         _list.RemoveAt(index);
      }

      /// <summary>
      /// Overriden from <see cref="IList{T}"/>
      /// </summary>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return _list.GetEnumerator();
      }

      #endregion
   }
}
