using System.Collections;
using System.Collections.Generic;

public class SelectedList : IList<Selectable> {
    readonly List<Selectable> _list = new List<Selectable>();
    public IEnumerator<Selectable> GetEnumerator() {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public void Add(Selectable item) {
        if (item) {
            item.ToggleSelection(true);
        }
        _list.Add(item);
    }

    public void Clear() {
        foreach (var selectable in _list) {
            selectable.ToggleSelection(false);
        }
        _list.Clear();
    }

    public bool Contains(Selectable item) {
        return _list.Contains(item);
    }

    public void CopyTo(Selectable[] array, int arrayIndex) {
        foreach (var selectable in array) {
            selectable.ToggleSelection(true);
        }
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(Selectable item) {
        var index = IndexOf(item);
        if (index != -1) {
            _list[index].ToggleSelection(false);
        }
        return _list.Remove(item);
    }

    public int Count => _list.Count;
    public bool IsReadOnly => false;
    public int IndexOf(Selectable item) {
        return _list.IndexOf(item);
    }

    public void Insert(int index, Selectable item) {
        _list.Insert(index, item);
    }

    public void RemoveAt(int index) {
        if (index < Count) {
            _list[index].ToggleSelection(false);
        }
    }

    public Selectable this[int index] {
        get => _list[index];
        set => _list[index] = value;
    }
}
